using System;
using System.Threading;
using Akka.Actor;
using AkkaDemo.Common;
using AkkaDemo.Common.Actors;
using AkkaDemo.Common.Messages;
using Akka.Configuration;

namespace AkkaDemo.Server
{
    class Program
    {
        private static ActorSystem _actorSystem;

        private static void Main(string[] args)
        {
            ColorConsole.WriteLineGray("Creating Demo Server ActorSystem");

            var config = ConfigurationFactory.ParseString(@"
akka {
 loglevel = OFF
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
            debug {
                receive = on
              autoreceive = on
              lifecycle = on
              event-stream = on
              unhandled = on
        }
    }

    remote {
        helios.tcp {
              transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
              transport-protocol = tcp
            port = 8080
            hostname = localhost
        }
    }
}
");

            _actorSystem = ActorSystem.Create("LogServer");

            ColorConsole.WriteLineGray("Creating actor supervisory hierarchy");
            var logger = _actorSystem.ActorOf(Props.Create<LogCoordinatorActor>(), "LogCoordinator");
            /*

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
                                                      */
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

