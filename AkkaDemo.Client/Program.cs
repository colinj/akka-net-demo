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
            var jobId = 1;

            ColorConsole.WriteLineGray("Creating Client Actor System");
            var serverLocation = ConfigurationManager.AppSettings["serverLocation"];

            var actorSystem = ActorSystem.Create("ConsoleClient");
            ColorConsole.WriteLineGray("Creating actor supervisory hierarchy");
            var logger = actorSystem.ActorSelection($"akka.tcp://{ serverLocation }/user/LogCoordinator");
            var reporter = actorSystem.ActorSelection($"akka.tcp://{ serverLocation }/user/Report");
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
                    var report = new ReportMessage(jobId++, command.Split(',')[1]);

                    Task.Run(async () =>
                                   {
                                       var r = reporter.Ask(report);
                                       var ack = await r;
                                       ColorConsole.WriteLineCyan(ack.ToString());
                                       ColorConsole.WriteLineGray("");
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
