using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISettingService
    {
        public Setting? GetSetting();
        public Task<Setting?> GetSettingAsync();
        public ResultSet SaveSetting(Setting setting);
        public Task<ResultSet> SaveSettingAsync(Setting setting);
    }
}
