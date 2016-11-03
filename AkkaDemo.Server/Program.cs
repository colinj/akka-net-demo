using System;
using Akka.Actor;
using AkkaDemo.Common;
using AkkaDemo.Common.Actors;

namespace AkkaDemo.Server
{
    class Program
    {
        private static void Main(string[] args)
        {
            ColorConsole.WriteLineGray("Creating Demo Server Actor System");
            var actorSystem = ActorSystem.Create("DemoServer");

            ColorConsole.WriteLineGray("Creating actor supervisory hierarchy");
            actorSystem.ActorOf(Props.Create<LogCoordinatorActor>(), "LogCoordinator");
            actorSystem.ActorOf(Props.Create<ReportSchedulerActor>(), "ReportScheduler");

            Console.ReadKey();
            actorSystem.Terminate();
            ColorConsole.WriteLineGray("Terminate called");
            actorSystem.WhenTerminated.Wait();
            ColorConsole.WriteLineGray("Actor system terminated");
            Console.ReadKey();

            Environment.Exit(1);
        }
    }
}

