using System;
using System.Collections.Generic;
using System.Text;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.CrossCuttingConcerns.Caching.Concrete.MicrosoftMemory;
using ConfigManager.Core.CrossCuttingConcerns.Caching.Concrete.Redis;
using ConfigManager.Core.Enums;

namespace ConfigManager.Core.Factory
{
    public class CacheRepositoryFactory : ICacheRepositoryFactory
    {
        public ICacheRepository Create(CacheProviderType type)
        {
            switch (type)
            {
                case CacheProviderType.RedisCache:
                    return new RedisCacheRepository();
                case CacheProviderType.MemoryCache:
                    return new MicrosoftMemoryCacheRepository();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
