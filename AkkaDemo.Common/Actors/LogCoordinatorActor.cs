using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    public class LogCoordinatorActor : ReceiveActor
    {
        private const string LogPath = @"C:\AkkaLogDemo\";
        private readonly Dictionary<string, IActorRef> _loggers;

        public LogCoordinatorActor()
        {
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
                var newLogEntryActor = Context.ActorOf(Props.Create(() => new LoggerActor(fileName)), "Logger_" + appId);

                _loggers.Add(appId, newLogEntryActor);
            }

            return _loggers[appId];
        }
    }
}
