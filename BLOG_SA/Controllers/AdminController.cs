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
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IArticleService _articleService;
        private readonly IArticleCommentService _articleCommentService;
        private readonly IContactService _contactService;
        private readonly ISettingService _settingService;
        private readonly ISubscriberService _subscriberService;
        private readonly IService _service;

        public AdminController(IArticleService articleService, IAdminService adminService, IArticleCommentService articleCommentService, IContactService contactService, ISettingService settingService, ISubscriberService subscriberService, IService service)
        {
            _articleService = articleService;
            _adminService = adminService;
            _articleCommentService = articleCommentService;
            _contactService = contactService;
            _settingService = settingService;
            _subscriberService = subscriberService;
            _service = service;
        }

        public IActionResult Index()
        {
            ViewData["UserName"] = _service.GetActiveUserName();
            return View();
        }

        #region User
        public IActionResult User()
        {
            ViewData["UserName"] = _service.GetActiveUserName();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAdmins()
        {
            //jquery datatable ile listeleme işlemleri yapılıyor, bu bir backend projesi olduğu için standart kolay yöntem ne ise o şekilde listeleme yapıldı, performans ya da işlevsellik üzerinde durulmadı
            //AdminService adminService = new AdminService();
            List<Admin> admins = await _adminService.GetAdminsAsync();
            return Json(admins);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdmin(Admin admin)
        { //Validation ve ModalState eklenecek
            ResultSet result = new ResultSet();
            if (!string.IsNullOrEmpty(admin.FullName) && !string.IsNullOrEmpty(admin.UserName) && !string.IsNullOrEmpty(admin.Password))
            {
                //AdminService adminService = new AdminService();
                result = await _adminService.SaveAdminAsync(admin);
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
                result = await _adminService.DeleteAdminAsync(id);
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
            ViewData["UserName"] = _service.GetActiveUserName();
            Setting setting = _settingService.GetSetting();
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSetting(Setting setting)
        {
            //SettingService settingService = new SettingService();
            ResultSet result = await _settingService.SaveSettingAsync(setting);
            return Json(result);
        }
        #endregion

        #region Contact
        public IActionResult Contact()
        {
            ViewData["UserName"] = _service.GetActiveUserName();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetContacts()
        {
            //ContactService contactService = new ContactService();
            List<Contact> contacts = await _contactService.GetContactsAsync();
            return Json(contacts);
        }

        #endregion

        #region Subscriber
        public IActionResult Subscriber()
        {
            ViewData["UserName"] = _service.GetActiveUserName();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetSubscribers()
        {
            //SubscriberService subscriberService = new SubscriberService();
            List<Subscriber> subscribers = await _subscriberService.GetSubscribersAsync();
            return Json(subscribers);
        }

        #endregion

        #region Article
        public IActionResult Article()
        {
            ViewData["UserName"] = _service.GetActiveUserName();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetArticles()
        {
            //ArticleService articleService = new ArticleService();
            List<Article> articles = await _articleService.GetArticlesAsync();
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
                result = await _articleService.SaveArticleAsync(article);
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
                result = await _articleService.DeleteArticleAsync(id);
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
            Article article = await _articleService.GetArticleAsync(1);
            return Json(article);
        }
        #endregion

        #region ArticleComment
        public IActionResult ArticleComment(Article article)
        {
            ViewData["UserName"] = _service.GetActiveUserName();
            return View(article.Id);
        }

        [HttpPost]
        public async Task<IActionResult> GetArticleComments(int articleId)
        {
            //ArticleCommentService articleCommentService = new ArticleCommentService();
            List<ArticleComment> articleComments = await _articleCommentService.GetArticleCommentsAsync(articleId);
            return Json(articleComments);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmArticleComment(int id, bool confirm)
        {
            ResultSet result = new ResultSet();
            if (id > 0)
            {
                //ArticleCommentService articleCommentService = new ArticleCommentService();
                result = await _articleCommentService.SetConfirmAsync(id, confirm);
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
                result = await _articleCommentService.DeleteArticleCommentAsync(id);
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
