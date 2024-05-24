using Business.Interfaces;
using Business.Services;
using Constants;
using DB_EFCore.Entity;

namespace Business.Decorators
{
    public class SettingRedisDecorator : BaseSettingDecorator
    {
        private readonly IRedisService _redisService;

        public SettingRedisDecorator(ISettingService settingService, IRedisService redisService) : base(settingService)
        {
            _redisService = redisService;
        }

        public override async Task<ResultSet> SaveSettingAsync(Setting setting)
        {
            ResultSet result = await base.SaveSettingAsync(setting);

            if (result.Result == Constants.Enums.Result.Success)
            {
                if (setting.IsRedisEnable)
                {
                    await _redisService.CreateSettingCache((Setting)result.Object);
                }
            }

            return result;
        }
    }
}
