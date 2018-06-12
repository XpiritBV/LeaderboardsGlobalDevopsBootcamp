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
using gdbcLeaderBoard.Helpers;
using gdbcLeaderBoard.Domain.VSTSModels;

namespace gdbcLeaderBoard.Controllers
{
    [Authorize(Roles = "Xpirit")]
    [Route("api/challenge")]
    public class ChallengesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private string _token;
        private ILogger _logger;

        public ChallengesController(ApplicationDbContext context, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _context = context;

            _token = configuration.GetConnectionString("Token");
            //_url = configuration.GetConnectionString("VSTSUrl");
            _logger = loggerFactory.CreateLogger<ChallengesController>();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]WorkItemUpdate item)
        {
            var vstsUrl = item.resourceContainers.collection.baseUrl;

            int workitemid = item.resource.workItemId;
            WorkItem workitem = await GetWorkItemInformation(vstsUrl, workitemid);

            string[] teaminfo = workitem.fields.SystemTeamProject.Split('-');
            string venuename = teaminfo[1];
            string teamname = teaminfo.Count() >= 3 ? teaminfo[2] : "DummyTeam";
            string uniqueTag = TagHelper.GetUniqueTag(workitem.fields.SystemTags);
            _logger.LogInformation($"Received call from VSTS. Splitted in Team [{teamname}], Venue [{venuename}]");

            string status = workitem.fields.SystemState;
            bool helpTagFound = workitem.fields.SystemTags.Split(';').Select(h => h.Trim().ToLowerInvariant()).Contains("help");
            return await UpdateChallenge(vstsUrl, uniqueTag, teamname, venuename, status, helpTagFound, workitemid);
        }

        private async Task<WorkItem> GetWorkItemInformation(string vstsUrl, int workitemid)
        {
            string url = $"{vstsUrl}_apis/wit/workitems/{workitemid}?api-version=4.1";
            string response = await Get(url);
            response = response.Replace(".", "");
            try
            {
                WorkItem workitem = JsonConvert.DeserializeObject<WorkItem>(response);
                return workitem;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error converting workitem information: {e.Message}");
                throw;
            }
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
        
        private async Task<IActionResult> UpdateChallenge(string vstsUrl, string uniqueTag, string teamname, string venuename, string status, bool helpTagFound, int workitemid)
        {
            var challenge = await _context.Challenge.SingleOrDefaultAsync(c => c.UniqueTag == uniqueTag);
            if (challenge == null)
            {
                return BadRequest("Unknown challenge");
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

            var tsi = await _context.TeamScoreItem.SingleOrDefaultAsync(t => t.Challenge == challenge && t.Team == team);


            if (tsi == null)
            {
                tsi = new TeamScoreItem()
                {
                    Challenge = challenge,
                    Team = team,
                    Status = status,
                };
                await _context.TeamScoreItem.AddAsync(tsi);
            }
            else
            {
                tsi.Status = status;
            }
            if (!challenge.IsBonus)
                if (!tsi.HelpUsed)
                {
                    if (helpTagFound)
                    {
                        await Patch($"{vstsUrl}_apis/wit/workitems/{workitemid}?api-version=4.1",
                             @"
[
  {
    ""op"": ""add"",
    ""path"": ""/fields/System.History"",
    ""value"": ""You requested help for this achievement. You can find it here [" + challenge.HelpUrl + @"]""
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
