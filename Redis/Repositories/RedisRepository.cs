using DB_EFCore.Entity;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;


namespace Redis.Repositories
{
    public class RedisRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase db;
        private const int dbIndex = 1;
        private const string settingKey = "settingKey";
        public RedisRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            db = _redis.GetDatabase(dbIndex);
        }

        public async Task<bool> CreateSettingCache(Setting setting)
        {
            try
            {
                return await db.HashSetAsync(settingKey, setting.Id, JsonSerializer.Serialize(setting));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Setting?> GetSettingFromCache(int id)
        {
            try
            {
                if (!db.KeyExists(settingKey)) return null;

                var settingJson = await db.HashGetAsync(settingKey, id);
                return settingJson.HasValue ? JsonSerializer.Deserialize<Setting>(settingJson.ToString()) : null;
            }
            catch (Exception) 
            {
                return null;
            }
        }
    }
}
