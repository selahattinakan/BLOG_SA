using BLOG_SA.Models;
using Business.Services;
using Business.Interfaces;
using Constants;
using Constants.Enums;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BLOG_SA.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IArticleService articleService;
        private readonly IArticleCommentService articleCommentService;
        private readonly IContactService contactService;
        private readonly ISettingService settingService;
        private readonly ISubscriberService subscriberService;

        public AdminController(IArticleService _articleService, IAdminService _adminService, IArticleCommentService _articleCommentService, IContactService _contactService, ISettingService _settingService, ISubscriberService _subscriberService)
        {
            articleService = _articleService;
            adminService = _adminService;
            articleCommentService = _articleCommentService;
            contactService = _contactService;
            settingService = _settingService;
            subscriberService = _subscriberService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region User
        public IActionResult User()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAdmins()
        {
            //jquery datatable ile listeleme işlemleri yapılıyor, bu bir backend projesi olduğu için standart kolay yöntem ne ise o şekilde listeleme yapıldı, performans ya da işlevsellik üzerinde durulmadı
            //AdminService adminService = new AdminService();
            List<Admin> admins = await adminService.GetAdminsAsync();
            return Json(admins);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdmin(Admin admin)
        {
            ResultSet result = new ResultSet();
            if (!string.IsNullOrEmpty(admin.FullName) && !string.IsNullOrEmpty(admin.UserName) && !string.IsNullOrEmpty(admin.Password))
            {
                //AdminService adminService = new AdminService();
                result = await adminService.SaveAdminAsync(admin);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen tüm alanları doldurunuz";
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            ResultSet result = new ResultSet();
            if (id > 0)
            {
                //AdminService adminService = new AdminService();
                result = await adminService.DeleteAdminAsync(id);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen bir seçim yapınız";
            }
            return Json(result);
        }
        #endregion

        #region Setting
        public IActionResult Setting()
        {
            //SettingService settingService = new SettingService();
            Setting setting = settingService.GetSetting();
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSetting(Setting setting)
        {
            //SettingService settingService = new SettingService();
            ResultSet result = await settingService.SaveSettingAsync(setting);
            return Json(result);
        }
        #endregion

        #region Contact
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetContacts()
        {
            //ContactService contactService = new ContactService();
            List<Contact> contacts = await contactService.GetContactsAsync();
            return Json(contacts);
        }

        #endregion

        #region Subscriber
        public IActionResult Subscriber()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetSubscribers()
        {
            //SubscriberService subscriberService = new SubscriberService();
            List<Subscriber> subscribers = await subscriberService.GetSubscribersAsync();
            return Json(subscribers);
        }

        #endregion

        #region Article
        public IActionResult Article()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetArticles()
        {
            //ArticleService articleService = new ArticleService();
            List<Article> articles = await articleService.GetArticlesAsync();
            return Json(articles);
        }

        [HttpPost]
        public async Task<IActionResult> SaveArticle(Article article)
        {
            // form kontrolleri standardize edilecek
            ResultSet result = new ResultSet();
            if (!string.IsNullOrEmpty(article.Title) && !string.IsNullOrEmpty(article.Content) && article.PublishDate != DateTime.MinValue && article.PhotoIndex > 0)
            {
                //ArticleService articleService = new ArticleService();
                result = await articleService.SaveArticleAsync(article);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen tüm alanları doldurunuz";
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            ResultSet result = new ResultSet();
            if (id > 0)
            {
                //ArticleService articleService = new ArticleService();
                result = await articleService.DeleteArticleAsync(id);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen bir seçim yapınız";
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetTestArticle()
        {
            //ArticleService articleService = new ArticleService();
            Article article = await articleService.GetArticleAsync(1);
            return Json(article);
        }
        #endregion

        #region ArticleComment
        public IActionResult ArticleComment(Article article)
        {
            return View(article.Id);
        }

        [HttpPost]
        public async Task<IActionResult> GetArticleComments(int articleId)
        {
            //ArticleCommentService articleCommentService = new ArticleCommentService();
            List<ArticleComment> articleComments = await articleCommentService.GetArticleCommentsAsync(articleId);
            return Json(articleComments);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmArticleComment(int id, bool confirm)
        {
            ResultSet result = new ResultSet();
            if (id > 0)
            {
                //ArticleCommentService articleCommentService = new ArticleCommentService();
                result = await articleCommentService.SetConfirmAsync(id, confirm);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen bir seçim yapınız";
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteArticleComment(int id)
        {
            ResultSet result = new ResultSet();
            if (id > 0)
            {
                //ArticleCommentService articleCommentService = new ArticleCommentService();
                result = await articleCommentService.DeleteArticleCommentAsync(id);
            }
            else
            {
                result.Result = Result.Fail;
                result.Message = "Lütfen bir seçim yapınız";
            }
            return Json(result);
        }

        #endregion

    }
}
