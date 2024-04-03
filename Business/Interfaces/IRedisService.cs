using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRedisService
    {
        public Task<bool> CreateSettingCache(Setting setting);
        public Task<Setting?> GetSettingFromCache(int id);
    }
}
