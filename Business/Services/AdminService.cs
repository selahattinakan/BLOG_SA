using Business.Interfaces;
using Constants;
using Constants.Enums;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;

namespace Business.Services
{

    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        private readonly IService _service;

        public AdminService(IAdminRepository repository, IService service)
        {
            _repository = repository;
            _service = service;
        }


        // Şifre kayıt ve gösterme işlemleri encrytion sınıfı ile yapılacak
        #region LogIn
        public bool LogInControl(string userName, string password)
        {
            return _repository.LogInControl(userName, password);
        }

        public async Task<Admin> LogInControlAsync(string userName, string password)
        {
            return await _repository.LogInControlAsync(userName, password);
        }
        #endregion

        public Admin? GetAdmin(int id)
        {
            return _repository.GetAdmin(id);
        }

        public async Task<Admin?> GetAdminAsync(int id)
        {
            return await _repository.GetAdminAsync(id);
        }

        public List<Admin> GetAdmins()
        {
            return _repository.GetAdmins();
        }

        public async Task<List<Admin>> GetAdminsAsync()
        {
            return await _repository.GetAdminsAsync();
        }


        public ResultSet SaveAdmin(Admin admin)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;
                Admin? data = GetAdmin(admin.Id);
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
                    data.UpdateAdminId = _service.GetActiveUserId();
                    result = _repository.UpdateAdmin(data);
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.RegisterId = _service.GetActiveUserId();
                    result = _repository.SaveAdmin(data);
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
                Admin? data = await GetAdminAsync(admin.Id);
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
                    data.UpdateAdminId = _service.GetActiveUserId();
                    result = await _repository.UpdateAdminAsync(data);
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.RegisterId = _service.GetActiveUserId();
                    result = await _repository.SaveAdminAsync(data);
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
            Admin? admin = _repository.GetAdmin(id);
            if (admin != null)
            {
                result = _repository.DeleteAdmin(admin);
            }
            return result;
        }

        public async Task<ResultSet> DeleteAdminAsync(int id)
        {
            ResultSet result = new ResultSet();
            Admin? admin = await _repository.GetAdminAsync(id);
            if (admin != null)
            {
                result = await _repository.DeleteAdminAsync(admin);
            }
            return result;
        }

        public void SP_FN_Test()
        {
            _repository.SP_FN_Test();
        }
    }
}
