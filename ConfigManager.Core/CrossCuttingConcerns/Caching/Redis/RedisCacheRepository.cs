using System;
using ConfigManager.Core.Contracts;
using StackExchange.Redis.Extensions.Core;

namespace ConfigManager.Core.CrossCuttingConcerns.Caching.Concrete.Redis
{
    public class RedisCacheRepository : ICacheRepository
    {
        private readonly ICacheClient _database;

        public RedisCacheRepository()
        {
            var redisConfig = new RedisConfigurationManager();
            _database = redisConfig.GetConnection();
        }

        public T Get<T>(string key)
        {
            return _database.Get<T>(key);
        }

        public void Add(string key, object data, int cacheTime)
        {
            if (data == null) return;

            _database.Add(key, data, DateTimeOffset.Now.AddMinutes(cacheTime));
        }

        public bool IsAdd(string key)
        {
            return _database.Exists(key);
        }

        public void Remove(string key)
        {
            _database.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var keys = _database.SearchKeys(pattern);
            _database.RemoveAll(keys);
        }

        public void Clear()
        {
            _database.FlushDb();
        }
    }
}