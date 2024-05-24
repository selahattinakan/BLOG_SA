using Business.Interfaces;
using Constants;
using DB_EFCore.Entity;

namespace Business.Decorators
{
    public class BaseSettingDecorator : ISettingSave
    {
        private readonly ISettingService _settingService;

        public BaseSettingDecorator(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public virtual async Task<ResultSet> SaveSettingAsync(Setting setting)
        {
            return await _settingService.SaveSettingAsync(setting);
        }
    }
}
