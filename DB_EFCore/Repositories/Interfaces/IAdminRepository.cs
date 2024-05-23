using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        public bool LogInControl(string userName, string password);
        public Task<Admin> LogInControlAsync(string userName, string password);
        public Admin? GetAdmin(int id);
        public Task<Admin?> GetAdminAsync(int id);
        public List<Admin> GetAdmins();
        public Task<List<Admin>> GetAdminsAsync();
        public ResultSet SaveAdmin(Admin admin);
        public Task<ResultSet> SaveAdminAsync(Admin admin);
        public ResultSet UpdateAdmin(Admin admin);
        public Task<ResultSet> UpdateAdminAsync(Admin admin);
        public ResultSet DeleteAdmin(Admin admin);
        public Task<ResultSet> DeleteAdminAsync(Admin admin);
        public bool IsExist(int id);
        public Task<bool> IsExistAsync(int id);
        public void SP_FN_Test();
    }
}
