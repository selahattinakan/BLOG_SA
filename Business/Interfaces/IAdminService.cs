using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAdminService
    {
        public bool LogInControl(string userName, string password);
        public Task<Admin> LogInControlAsync(string userName, string password);
        public Admin? GetAdmin(int id);
        public Task<Admin?> GetAdminAsync(int id);
        public List<Admin> GetAdmins();
        public Task<List<Admin>> GetAdminsAsync();
        public ResultSet SaveAdmin(Admin admin);
        public Task<ResultSet> SaveAdminAsync(Admin admin);
        public ResultSet DeleteAdmin(int id);
        public Task<ResultSet> DeleteAdminAsync(int id);
        public void SP_FN_Test();
    }
}
