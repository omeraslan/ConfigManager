using ConfigManager.Core.CrossCuttingConcerns.Logging.Concrete;

namespace ConfigManager.Core.Contracts
{
    public interface ILogger
    {
        void Log(LogEntry entry);
    }
}