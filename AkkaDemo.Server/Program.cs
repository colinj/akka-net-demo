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

            //            actorSystem.ActorOf(Props.Create<ApiActor>(logger, reporter), "api");
            actorSystem.ActorOf(Props.Create(() => new ApiActor(logger, reporter)), "api");

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

