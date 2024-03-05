using Constants.Enums;
using Constants;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;

namespace Business.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext context;
        private readonly IService service;
        public SettingService(AppDbContext _context, IService _service)
        {
            context = _context;
            service = _service;
        }
        public Setting? GetSetting()
        {
            return context.Setting.FirstOrDefault();
        }

        public async Task<Setting?> GetSettingAsync()
        {
            return await context.Setting.FirstOrDefaultAsync();
        }

        public ResultSet SaveSetting(Setting setting)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;
                Setting? data = context.Setting.FirstOrDefault(x => x.Id == setting.Id);
                if (data == null)
                {
                    data = new Setting();
                    state = DbState.Insert;
                }
                data.MaintenanceMode = setting.MaintenanceMode;
                data.MaintenanceImgPath = setting.MaintenanceImgPath;
                data.MaintenanceText = setting.MaintenanceText;
                data.SubscribeMode = setting.SubscribeMode;
                data.IsCommentEnable = setting.IsCommentEnable;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = service.GetActiveUserId();
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = service.GetActiveUserId();
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

        public async Task<ResultSet> SaveSettingAsync(Setting setting)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;// context changetracker'dan da bakılabilir
                Setting? data = await context.Setting.FirstOrDefaultAsync(x => x.Id == setting.Id);
                if (data == null)
                {
                    data = new Setting();
                    state = DbState.Insert;
                }
                data.MaintenanceMode = setting.MaintenanceMode;
                data.MaintenanceImgPath = setting.MaintenanceImgPath;
                data.MaintenanceText = setting.MaintenanceText;
                data.SubscribeMode = setting.SubscribeMode;
                data.IsCommentEnable = setting.IsCommentEnable;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = service.GetActiveUserId();
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = service.GetActiveUserId();
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
    }
}
