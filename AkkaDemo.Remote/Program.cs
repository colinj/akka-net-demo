using System;
using Akka.Actor;
using AkkaDemo.Common;

namespace AkkaDemo.Remote
{
    class Program
    {
        private static void Main(string[] args)
        {
            ColorConsole.WriteLineGray("Creating Remote Actor System");
            var actorSystem = ActorSystem.Create("LogRemote");

            actorSystem.WhenTerminated.Wait();
            ColorConsole.WriteLineGray("Actor system terminated");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
