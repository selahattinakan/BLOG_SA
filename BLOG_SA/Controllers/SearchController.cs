﻿using Business.Interfaces;
using DB_EFCore.Entity;
using Elasticsearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace BLOG_SA.Controllers
{
    public class SearchController : Controller
    {
        private readonly IElasticsearchService _elasticsearch;
        private readonly IArticleService _articleService;
        private readonly ISettingCache _settingCache;

        public SearchController(IElasticsearchService elasticsearch, IArticleService articleService, ISettingCache settingCache)
        {
            _elasticsearch = elasticsearch;
            _articleService = articleService;
            _settingCache = settingCache;
        }
        public async Task<IActionResult> IndexAsync(int page = 1, int pageSize = 5, string searchText = "")
        {
            ViewBag.Page = page;
            ViewBag.FirstPage = false;
            ViewBag.LastPage = false;
            ViewBag.SearchText = searchText;
            Setting? setting = await _settingCache.GetSettingAsync();
            ViewBag.Bio = setting?.BioText;
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
