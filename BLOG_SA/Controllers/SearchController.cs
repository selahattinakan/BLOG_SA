using Business.Interfaces;
using DB_EFCore.Entity;
using Elasticsearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace BLOG_SA.Controllers
{
    public class SearchController : Controller
    {
        private readonly IElasticsearch _elasticsearch;
        private readonly IArticleService _articleService;

        public SearchController(IElasticsearch elasticsearch, IArticleService articleService)
        {
            _elasticsearch = elasticsearch;
            _articleService = articleService;

        }
        public async Task<IActionResult> IndexAsync(int page = 1, int pageSize = 5, string searchText = "")
        {
            ViewBag.Page = page;
            ViewBag.FirstPage = false;
            ViewBag.LastPage = false;
            ViewBag.SearchText = searchText;
            var articles = await _elasticsearch.SearchAsync(searchText, page, pageSize);

            long totalCount = articles.totalCount;
            if (page == 1)
            {
                ViewBag.FirstPage = true;
            }
            if (page * pageSize >= totalCount)
            {
                ViewBag.LastPage = true;
            }
            return View(articles.list);
        }

        //public async Task<IActionResult> ElasticDataInitial()
        //{
        //    List<Article> currents = await _articleService.GetArticlesAsync();
        //    currents.ForEach(async x =>
        //    {
        //        ES_Article article = new ES_Article()
        //        {
        //            Content = x.IntroContent,
        //            Title = x.Title,
        //            Tags = string.Empty
        //        };
        //        var result = await _elasticsearch.SaveAsync(article, x.Id);
        //    });

        //    return Content("Ok");
        //}
    }
}
