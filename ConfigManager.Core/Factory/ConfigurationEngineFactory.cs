using ConfigManager.Core.Business;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;

namespace ConfigManager.Core.Factory
{
    public class ConfigurationEngineFactory : IConfigurationEngineFactory
    {
        private readonly IConfigurationDataRepositoryFactory _storageProviderFactory;
        private readonly ICacheRepositoryFactory _cacheManagerFactory;
        public ConfigurationEngineFactory()
        {
            _storageProviderFactory = new StorageRepositoryFactory();
            _cacheManagerFactory = new CacheRepositoryFactory();

        }
        public IConfigurationEngine Create(string applicationName, ConnectionDTO connection, int refreshTimerIntervalInMs)
        {
            var storageProvider = _storageProviderFactory.Create(connection);
            var cacheManager = _cacheManagerFactory.Create(Enums.CacheProviderType.MemoryCache);

            return new ConfigurationEngine(storageProvider, cacheManager, applicationName, refreshTimerIntervalInMs);
        }
    }
}
