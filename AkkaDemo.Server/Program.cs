using System;
using Akka.Actor;
using AkkaDemo.Common;
using AkkaDemo.Common.Actors;

namespace AkkaDemo.Server
{
    class Program
    {
        private static ActorSystem _actorSystem;

        private static void Main(string[] args)
        {
            ColorConsole.WriteLineGray("Creating Demo Server ActorSystem");
            _actorSystem = ActorSystem.Create("LogServer");

            ColorConsole.WriteLineGray("Creating actor supervisory hierarchy");
            _actorSystem.ActorOf(Props.Create<LogCoordinatorActor>(), "LogCoordinator");
            _actorSystem.ActorOf(Props.Create<ReportSchedulerActor>(), "ReportScheduler");

            Console.ReadKey();
            _actorSystem.Terminate();
            ColorConsole.WriteLineGray("Terminate called");
            _actorSystem.WhenTerminated.Wait();
            ColorConsole.WriteLineGray("Actor system terminated");
            Console.ReadKey();

            Environment.Exit(1);
        }
    }
}

