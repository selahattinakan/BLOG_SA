using Business.Interfaces;
using DB_EFCore.Entity;
using Redis.Repositories;

namespace Business.Services
{
    public class RedisService : IRedisService
    {
        private readonly RedisRepository _redisRepository;

        public RedisService(RedisRepository redisRepository)
        {
            _redisRepository = redisRepository;
        }

        public async Task<bool> CreateSettingCache(Setting setting)
        {
            return await _redisRepository.CreateSettingCache(setting);
        }

        public async Task<Setting?> GetSettingFromCache(int id)
        {
            return await _redisRepository.GetSettingFromCache(id);
        }
    }
}
