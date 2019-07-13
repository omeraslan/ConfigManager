using ConfigManager.Core.Contracts;
using ConfigManager.Core.DataAccess.InMemoryStorageProvider;
using ConfigManager.Core.DataAccess.MongoStorageProvider;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Enums;
using System;

namespace ConfigManager.Core.Factory
{
    public class StorageRepositoryFactory : IConfigurationDataRepositoryFactory
    {
        public IConfigurationDataRepository Create(ConnectionDTO connectionDTO)
        {
            switch (connectionDTO.StorageProviderType)
            {
                case StorageProviderType.MongoDb:
                    return new MongoDbDataRepository(connectionDTO.ConnectionString);
                case StorageProviderType.InMemoryDb:
                    return new InMemoryDataRepository();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
