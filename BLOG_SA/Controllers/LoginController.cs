using BLOG_SA.Models;
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
        public IActionResult Index()
        {
            return View(true);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserModel model, string ReturnUrl)
        {
            //returnurl güvenlik açığı olabilir, ilerde kalkacak
            using (var context = new AppDbContext())
            {
                //db işlemi başka bir katmana alınacak
                Admin? admin = await context.Admin.FirstOrDefaultAsync(x => x.UserName == model.UserName && x.Password == model.Password);
                if (admin != null)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim (ClaimTypes.Name, model.UserName)
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
