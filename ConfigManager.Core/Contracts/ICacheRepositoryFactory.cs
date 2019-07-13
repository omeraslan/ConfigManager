using ConfigManager.Core.Enums;

namespace ConfigManager.Core.Contracts
{
    public interface ICacheRepositoryFactory
    {
        ICacheRepository Create(CacheProviderType type);
    }
}
