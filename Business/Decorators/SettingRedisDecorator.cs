using Business.Interfaces;
using Constants;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business.Decorators
{
    public class SettingRedisDecorator : BaseSettingDecorator
    {
        private readonly IRedisService _redisService;

        public SettingRedisDecorator(ISettingService settingService, IService service, IRedisService redisService) : base(settingService, service)
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
