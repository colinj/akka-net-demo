using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    public class LoggerActor : BaseActor
    {
        private readonly string _appId;
        private readonly string _logFileName;

        public LoggerActor(string appId, string logFileName)
        {
            _appId = appId;
            _logFileName = logFileName;
            Receive<LogEntryMessage>(msg => HandleLogEntryMessage(msg));
        }

        private void HandleLogEntryMessage(LogEntryMessage msg)
        {
            File.AppendAllText(_logFileName, $"{ msg }\r\n");
            ColorConsole.WriteLineYellow($"Message logged for LoggerActor '{ msg.AppId }'.");
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName }: Log file = { _logFileName }.");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _appId } PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _appId } PreRestart because: { reason }");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow($"{ ActorClassName } { _appId } PostRestart because: { reason }");
            base.PostRestart(reason);
        }
        #endregion
    }
}
