using Constants;
using DB_EFCore.Entity;

namespace Business.Interfaces
{
    public interface ISettingSave
    {
        public Task<ResultSet> SaveSettingAsync(Setting setting);
    }
}
