using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gdbcLeaderBoard.Data;
using gdbcLeaderBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace gdbcLeaderBoard.Controllers
{
    [Authorize(Roles = "Xpirit")]
    [Route("api/challenge")]
    public class ChallengesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private string _token;
        private string _url;
        private ILogger _logger;

        public ChallengesController(ApplicationDbContext context, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _context = context;

            _token = configuration.GetConnectionString("Token");
            _url = configuration.GetConnectionString("VSTSUrl");
            _logger = loggerFactory.CreateLogger<ChallengesController>();
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]WorkItemUpdate item)
        {
            int workitemid = item.resource.workItemId;
            string url = $"{_url}_apis/wit/workitems/{workitemid}?api-version=4.1";
            string response = await Get(url);
            response = response.Replace(".", "");
            WorkItem workitem = JsonConvert.DeserializeObject<WorkItem>(response);
            string[] teaminfo = workitem.fields.SystemTeamProject.Split('-');
            string venuename = teaminfo[1];
            string teamname = teaminfo.Count() == 3 ? teaminfo[2] : "DummyTeam";
            string challenge = workitem.fields.SystemTags.Split(';')[0].Trim();
            _logger.LogInformation($"Received call from VSTS. Splitted in Team [{teamname}], Venue [{venuename}]");

            string status = workitem.fields.SystemState;
            bool helpTagFound = workitem.fields.SystemTags.Split(';').Select(h => h.Trim().ToLowerInvariant()).Contains("help");
            return await UpdateChallenge(challenge, teamname, venuename, status, helpTagFound, workitemid);
        }

        protected async Task<string> Get(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers["Authorization"] = "Basic " + _token;
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex}";
            }
        }

        protected async Task<string> Patch(string url, string json)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PATCH";
                request.Headers["Authorization"] = "Basic " + _token;

                UTF8Encoding encoding = new UTF8Encoding();
                var byteArray = Encoding.ASCII.GetBytes(json);

                request.ContentType = "application/json-patch+json";

                using (Stream dataStream = await request.GetRequestStreamAsync())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Flush();
                    dataStream.Dispose();

                    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex}";
            }
        }

        private class WorkItem
        {
            public Fields fields { get; set; }

            public class Fields
            {
                public string SystemTeamProject { get; set; }
                public string SystemTags { get; set; }
                public string SystemState { get; set; }
            }
        }
        public class WorkItemUpdate
        {
            public Resource resource { get; set; }

            public class Resource
            {
                public int workItemId { get; set; }
            }
        }

        private async Task<IActionResult> UpdateChallenge(string challangename, string teamname, string venuename, string status, bool helpTagFound, int workitemid)
        {
            var challange = await _context.Challenge.SingleOrDefaultAsync(c => c.Name == challangename);
            if (challange == null)
            {
                return BadRequest("Unknown challange");
            }

            var team = await _context.Team.SingleOrDefaultAsync(c => c.Name == teamname && c.Venue.Name == venuename);
            if (team == null)
            {
                var venue = await _context.Venue.SingleOrDefaultAsync(c => c.Name == venuename);
                if (venue == null)
                {
                    venue = new Venue()
                    {
                        Name = venuename
                    };
                    await _context.Venue.AddAsync(venue);
                }
                team = new Team()
                {
                    Venue = venue,
                    Name = teamname
                };
            }

            var tsi = await _context.TeamScoreItem.SingleOrDefaultAsync(t => t.Challenge == challange && t.Team == team);


            if (tsi == null)
            {
                tsi = new TeamScoreItem()
                {
                    Challenge = challange,
                    Team = team,
                    Status = status,
                };
                await _context.TeamScoreItem.AddAsync(tsi);
            }
            else
            {
                tsi.Status = status;
            }
            if (!challange.IsBonus)
            if (!tsi.HelpUsed)
            {
                if (helpTagFound)
                {
                    await Patch($"{_url}_apis/wit/workitems/{workitemid}?api-version=4.1",
                         @"
[
  {
    ""op"": ""add"",
    ""path"": ""/fields/System.History"",
    ""value"": ""You requested help for this achievement. You can find it here [" + challange.HelpUrl + @"]""
  }
]
");

                    tsi.HelpUsed = true;
                }
            }
            await _context.SaveChangesAsync();
            return Accepted(tsi);
        }


    }
}
