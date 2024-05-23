using Constants;
using Constants.Enums;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB_EFCore.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private readonly AppDbContext _context;

        public SettingRepository(AppDbContext context)
        {
            _context = context;
        }

        public Setting? GetSetting()
        {
            return _context.Setting.FirstOrDefault();
        }

        public Setting? GetSetting(int id)
        {
            return _context.Setting.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public async Task<Setting?> GetSettingAsync()
        {
            return await _context.Setting.FirstOrDefaultAsync();
        }

        public async Task<Setting?> GetSettingAsync(int id)
        {
            return await _context.Setting.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public ResultSet SaveSetting(Setting setting)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Add(setting);
                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = setting.Id;
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
                await _context.AddAsync(setting);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = setting.Id;
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

        public ResultSet UpdateSetting(Setting setting)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Update(setting);
                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = setting.Id;
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

        public async Task<ResultSet> UpdateSettingAsync(Setting setting)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Update(setting);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = setting.Id;
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
