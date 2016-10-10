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
            var logger = _actorSystem.ActorOf(Props.Create<LogCoordinatorActor>(), "LogCoordinator");

            _actorSystem.WhenTerminated.Wait();
            ColorConsole.WriteLineGray("Actor system terminated");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}

