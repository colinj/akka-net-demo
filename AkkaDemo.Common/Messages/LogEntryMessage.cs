using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaDemo.Common.Messages
{
    public class LogEntryMessage
    {
        public string AppId { get; }
        public LogEventType LogEvent { get; }
        public string Message { get; }

        public LogEntryMessage(string appId, LogEventType logEvent, string message)
        {
            AppId = appId.Trim();
            LogEvent = logEvent;
            Message = message;
        }

        public override string ToString()
        {
            return $"{LogEventDescription.ToString(LogEvent)}: {Message}";
        }
    }

    public enum LogEventType
    {
        Error = 0,
        Warn = 1,
        Info = 2,
        Debug = 3,
        Trace = 4
    }

    static class LogEventDescription
    {
        private static readonly string[] LogTypeDescription = { "ERROR", "WARN", "INFO", "DEBUG", "TRACE" };

        public static string ToString(LogEventType logEvent)
        {
            return LogTypeDescription[(int)logEvent];
        }
    }
}
