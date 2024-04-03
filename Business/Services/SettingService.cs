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
        private readonly AppDbContext _context;
        private readonly IService _service;
        private readonly IRedisService _redisService;
        public SettingService(AppDbContext context, IService service, IRedisService redisService)
        {
            _context = context;
            _service = service;
            _redisService = redisService;
        }
        public Setting? GetSetting()
        {
            return _context.Setting.FirstOrDefault();
        }

        public async Task<Setting?> GetSettingAsync()
        {
            Setting? setting = await _redisService.GetSettingFromCache(1);
            return setting != null ? setting : await _context.Setting.FirstOrDefaultAsync();
        }

        public ResultSet SaveSetting(Setting setting)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;
                Setting? data = _context.Setting.FirstOrDefault(x => x.Id == setting.Id);
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
                data.IsElasticsearchEnable = setting.IsElasticsearchEnable;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = _service.GetActiveUserId();
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = _service.GetActiveUserId();
                    _context.Add(data);
                }

                int count = _context.SaveChanges();
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

        //design pattern'e göre redis ve ayarlar ayrılacak ya da başka bir servis çalışacak SettingWithRedis.cs gibi
        public async Task<ResultSet> SaveSettingAsync(Setting setting)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;// _context changetracker'dan da bakılabilir
                Setting? data = await _context.Setting.FirstOrDefaultAsync(x => x.Id == setting.Id);
                if (data == null)
                {
                    data = new Setting();
                    state = DbState.Insert;
                }
                data.MaintenanceMode = setting.MaintenanceMode;
                data.MaintenanceImgPath = setting.MaintenanceImgPath;
                data.MaintenanceText = setting.MaintenanceText;
                data.BioText= setting.BioText;
                data.SubscribeMode = setting.SubscribeMode;
                data.IsCommentEnable = setting.IsCommentEnable;
                data.IsElasticsearchEnable = setting.IsElasticsearchEnable;
                data.IsRedisEnable = setting.IsRedisEnable;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = _service.GetActiveUserId();
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = _service.GetActiveUserId();
                    await _context.AddAsync(data);
                }

                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = data.Id;
                    if (setting.IsRedisEnable)
                    {
                        await _redisService.CreateSettingCache(setting); 
                    }
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
