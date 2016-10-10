using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.Common.Actors
{
    public class LoggerActor : ReceiveActor
    {
        private readonly string _logFileName;

        public LoggerActor(string logFileName)
        {
            _logFileName = logFileName;
            Receive<LogEntryMessage>(msg => HandleLogEntryMessage(msg));
        }

        private void HandleLogEntryMessage(LogEntryMessage msg)
        {
            File.AppendAllText(_logFileName, $"{ msg }\r\n");
            ColorConsole.WriteLineYellow($"messaged logged for { msg.AppId }.");
        }

        protected override void PreStart()
        {
            ColorConsole.WriteLineGreen($"LoggerActor: Log file = { _logFileName }.");
        }
    }
}
