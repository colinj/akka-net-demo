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
            var actorSystem = ActorSystem.Create("akkaDemo");

            ColorConsole.WriteLineGray("Creating actor supervisory hierarchy");

            var logger = actorSystem.ActorOf(Props.Create<LogCoordinatorActor>(), "logCoordinator");
            var reporter = actorSystem.ActorOf(Props.Create<ReportActor>().WithRouter(FromConfig.Instance), "report");
            var calc = actorSystem.ActorOf(Props.Create<CalcActor>().WithRouter(FromConfig.Instance), "calculator");

            actorSystem.ActorOf(Props.Create(() => new ApiActor(logger, reporter, calc)), "api");

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

