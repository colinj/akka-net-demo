using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaDemo.Common;
using AkkaDemo.Common.Actors;
using AkkaDemo.Common.Messages;

namespace AkkaDemo
{
    internal class Program
    {
        private static ActorSystem _actorSystem;

        private static void Main(string[] args)
        {
            ColorConsole.WriteLineGray("Creating Demo ActorSystem");
            _actorSystem = ActorSystem.Create("LogDemo");

            ColorConsole.WriteLineGray("Creating actor supervisory hierarchy");
            var logger = _actorSystem.ActorOf(Props.Create<LogCoordinatorActor>(), "LogCoordinator");

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

            } while (command != "exit");

            _actorSystem.Terminate();
            _actorSystem.WhenTerminated.Wait();
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

