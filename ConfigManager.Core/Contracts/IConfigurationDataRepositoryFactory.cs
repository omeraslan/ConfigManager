using ConfigManager.Core.DTOs;

namespace ConfigManager.Core.Contracts
{
    public interface IConfigurationDataRepositoryFactory
    {
        IConfigurationDataRepository Create(ConnectionDTO connectionDTO);
    }
}
