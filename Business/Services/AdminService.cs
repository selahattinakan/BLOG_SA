using Business.Helpers;
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
    public class AdminService
    {
        #region LogIn
        public bool LogInControl(string userName, string password)
        {
            using (var context = new AppDbContext())
            {
                Admin? admin = context.Admin.FirstOrDefault(x => x.UserName == userName && x.Password == Encryption.Encrypt(password));
                if (admin != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> LogInControlAsync(string userName, string password)
        {
            using (var context = new AppDbContext())
            {
                Admin? admin = await context.Admin.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == Encryption.Encrypt(password));
                if (admin != null)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        public Admin? GetAdmin(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Admin.FirstOrDefault(x => x.Id == id);
            }
        }

        public async Task<Admin?> GetAdminAsync(int id)
        {
            using (var context = new AppDbContext())
            {
                return await context.Admin.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public List<Admin> GetAdmins()
        {
            using (var context = new AppDbContext())
            {
                return context.Admin.ToList();
            }
        }

        public async Task<List<Admin>> GetAdminsAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Admin.ToListAsync();
            }
        }

        public ResultSet SaveAdmin(Admin admin)
        {
            ResultSet result = new ResultSet();
            try
            {
                using (var context = new AppDbContext())
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
                        data.UpdateAdminId = 2; //aktif kullanıcı id
                    }
                    else
                    {
                        data.RegisterDate = DateTime.Now;
                        data.RegisterId = 2; //aktif kullanıcı id
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
                using (var context = new AppDbContext())
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
                        data.UpdateAdminId = 2; //aktif kullanıcı id
                    }
                    else
                    {
                        data.RegisterDate = DateTime.Now;
                        data.RegisterId = 2; //aktif kullanıcı id
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
            using (var context = new AppDbContext())
            {
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
            }
            return result;
        }

        public async Task<ResultSet> DeleteAdminAsync(int id)
        {
            ResultSet result = new ResultSet();
            using (var context = new AppDbContext())
            {
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
            }
            return result;
        }
    }
}
