using BLOG_SA.Models;
using Business.DTOs;
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
        private readonly IArticleService articleService;
        private readonly IArticleCommentService articleCommentService;
        private readonly ISubscriberService subscriberService;
        private readonly IContactService contactService;

        public HomeController(ILogger<HomeController> logger, IArticleService _articleService, IArticleCommentService _articleCommentService, ISubscriberService _subscriberService, IContactService _contactService)
        {
            _logger = logger;
            articleService = _articleService;
            articleCommentService = _articleCommentService;
            subscriberService = _subscriberService;
            contactService = _contactService;
        }

        #region Views
        public async Task<IActionResult> IndexAsync(int page = 1, int pageSize = 5)
        {
            ViewBag.Page = page;
            ViewBag.FirstPage = false;
            ViewBag.LastPage = false;
            List<ArticleDto> articles = await articleService.GetArticlesWithCommentCountsAsync(page, pageSize);
            int totalCount = await articleService.GetArticleCountAsync(true);
            if (page == 1)
            {
                ViewBag.FirstPage = true;
            }
            if (page * pageSize >= totalCount)
            {
                ViewBag.LastPage = true;
            }
            return View(articles);
        }

        public async Task<IActionResult> ArticleAsync(int articleId)
        {
            ViewBag.FirstArticle = false;
            ViewBag.LastArticle = false;
            ViewBag.NextArticle = 0;
            ViewBag.PrevArticle = 0;
            Article article = await articleService.GetArticleWithCommentsAsync(articleId);
            List<int> articleIds = articleService.GetArticleIds();
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

            return View(article);
        }

        public async Task<ActionResult> SaveComment(ArticleComment comment)
        {
            ResultSet result = new ResultSet();
            if (Validations.IsMailValid(comment.Mail) && !string.IsNullOrEmpty(comment.FullName) && !string.IsNullOrEmpty(comment.Content))
            {
                result = await articleCommentService.SaveArticleCommentAsync(comment);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen tüm alanlarý geçerli bir þekilde doldurunuz";
            }
            return Json(result);
        }

        public async Task<ActionResult> SaveSubscriber(string subMail)
        {
            ResultSet result = new ResultSet();
            if (Validations.IsMailValid(subMail))
            {
                result = await subscriberService.SaveSubscriberAsync(subMail);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen tüm alanlarý geçerli bir þekilde doldurunuz";
            }
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
                var captchaImage = HttpContext.Request.Form["g-recaptcha-response"];
                if (string.IsNullOrEmpty(captchaImage))
                {
                    TempData["PostMessage"] = "Captche fail!";
                }
                var verified = await CheckCaptcha();
                if (verified)
                {
                    Contact contact = new Contact { FullName = FullName, Mail = Mail, Message = Message, Subject = Subject };
                    ResultSet result = await contactService.SaveContactAsync(contact);
                    TempData["PostMessage"] = result.Message;
                }
                else
                {
                    TempData["PostMessage"] = "Captche not verified!";
                }
            }
            else
            {
                TempData["PostMessage"] = "Lütfen tüm alanlarý doðru formatta doldurunuz.";
            }
            return RedirectToAction("Contact");
        }

        public async Task<bool> CheckCaptcha()
        {
            var secretKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Google")["RecaptchaV3SecretKey"];
            var postData = new List<KeyValuePair<string, string>>();
            {
                new KeyValuePair<string, string>("secret", secretKey);
                new KeyValuePair<string, string>("response", HttpContext.Request.Form["google-recaptcha-response"]);
            };

            var client = new HttpClient();
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(postData));
            var obj = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            return (bool)obj["success"];
        }

        public async Task<IActionResult> Contact()
        {
            var siteKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Google")["RecaptchaV3SiteKey"];
            ViewBag.CaptchaKey = siteKey;
            ViewBag.Message = TempData["PostMessage"]?.ToString();
            return View();
        }

        public IActionResult RSS()
        {
            return View();
        }

        public IActionResult Subscribe()
        {
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
