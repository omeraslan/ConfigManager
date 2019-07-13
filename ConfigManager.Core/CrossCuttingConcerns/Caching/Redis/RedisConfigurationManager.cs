using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace ConfigManager.Core.CrossCuttingConcerns.Caching.Concrete.Redis
{
    public class RedisConfigurationManager
    {
        private readonly StackExchangeRedisCacheClient _redisCacheClient;

        public RedisConfigurationManager()
        {
            var serializer = new NewtonsoftSerializer();
            _redisCacheClient = new StackExchangeRedisCacheClient(serializer, GetRedisConfiguration());
        }

        private static RedisConfiguration GetRedisConfiguration()
        {
            var redisConfiguration = new RedisConfiguration
            {
                AbortOnConnectFail = false,
                //KeyPrefix = "_my_key_prefix_",
                Hosts = new[]
                {
                    new RedisHost {Host = "127.0.0.1", Port = 6379}
                },
                AllowAdmin = true,
                ConnectTimeout = 60000,
                Database = 0,
                //Ssl = true,
                //Password = "my_super_secret_password",
                ServerEnumerationStrategy = new ServerEnumerationStrategy
                {
                    Mode = ServerEnumerationStrategy.ModeOptions.All,
                    TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                    UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                }
            };

            return redisConfiguration;
        }

        public StackExchangeRedisCacheClient GetConnection()
        {
            return _redisCacheClient;
        }
    }
}