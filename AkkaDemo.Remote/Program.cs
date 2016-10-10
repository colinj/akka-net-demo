using System;
using Akka.Actor;
using AkkaDemo.Common;

namespace AkkaDemo.Remote
{
    class Program
    {
        private static ActorSystem _actorSystem;

        private static void Main(string[] args)
        {
            ColorConsole.WriteLineGray("Creating Demo ActorSystem in remote process");
            _actorSystem = ActorSystem.Create("RemoteDemo");

            _actorSystem.WhenTerminated.Wait();
            ColorConsole.WriteLineGray("Actor system terminated");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
