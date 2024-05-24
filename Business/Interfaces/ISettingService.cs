using Constants;
using DB_EFCore.Entity;

namespace Business.Interfaces
{
    public interface ISettingService : ISettingSave
    {
        public Setting? GetSetting();
        public Task<Setting?> GetSettingAsync();
        public ResultSet SaveSetting(Setting setting);
    }
}
