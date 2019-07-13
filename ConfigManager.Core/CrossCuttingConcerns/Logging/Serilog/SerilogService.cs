using ConfigManager.Core.Contracts;
using ConfigManager.Core.Enums;

namespace ConfigManager.Core.CrossCuttingConcerns.Logging.Concrete.Serilog
{
    public class SerilogService : ILogger
    {
        private readonly global::Serilog.ILogger _logger;

        public SerilogService(global::Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void Log(LogEntry entry)
        {
            if (entry.Severity == LoggingEventType.Debug)
                _logger.Debug(entry.Exception, entry.Message);
            if (entry.Severity == LoggingEventType.Information)
                _logger.Information(entry.Exception, entry.Message);
            else if (entry.Severity == LoggingEventType.Warning)
                _logger.Warning(entry.Message, entry.Exception);
            else if (entry.Severity == LoggingEventType.Error)
                _logger.Error(entry.Message, entry.Exception);
            else
                _logger.Fatal(entry.Message, entry.Exception);
        }
    }
}