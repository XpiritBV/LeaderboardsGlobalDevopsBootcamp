using gdbcLeaderBoard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
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
            if (args.Length == 0)
            {
                Console.WriteLine("Please pass a file to read from as an argument to this exe.");
            }
            else
            {
                ReadChallengesFile(args[0]);
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }

        private static void ReadChallengesFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Cannot find file '{fileName}'");
                return;
            }

            Console.WriteLine($"Reading from file '{fileName}'");
            var contents = File.ReadAllLines(fileName);

            if (contents.Length <= 3)
            {
                Console.WriteLine("File contents to short, stopping execution");
                return;
            }

            LoadContext();

            var challenges = _context.Challenge.ToListAsync().GetAwaiter().GetResult();
            Console.WriteLine($"Found {challenges.Count} existing challenges. Will update the information if neccesary");
            Console.WriteLine();

            for (int i = 2; i < contents.Length - 1; i++)
            {
                var lineParts = contents[i].Split(",");
                if (lineParts.Length != 2)
                {
                    Console.WriteLine($"Error parsing line {i + 1}");
                }

                var sourceDirectory = lineParts[0].Replace("\"", "");
                var dropBoxLink = lineParts[1].Replace("\"", "");

                UpdateChallenge(sourceDirectory, dropBoxLink, challenges);                
            }
            _context.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Done with the updates");
        }

        private static ApplicationDbContext _context;

        private static void UpdateChallenge(string sourceDirectory, string dropBoxLink, List<gdbcLeaderBoard.Models.Challenge> challenges)
        {
            Console.Write($"Updating challenge '{sourceDirectory}'");

            var challangeName = sourceDirectory.Substring(0, 9);

            var challange = challenges.FirstOrDefault(item => item.Name == challangeName);
            if (challange == null)
            {
                challange = new gdbcLeaderBoard.Models.Challenge { Name = challangeName };
                _context.Challenge.Add(challange);
            }

            challange.HelpUrl = UpdateLinkToForceDownload(dropBoxLink);
            if (string.IsNullOrWhiteSpace(challange.UniqueTag))
            {
                challange.UniqueTag = GetNewUniqueTag();
            }
            
            Console.WriteLine();
        }

        private static string UpdateLinkToForceDownload(string dropBoxLink)
        {
            //example: https://www.dropbox.com/s/randomstringhere/F001-P002-ManuallyCreateAzureResources-help.zip?dl=0
            return dropBoxLink.Replace("?dl=0", "?dl=1");
        }

        private static Random random = new Random();
        private static string GetNewUniqueTag()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true);

            IConfiguration configuration = builder.Build();

            return configuration.GetConnectionString(connectionStringName);
        }
    }
}
