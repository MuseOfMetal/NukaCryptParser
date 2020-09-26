using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord.Webhook;
namespace NukaCryptParser
{
    class Program
    {
        static string Alpha;
        static string Bravo;
        static string Charlie;
        static string Week;
        static DiscordWebhookClient discord;
        static void Main(string[] args)
        {
            discord = new DiscordWebhookClient("");
            Task.Run(Sender);
            while (true)
            {
                Thread.Sleep(int.MaxValue);
            }
        }
        static void Sender()
        {
            while (true)
            {
                try
                {
                    string Responce = new WebClient().DownloadString("https://nukacrypt.com");
                    Regex codes = new Regex(@">\d{8}<");
                    Regex week = new Regex(@"Week of \d{2}\/\d{2} - \d{2}\/\d{2}");
                    var MatchesCodes = codes.Matches(Responce);
                    var MatchWeek = week.Match(Responce);
                    if (MatchWeek.Value != Week)
                    {
                        if (MatchesCodes.Count != 3)
                        {
                            Console.WriteLine($"Match codes: {MatchesCodes.Count}");
                            Thread.Sleep(3600000);
                            continue;
                        }
                        Week = MatchWeek.Value;
                        Alpha = MatchesCodes[0].Value;
                        Bravo = MatchesCodes[1].Value;
                        Charlie = MatchesCodes[2].Value;
                        discord.SendMessageAsync($"New codes!\n\n{Week}\n\nAlpha:\t  {Alpha}\nBravo:\t  {Bravo}\nCharlie:\t{Charlie}");
                    }
                    Thread.Sleep(3600000);
                }
                catch (Exception ex)
                {
                    discord.SendMessageAsync($"Error:\n\n{ex.Message}");
                }
            }
        }
    }
}
