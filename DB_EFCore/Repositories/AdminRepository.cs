using Constants;
using Constants.Enums;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB_EFCore.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        //private readonly IService _service;
        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        // Şifre kayıt ve gösterme işlemleri encrytion sınıfı ile yapılacak
        #region LogIn
        public bool LogInControl(string userName, string password)
        {
            Admin? admin = _context.Admin.FirstOrDefault(x => x.UserName == userName && x.Password == password);
            if (admin != null)
            {
                return true;
            }
            return false;
        }

        public async Task<Admin> LogInControlAsync(string userName, string password)
        {
            return await _context.Admin.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
        }
        #endregion

        public Admin? GetAdmin(int id)
        {
            return _context.Admin.Find(id);
        }

        public async Task<Admin?> GetAdminAsync(int id)
        {
            return await _context.Admin.FindAsync(id);
        }

        public List<Admin> GetAdmins()
        {
            return _context.Admin.ToList();
        }

        public async Task<List<Admin>> GetAdminsAsync()
        {
            return await _context.Admin.ToListAsync();
        }

        public ResultSet SaveAdmin(Admin admin)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Add(admin);
                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = admin.Id;
                }
                else
                {
                    result.Result = Result.Fail;
                    result.Message = "Db işlemi başarısız";
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultSet UpdateAdmin(Admin admin)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Update(admin);
                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = admin.Id;
                }
                else
                {
                    result.Result = Result.Fail;
                    result.Message = "Db işlemi başarısız";
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultSet> SaveAdminAsync(Admin admin)
        {
            ResultSet result = new ResultSet();
            try
            {
                await _context.AddAsync(admin);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = admin.Id;
                }
                else
                {
                    result.Result = Result.Fail;
                    result.Message = "Db işlemi başarısız";
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultSet> UpdateAdminAsync(Admin admin)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Update(admin);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = admin.Id;
                }
                else
                {
                    result.Result = Result.Fail;
                    result.Message = "Db işlemi başarısız";
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultSet DeleteAdmin(Admin admin)
        {
            ResultSet result = new ResultSet();
            _context.Remove(admin);
            int count = _context.SaveChanges();
            if (count <= 0)
            {
                result.Result = Result.Fail;
                result.Message = "Silme işlemi başarısız";
            }
            return result;
        }

        public async Task<ResultSet> DeleteAdminAsync(Admin admin)
        {
            ResultSet result = new ResultSet();
            _context.Remove(admin);
            int count = await _context.SaveChangesAsync();
            if (count <= 0)
            {
                result.Result = Result.Fail;
                result.Message = "Silme işlemi başarısız";
            }
            return result;
        }

        public void SP_FN_Test()
        {
            string userName = "SAKAN";
            var admin = _context.Admin.FromSql($"GetAdminEncrypt {userName}").ToList();// entity doldurur


            var param = new Dictionary<string, string>();
            param.Add("@UserName", "SAKAN");
            var spTest = _context.GetDataTableFromSP("GetAdminFullName", param);

        }

        public bool IsExist(int id)
        {
            return _context.Admin.AsNoTracking().FirstOrDefault(x => x.Id == id) != null;
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _context.Admin.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) != null;
        }
    }
}
