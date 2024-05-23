using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISettingSave
    {
        public Task<ResultSet> SaveSettingAsync(Setting setting);
    }
}
