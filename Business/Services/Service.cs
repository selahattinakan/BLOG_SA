using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Business.Services
{
    public class Service : IService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public Service(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetActiveUserId()
        {
            return Int32.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
        }

        public string GetActiveUserName()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value.ToString();
        }
    }
}
