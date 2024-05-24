using Business.Interfaces;
using Constants;
using Constants.Enums;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;

namespace Business.Services
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _repository;
        private readonly IService _service;
        public SettingService(ISettingRepository repository, IService service)
        {
            _repository = repository;
            _service = service;
        }
        public Setting? GetSetting()
        {
            return _repository.GetSetting();
        }

        public async Task<Setting?> GetSettingAsync()
        {
            return await _repository.GetSettingAsync();
        }

        public ResultSet SaveSetting(Setting setting)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;
                Setting? data = _repository.GetSetting(setting.Id);
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
                    result = _repository.UpdateSetting(data);
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = _service.GetActiveUserId();
                    result = _repository.SaveSetting(data);
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
                DbState state = DbState.Update;
                Setting? data = await _repository.GetSettingAsync(setting.Id);
                if (data == null)
                {
                    data = new Setting();
                    state = DbState.Insert;
                }
                data.MaintenanceMode = setting.MaintenanceMode;
                data.MaintenanceImgPath = setting.MaintenanceImgPath;
                data.MaintenanceText = setting.MaintenanceText;
                data.BioText = setting.BioText;
                data.SubscribeMode = setting.SubscribeMode;
                data.IsCommentEnable = setting.IsCommentEnable;
                data.IsElasticsearchEnable = setting.IsElasticsearchEnable;
                data.IsRedisEnable = setting.IsRedisEnable;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = _service.GetActiveUserId();
                    result = await _repository.UpdateSettingAsync(data);
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = _service.GetActiveUserId();
                    result = await _repository.SaveSettingAsync(data);
                }
                result.Object = data;

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
