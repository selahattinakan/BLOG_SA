using BLOG_SA.Models;
using Business.DTOs;
using Business.Interfaces;
using Business.Services;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BLOG_SA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService articleService;

        public HomeController(ILogger<HomeController> logger, IArticleService _articleService)
        {
            _logger = logger;
            articleService = _articleService;
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

        public IActionResult Article()
        {
            return View();
        }


        public IActionResult Contact()
        {
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
