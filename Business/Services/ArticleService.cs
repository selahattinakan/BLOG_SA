﻿using Business.Interfaces;
using Constants;
using Constants.DTOs;
using Constants.Enums;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;
using Elasticsearch.Models;

namespace Business.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repository;
        private readonly IService _service;
        private readonly ISettingService _settingService;

        public ArticleService(IArticleRepository repository, IService service, ISettingService settingService)
        {
            _repository = repository;
            _service = service;
            _settingService = settingService;
        }

        public Article? GetArticle(int id)
        {
            return _repository.GetArticle(id);
        }

        public async Task<Article?> GetArticleAsync(int id)
        {
            return await _repository.GetArticleAsync(id);
        }

        public List<Article> GetArticles()
        {
            return _repository.GetArticles();
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            return await _repository.GetArticlesAsync();
        }

        public async Task<List<Article>> GetArticlesForRssAsync()
        {
            return await _repository.GetArticlesForRssAsync();
        }

        public List<Article> GetArticlesNoTracking()
        {// as no tracking ile her bir kayıt için state durumu tutulmuyor(flagler) ve performans artıyor. bu sorgu sonucu 1m kayıt dönseydi 1m flag ramde tutulacaktı
         // bu kayıtlardan birinde update,insert gibi bir işlem yapılacaksa flagler takip edilmediği için savechanges öncesi manuel işlemler yapılması gerekir
            return _repository.GetArticlesNoTracking();
        }

        public async Task<Article> GetArticleWithCommentsAsync(int id)
        {
            return await _repository.GetArticleWithCommentsAsync(id);
        }

        public async Task<List<Article>> GetArticlesWithCommentsAsync()
        {
            return await _repository.GetArticlesWithCommentsAsync();
        }

        public List<int> GetArticleIds()
        {
            return _repository.GetArticleIds();
        }

        public async Task<List<ArticleDto>> GetArticlesWithCommentCountsAsync(int page, int pageSize)
        {
            return await _repository.GetArticlesWithCommentCountsAsync(page, pageSize);
        }

        public async Task<int> GetArticleCountAsync(bool enabled)
        {
            return await _repository.GetArticleCountAsync(enabled);
        }

        public ResultSet SaveArticle(Article article)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;
                Article? data = _repository.GetArticleAsNoTracking(article.Id);
                if (data == null)
                {
                    data = new Article();
                    state = DbState.Insert;
                }
                data.Title = article.Title;
                data.Content = article.Content;
                data.IntroContent = article.IntroContent;
                data.PublishDate = article.PublishDate;
                data.Enable = article.Enable;
                data.PhotoIndex = article.PhotoIndex;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = _service.GetActiveUserId();
                    result = _repository.UpdateArticle(data);
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = _service.GetActiveUserId();
                    result = _repository.SaveArticle(data);
                }

                result.Object = data;
                result.TempData = state;
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultSet> SaveArticleAsync(Article article)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;// _context changetracker'dan da bakılabilir
                Article? data = await _repository.GetArticleAsNoTrackingAsync(article.Id);
                if (data == null)
                {
                    data = new Article();
                    state = DbState.Insert;
                }
                data.Title = article.Title;
                data.Content = article.Content;
                data.IntroContent = article.IntroContent;
                data.PublishDate = article.PublishDate;
                data.Enable = article.Enable;
                data.PhotoIndex = article.PhotoIndex;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = _service.GetActiveUserId();
                    result = await _repository.UpdateArticleAsync(data);
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = _service.GetActiveUserId();
                    result = await _repository.SaveArticleAsync(data);
                }

                result.Object = data;
                result.TempData = state;
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultSet DeleteArticle(int id)
        {
            ResultSet result = new ResultSet();
            Article? article = _repository.GetArticle(id);
            if (article != null)
            {
                result = _repository.DeleteArticle(article);
            }
            return result;
        }

        /// <summary>
        /// Yorumlarla birlikte makaleyi siler.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultSet> DeleteArticleAsync(int id)
        {
            ResultSet result = new ResultSet();
            Article? article = await _repository.GetArticleAsync(id);
            if (article != null)
            {
                result = await _repository.DeleteArticleAsync(article);
            }
            return result;
        }
    }
}
