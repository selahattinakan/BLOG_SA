using Constants;
using DB_EFCore.Entity;

namespace Business.Interfaces
{
    public interface ISettingService : ISettingCache
    {
        public Setting? GetSetting();
        public ResultSet SaveSetting(Setting setting);
    }
}
