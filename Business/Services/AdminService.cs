using Business.Helpers;
using Business.Interfaces;
using Constants;
using Constants.Enums;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext context;
        private readonly IService service;
        public AdminService(AppDbContext _context, IService _service)
        {
            context = _context;
            service = _service;
        }

        // Şifre kayıt ve gösterme işlemleri encrytion sınıfı ile yapılacak
        #region LogIn
        public bool LogInControl(string userName, string password)
        {
            Admin? admin = context.Admin.FirstOrDefault(x => x.UserName == userName && x.Password == password);
            if (admin != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> LogInControlAsync(string userName, string password)
        {
            Admin? admin = await context.Admin.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
            if (admin != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        public Admin? GetAdmin(int id)
        {
            return context.Admin.Find(id);
        }

        public async Task<Admin?> GetAdminAsync(int id)
        {
            return await context.Admin.FindAsync(id);
        }

        public List<Admin> GetAdmins()
        {
            return context.Admin.ToList();
        }

        public async Task<List<Admin>> GetAdminsAsync()
        {
            return await context.Admin.ToListAsync();
        }

        public ResultSet SaveAdmin(Admin admin)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;// context changetracker'dan da bakılabilir
                Admin? data = context.Admin.FirstOrDefault(x => x.Id == admin.Id);
                if (data == null)
                {
                    data = new Admin();
                    state = DbState.Insert;
                }
                data.UserName = admin.UserName;
                data.Password = admin.Password;
                data.FullName = admin.FullName;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = service.GetActiveUserId();
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.RegisterId = service.GetActiveUserId();
                    context.Add(data);
                }

                int count = context.SaveChanges();
                if (count > 0)
                {
                    result.Id = data.Id;
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
                DbState state = DbState.Update;
                Admin? data = await context.Admin.FirstOrDefaultAsync(x => x.Id == admin.Id);
                if (data == null)
                {
                    data = new Admin();
                    state = DbState.Insert;
                }
                data.UserName = admin.UserName;
                data.Password = admin.Password;
                data.FullName = admin.FullName;


                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = service.GetActiveUserId();
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.RegisterId = service.GetActiveUserId();
                    await context.AddAsync(data);
                }

                int count = await context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = data.Id;
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

        public ResultSet DeleteAdmin(int id)
        {
            ResultSet result = new ResultSet();
            Admin? admin = context.Admin.FirstOrDefault(x => x.Id == id);
            if (admin != null)
            {
                context.Remove(admin);
                int count = context.SaveChanges();
                if (count <= 0)
                {
                    result.Result = Result.Fail;
                    result.Message = "Silme işlemi başarısız";
                }
            }
            return result;
        }

        public async Task<ResultSet> DeleteAdminAsync(int id)
        {
            ResultSet result = new ResultSet();
            Admin? admin = await context.Admin.FirstOrDefaultAsync(x => x.Id == id);
            if (admin != null)
            {
                context.Remove(admin);
                int count = await context.SaveChangesAsync();
                if (count <= 0)
                {
                    result.Result = Result.Fail;
                    result.Message = "Silme işlemi başarısız";
                }
            }
            return result;
        }

        public void SP_FN_Test()
        {
            string userName = "SAKAN";
            var admin = context.Admin.FromSql($"GetAdminEncrypt {userName}").ToList();// entity doldurur


            var param = new Dictionary<string, string>();
            param.Add("@UserName", "SAKAN");
            var spTest = context.GetDataTableFromSP("GetAdminFullName", param);

        }
    }
}
