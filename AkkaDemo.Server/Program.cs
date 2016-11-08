using System;
using Akka.Actor;
using Akka.Routing;
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
            var logger = actorSystem.ActorOf(Props.Create<LogCoordinatorActor>(), "LogCoordinator");
            var reporter = actorSystem.ActorOf(Props.Create<ReportActor>().WithRouter(FromConfig.Instance), "Report");

            actorSystem.ActorOf(Props.Create<ApiActor>(logger, reporter), "api");

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

