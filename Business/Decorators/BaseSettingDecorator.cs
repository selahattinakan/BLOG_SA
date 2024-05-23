using Business.Interfaces;
using Business.Services;
using Constants;
using Constants.Enums;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Decorators
{
    public class BaseSettingDecorator : ISettingSave
    {
        private readonly ISettingService _settingService;
        private readonly IService _service;

        public BaseSettingDecorator(ISettingService settingService, IService service)
        {
            _settingService = settingService;
            _service = service;
        }

        public virtual async Task<ResultSet> SaveSettingAsync(Setting setting)
        {
            return await _settingService.SaveSettingAsync(setting);
        }
    }
}
