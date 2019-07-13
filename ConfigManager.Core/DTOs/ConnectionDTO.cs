using ConfigManager.Core.Enums;

namespace ConfigManager.Core.DTOs
{
    public class ConnectionDTO
    {
        public string ConnectionString { get; set; }
        public StorageProviderType StorageProviderType { get; set; }

        public ConnectionDTO(string connectionString, StorageProviderType storageProviderType)
        {
            ConnectionString = connectionString;
            StorageProviderType = storageProviderType;
        }

    }
}
