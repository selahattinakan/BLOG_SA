using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Repositories.Interfaces
{
    public interface ISettingRepository
    {
        public Setting? GetSetting();
        public Setting? GetSetting(int id);
        public Task<Setting?> GetSettingAsync();
        public Task<Setting?> GetSettingAsync(int id);
        public ResultSet SaveSetting(Setting setting);
        public Task<ResultSet> SaveSettingAsync(Setting setting);
        public ResultSet UpdateSetting(Setting setting);
        public Task<ResultSet> UpdateSettingAsync(Setting setting);
    }
}
