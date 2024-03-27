using BLOG_SA.Models;
using Business.Interfaces;
using Business.Services;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BLOG_SA.Controllers
{
    /*İlerde kimlik doğrulama ve yetkilendirme işlemleri detaylandırılacak*/
    public class LoginController : Controller
    {
        private readonly IAdminService _adminService;

        public LoginController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View(true);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserModel model, string ReturnUrl)
        {
            //returnurl güvenlik açığı olabilir, ilerde kalkacak
            Admin admin = await _adminService.LogInControlAsync(model.UserName, model.Password);
            if (admin != null && admin?.Id > 0)
            {
                List<Claim> claims = new List<Claim>()
                    {
                        new Claim (ClaimTypes.Name, admin.UserName),
                        new Claim (ClaimTypes.NameIdentifier, admin.Id.ToString())
                    };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(principal);

                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View(false);
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
