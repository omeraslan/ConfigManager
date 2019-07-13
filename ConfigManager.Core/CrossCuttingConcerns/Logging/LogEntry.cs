using System;
using ConfigManager.Core.Enums;

namespace ConfigManager.Core.CrossCuttingConcerns.Logging.Concrete
{
    public class LogEntry
    {
        public readonly Exception Exception;
        public readonly string Message;
        public readonly LoggingEventType Severity;

        public LogEntry(LoggingEventType severity, string message, Exception exception = null)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (message == string.Empty) throw new ArgumentException("empty", "message");

            Severity = severity;
            Message = message;
            Exception = exception;
        }
    }
}