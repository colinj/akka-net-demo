﻿using System;
using System.Threading;
using Akka.Actor;
using AkkaDemo.Common;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using System.Configuration;

namespace AkkaDemo.Remote
{
    class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                QuitEvent.Set();
                e.Cancel = true;
            };

            ColorConsole.WriteLineGray("Creating Remote Actor System");

            var config = ConfigurationFactory.ParseString($"akka.remote.helios.tcp.hostname = {IPUtils.LocalIPAddress()}")
                                             .WithFallback(GetAkkaConfig("akka"));

            var actorSystem = ActorSystem.Create("akkaDemo", config);

            QuitEvent.WaitOne();

            ColorConsole.WriteLineGray("Shutting down");
            var cluster = Akka.Cluster.Cluster.Get(actorSystem);
            cluster.RegisterOnMemberRemoved(() => MemberRemoved(actorSystem));

            ColorConsole.WriteLineGray("Leaving Cluster...");
            cluster.Leave(cluster.SelfAddress);

            actorSystem.WhenTerminated.Wait();
            ColorConsole.WriteLineGray("Actor system terminated");

            Console.ReadKey();

            Environment.Exit(1);
        }

        private static async void MemberRemoved(ActorSystem actorSystem)
        {
            Thread.Sleep(2000);
            await actorSystem.Terminate();
            ColorConsole.WriteLineGray("Left Cluster.");
        }

        private static Config GetAkkaConfig(string sectionName)
        {
            return ((AkkaConfigurationSection) ConfigurationManager.GetSection(sectionName)).AkkaConfig;
        }

    }
}
