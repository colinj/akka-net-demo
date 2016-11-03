using System;
using System.Configuration;
using Akka.Actor;
using AkkaDemo.Common;
using AkkaDemo.Common.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace AkkaDemo.Client
{
    class Program
    {
        private static void Main(string[] args)
        {
            ColorConsole.WriteLineGray("Creating Client Actor System");
            var loggerAddress = ConfigurationManager.AppSettings["loggerAddress"];

            var actorSystem = ActorSystem.Create("LogClient");
            ColorConsole.WriteLineGray("Creating actor supervisory hierarchy");
            var logger = actorSystem.ActorSelection($"akka.tcp://DemoServer@{ loggerAddress }/user/LogCoordinator");
            var scheduler = actorSystem.ActorSelection($"akka.tcp://DemoServer@{ loggerAddress }/user/ReportScheduler");
            string command;

            do
            {
                ShortPause();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                ColorConsole.WriteLineGray("enter a command and hit enter");

                command = Console.ReadLine() ?? string.Empty;

                if (command.StartsWith("log"))
                {
                    var appId = command.Split(',')[1];
                    var logMsg = command.Split(',')[2];

                    var message = new LogEntryMessage(appId, LogEventType.Info, logMsg);
                    logger.Tell(message);
                }

                if (command.StartsWith("rpt"))
                {
                    ColorConsole.WriteLineGreen("ready to send report");
                    int jobId;

                    int.TryParse(command.Split(',')[1], out jobId);
                    var rptTitle= command.Split(',')[2];

                    var report = new ReportMessage
                                 {
                                     JobId = jobId,
                                     ReportTitle = rptTitle
                                 };
                    Task.Run(async () =>
                                   {
                                       var r = scheduler.Ask(report);

                                       await Task.WhenAll(r);
                                       ColorConsole.WriteLineCyan(r.Result.ToString());
                                   });
                       
                }


            } while (command != "exit");

            actorSystem.Terminate();
            actorSystem.WhenTerminated.Wait();
            ColorConsole.WriteLineGray("Actor system terminated");
            Console.ReadKey();
            Environment.Exit(1);
        }

        // Perform a short pause for demo purposes to allow console to update nicely
        private static void ShortPause()
        {
            Thread.Sleep(450);
        }
    }
}
