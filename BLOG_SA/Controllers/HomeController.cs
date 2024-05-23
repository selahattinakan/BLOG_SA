using BLOG_SA.Models;
using Constants.DTOs;
using Business.Helpers;
using Business.Interfaces;
using Business.Services;
using Constants;
using Constants.Enums;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace BLOG_SA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly IArticleCommentService _articleCommentService;
        private readonly ISubscriberService _subscriberService;
        private readonly IContactService _contactService;
        private readonly ISettingService _settingService;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService, IArticleCommentService articleCommentService, ISubscriberService subscriberService, IContactService contactService, ISettingService settingService)
        {
            _logger = logger;
            _articleService = articleService;
            _articleCommentService = articleCommentService;
            _subscriberService = subscriberService;
            _contactService = contactService;
            _settingService = settingService;
        }

        #region Views
        public async Task<IActionResult> IndexAsync(int page = 1, int pageSize = 5)
        {
            ViewBag.Page = page;
            ViewBag.FirstPage = false;
            ViewBag.LastPage = false;
            Setting? setting = await _settingService.GetSettingAsync();
            ViewBag.Bio = setting?.BioText;

            List<ArticleDto> articles = await _articleService.GetArticlesWithCommentCountsAsync(page, pageSize);
            int totalCount = await _articleService.GetArticleCountAsync(true);
            if (page == 1)
            {
                ViewBag.FirstPage = true;
            }
            if (page * pageSize >= totalCount)
            {
                ViewBag.LastPage = true;
            }
            _logger.LogInformation("Anasayfa açýlýyor");
            return View(articles);
        }

        public async Task<IActionResult> ArticleAsync(int articleId)
        {
            ViewBag.FirstArticle = false;
            ViewBag.LastArticle = false;
            ViewBag.NextArticle = 0;
            ViewBag.PrevArticle = 0;
            Setting? setting = await _settingService.GetSettingAsync();
            ViewBag.Bio = setting?.BioText;
            Article article = await _articleService.GetArticleWithCommentsAsync(articleId);

            if (article == null) return View(new Article() { ArticleComments = new List<ArticleComment>()});

            List<int> articleIds = _articleService.GetArticleIds();
            var index = articleIds.IndexOf(article.Id);
            if (index == 0)
            {
                ViewBag.FirstArticle = true;
            }
            else
            {
                ViewBag.PrevArticle = articleIds[index - 1];
            }
            if (index == articleIds.Count - 1)
            {
                ViewBag.LastArticle = true;
            }
            else
            {
                ViewBag.NextArticle = articleIds[index + 1];
            }

            _logger.LogInformation("Makale açýlýyor");
            return View(article);
        }

        public async Task<ActionResult> SaveComment(ArticleComment comment)
        {
            ResultSet result = new ResultSet();
            if (Validations.IsMailValid(comment.Mail) && !string.IsNullOrEmpty(comment.FullName) && !string.IsNullOrEmpty(comment.Content))
            {
                result = await _articleCommentService.SaveArticleCommentAsync(comment);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen tüm alanlarý geçerli bir þekilde doldurunuz";
            }

            _logger.LogInformation("Yorum kaydetme iþlemi");
            return Json(result);
        }

        public async Task<ActionResult> SaveSubscriber(string subMail)
        {
            ResultSet result = new ResultSet();
            if (Validations.IsMailValid(subMail))
            {
                result = await _subscriberService.SaveSubscriberAsync(subMail);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen tüm alanlarý geçerli bir þekilde doldurunuz";
            }

            _logger.LogInformation("Abone kaydetme iþlemi");
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> SaveContact()
        {
            string FullName = HttpContext.Request.Form["FullName"];
            string Mail = HttpContext.Request.Form["Mail"];
            string Subject = HttpContext.Request.Form["Subject"];
            string Message = HttpContext.Request.Form["Message"];
            if (!string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(FullName) && Validations.IsMailValid(Mail))
            {
                Contact contact = new Contact { FullName = FullName, Mail = Mail, Message = Message, Subject = Subject };
                ResultSet result = await _contactService.SaveContactAsync(contact);
                TempData["PostMessage"] = result.Message;
            }
            else
            {
                TempData["PostMessage"] = "Lütfen tüm alanlarý doðru formatta doldurunuz.";
            }

            _logger.LogInformation("Ýletiþim kaydetme iþlemi");
            return RedirectToAction("Contact");
        }

        public async Task<IActionResult> Contact()
        {
            Setting? setting = await _settingService.GetSettingAsync();
            ViewBag.Bio = setting?.BioText;

            var siteKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Google")["RecaptchaV3SiteKey"];
            ViewBag.CaptchaKey = siteKey;
            ViewBag.Message = TempData["PostMessage"]?.ToString();

            _logger.LogInformation("Ýletiþim sayfasý açýlýyor");
            return View();
        }

        public IActionResult RSS()
        {
            _logger.LogInformation("Rss sayfasý açýlýyor");
            return View();
        }

        public IActionResult Subscribe()
        {
            _logger.LogInformation("Abone sayfasý açýlýyor");
            return View();
        }

        public async Task<IActionResult> Chat()
        {
            Setting? setting = await _settingService.GetSettingAsync();
            ViewBag.Bio = setting?.BioText;

            _logger.LogInformation("Chat sayfasý açýlýyor..");
            return View();
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
