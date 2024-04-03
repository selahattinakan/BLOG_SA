using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Diagnostics;

namespace Redis.Extensions
{
    public static class RedisExtension
    {
        public static void AddStackExchangeRedis(this IServiceCollection services, IConfiguration configuration)
        {
            string _redisHost = configuration["Redis:Host"];
            string _redisPort = configuration["Redis:Port"];
            string _redisPassword = configuration["Redis:Password"];
            var configString = $"{_redisHost}:{_redisPort}";

#if DEBUG
            var redis = ConnectionMultiplexer.Connect(configString);
            services.AddSingleton(redis);

#else
            var options = ConfigurationOptions.Parse(configString); 
            options.Password = _redisPassword;
            var redis = ConnectionMultiplexer.Connect(options);
            services.AddSingleton(redis);
#endif

        }
    }
}
