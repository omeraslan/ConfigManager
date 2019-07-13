using ConfigManager.Core.DTOs;

namespace ConfigManager.Core.Contracts
{
    public interface IConfigurationEngineFactory
    {
        IConfigurationEngine Create(string applicationName, ConnectionDTO connection, int refreshTimerIntervalInMs);
    }
}
