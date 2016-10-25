using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    public class LogCoordinatorActor : ReceiveActor
    {
        private const string ActorTypeDesc = "LogCoordinatorActor";
        private const string LogPath = @"C:\AkkaLogDemo\";
        private readonly Dictionary<string, IActorRef> _loggers;

        public LogCoordinatorActor()
        {
            Directory.CreateDirectory(LogPath);
            _loggers = new Dictionary<string, IActorRef>();

            Receive<LogEntryMessage>(msg => HandleLogEntryMessage(msg));
        }

        private void HandleLogEntryMessage(LogEntryMessage msg)
        {
            var logEntryActor = GetLogEntryActor(msg.AppId);
            logEntryActor.Tell(msg);
        }

        private IActorRef GetLogEntryActor(string appId)
        {
            if (!_loggers.ContainsKey(appId))
            {
                var fileName = LogPath + appId + ".txt";
                var newLogEntryActor = Context.ActorOf(Props.Create(() => new LoggerActor(appId, fileName)), "Logger_" + appId);

                _loggers.Add(appId, newLogEntryActor);
            }

            return _loggers[appId];
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } PreStart on { Environment.MachineName }");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } PreRestart because: { reason }");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow($"{ ActorTypeDesc } PostRestart because: { reason }");
            base.PostRestart(reason);
        }
        #endregion
    }
}
