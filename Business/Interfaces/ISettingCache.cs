using Constants;
using DB_EFCore.Entity;

namespace Business.Interfaces
{
    public interface ISettingCache
    {
        public Task<ResultSet> SaveSettingAsync(Setting setting);
        public Task<Setting?> GetSettingAsync();
    }
}
