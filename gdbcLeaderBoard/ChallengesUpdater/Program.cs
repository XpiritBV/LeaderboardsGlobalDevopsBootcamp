using gdbcLeaderBoard.Data;
using mdtemplate.Stories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ChallengesUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please pass two filenames to read from as an argument to this exe.");
            }
            else
            {
                ReadChallengesFile(args[0], args[1]);
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }

        private static void ReadChallengesFile(string dropboxLinkFileName, string storiesFileName)
        {
            if (!File.Exists(dropboxLinkFileName))
            {
                Console.WriteLine($"Cannot find dropbox links file '{dropboxLinkFileName}'");
                return;
            }

            if (!File.Exists(storiesFileName))
            {
                Console.WriteLine($"Cannot find stories file '{storiesFileName}'");
                return;
            }

            Console.WriteLine($"Reading from file '{dropboxLinkFileName}'");
            var contents = File.ReadAllLines(dropboxLinkFileName);

            if (contents.Length <= 3)
            {
                Console.WriteLine("File contents to short, stopping execution");
                return;
            }

            var stories = GetStories(storiesFileName);
            Console.WriteLine($"Found {stories.Count} stories in '{storiesFileName}'");
            LoadContext();

            var challenges = _context.Challenge.ToListAsync().GetAwaiter().GetResult();
            Console.WriteLine($"Found {challenges.Count} existing challenges. Will update the information if neccesary");
            Console.WriteLine();

            var processedChallenges = new List<string>();
            for (int i = 2; i <= contents.Length - 1; i++)
            {
                // line should have 2 elements
                var lineParts = contents[i].Split(",");
                if (lineParts.Length != 2)
                {
                    Console.WriteLine($"Error parsing line {i + 1}");
                }

                // remove the quotes
                var sourceDirectory = lineParts[0].Replace("\"", "");
                var dropBoxLink = lineParts[1].Replace("\"", "");

                UpdateChallenge(sourceDirectory, dropBoxLink, challenges, stories);

                processedChallenges.Add(ExtractChallengeName(sourceDirectory));
            }

            AddMissingChallenges(stories, processedChallenges, _context);

            _context.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Done with the updates");
        }

        private static void AddMissingChallenges(StoryCollection stories, List<string> processedChallenges, ApplicationDbContext context)
        {
            foreach (var story in stories)
            {
                var challengeName = ExtractChallengeName(story.Filename);

                if (processedChallenges.IndexOf(challengeName) == -1 && _context.Challenge.FirstOrDefault(item => item.Name == challengeName) == null && story.StoryType != StoryType.Feature)
                {
                    // add this missing challenge to the db

                    var data = GetPointsForStory(stories, challengeName);

                    _context.Challenge.Add(new gdbcLeaderBoard.Models.Challenge
                    {
                        Name = challengeName,
                        Points = data.Item1,
                        IsBonus = data.Item2,
                        UniqueTag = data.Item3
                    });                
                }
            }
        }

        private static StoryCollection GetStories(string storiesFileName)
        {
            var contents = File.ReadAllText(storiesFileName);
            return JsonConvert.DeserializeObject<StoryCollection>(contents);
        }

        private static ApplicationDbContext _context;

        private static string ExtractChallengeName(string sourceDirectory)
        {
            return sourceDirectory.Substring(0, 9);
        }

        private static void UpdateChallenge(string sourceDirectory, string dropBoxLink, List<gdbcLeaderBoard.Models.Challenge> challenges, StoryCollection stories)
        {
            var challengeName = ExtractChallengeName(sourceDirectory);

            var challenge = challenges.FirstOrDefault(item => item.Name == challengeName);
            if (challenge == null)
            {
                challenge = new gdbcLeaderBoard.Models.Challenge { Name = challengeName };
                _context.Challenge.Add(challenge);
            }

            // update properties
            challenge.HelpUrl = UpdateLinkToForceDownload(dropBoxLink);

            // get info from parsed stories:
            var data = GetPointsForStory(stories, challengeName);
            challenge.Points = data.Item1;
            challenge.IsBonus = data.Item2;
            challenge.UniqueTag = data.Item3;

            Console.Write($"Updating challenge '{sourceDirectory}', points: {challenge.Points}, bonus: {challenge.IsBonus}");

            Console.WriteLine();
        }

        private static Tuple<int, bool, string> GetPointsForStory(StoryCollection stories, string challengeName)
        {
            var story = stories.FirstOrDefault(item => item.Id == challengeName);
            var points = 0;
            var isBonus = false;
            var uniqueTag = "";

            if (story == null)
            {
                Console.WriteLine($"Cannot process story for challenge '{challengeName}'");
                return new Tuple<int, bool, string>(points, isBonus, uniqueTag);
            }

            var pointsProperty = story.Properties.FirstOrDefault(item => item.Key == "effort");
            if (!string.IsNullOrEmpty(pointsProperty.Key))
            {
                int.TryParse(pointsProperty.Value, out points);
            }

            var isBonusProperty = story.Properties.FirstOrDefault(item => item.Key == "bonus");
            if (!string.IsNullOrEmpty(pointsProperty.Key))
            {
                bool.TryParse(isBonusProperty.Value, out isBonus);
            }

            var uniqueTagProperty = story.Properties.FirstOrDefault(item => item.Key == "code");
            if (!string.IsNullOrEmpty(uniqueTagProperty.Key))
            {
                uniqueTag = uniqueTagProperty.Value;
            }

            return new Tuple<int, bool, string>(points, isBonus, uniqueTag);
        }

        private static string UpdateLinkToForceDownload(string dropBoxLink)
        {
            //example: https://www.dropbox.com/s/randomstringhere/F001-P002-ManuallyCreateAzureResources-help.zip?dl=0
            return dropBoxLink.Replace("?dl=0", "?dl=1");
        }
        
        private static void LoadContext()
        {
            if (_context == null)
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>();
                options.UseSqlServer(GetConnectionString("DefaultConnection"));

                _context = new ApplicationDbContext(options.Options);

                try
                {
                    // test the connection
                    _context.Database.OpenConnection();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error connection to database: {e.Message}");
                }
            }
        }

        private static string GetConnectionString(string connectionStringName)
        {
            var currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"Checking directory '{currentDirectory}' for appsettings");

            var builder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true);

            IConfiguration configuration = builder.Build();

            return configuration.GetConnectionString(connectionStringName);
        }
    }
}
