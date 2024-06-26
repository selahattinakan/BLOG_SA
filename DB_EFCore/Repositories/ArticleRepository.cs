﻿using Constants;
using Constants.DTOs;
using Constants.Enums;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DB_EFCore.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _context;

        public ArticleRepository(AppDbContext context)
        {
            _context = context;
        }

        public Article? GetArticle(int id)
        {
            return _context.Article.Find(id);
        }

        public async Task<Article?> GetArticleAsync(int id)
        { 
            return await _context.Article.FindAsync(id);
        }

        public Article? GetArticleAsNoTracking(int id)
        {
            return _context.Article.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);
        }

        public async Task<Article?> GetArticleAsNoTrackingAsync(int id)
        {
            return await _context.Article.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Article> GetArticles()
        {
            return _context.Article.IgnoreQueryFilters().ToList();
        }

        /// <summary>
        /// Get all articles with IgnoreQueryFilter
        /// </summary>
        /// <returns></returns>
        public async Task<List<Article>> GetArticlesAsync()
        {
            return await _context.Article.IgnoreQueryFilters().ToListAsync();
        }

        public async Task<List<Article>> GetArticlesForRssAsync()
        {
            return await _context.Article.AsNoTracking().ToListAsync();
        }

        public List<Article> GetArticlesNoTracking()
        {// as no tracking ile her bir kayıt için state durumu tutulmuyor(flagler) ve performans artıyor. bu sorgu sonucu 1m kayıt dönseydi 1m flag ramde tutulacaktı
         // bu kayıtlardan birinde update,insert gibi bir işlem yapılacaksa flagler takip edilmediği için savechanges öncesi manuel işlemler yapılması gerekir
            return _context.Article.AsNoTracking().IgnoreQueryFilters().ToList();
        }

        public async Task<Article> GetArticleWithCommentsAsync(int id)
        {
            return await _context.Article.Include(x => x.ArticleComments.Where(x => x.IsConfirm)).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Article>> GetArticlesWithCommentsAsync()
        {
            return await _context.Article.Include(x => x.ArticleComments).ToListAsync();
        }

        public List<int> GetArticleIds()
        {
            var dataTable = _context.GetDataTableFromSP($"SP_GetArticleIds");
            return dataTable.Rows.OfType<DataRow>().Select(dr => dr.Field<int>("Id")).ToList();
        }

        public async Task<List<ArticleDto>> GetArticlesWithCommentCountsAsync(int page, int pageSize)
        {
            var articles = await _context.Article.Select(x => new ArticleDto
            {
                Id = x.Id,
                PhotoIndex = x.PhotoIndex,
                Title = x.Title,
                PublishDate = x.PublishDate,
                ReadMinute = x.ReadMinute,
                CommentCounts = x.ArticleComments.Where(x => x.IsConfirm).Count(),
                IntroContent = x.IntroContent,
                Enable = x.Enable
            }).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return articles;
        }

        public async Task<int> GetArticleCountAsync(bool enabled)
        {
            return await _context.Article.CountAsync();
        }

        public ResultSet SaveArticle(Article article)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Add(article);
                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = article.Id;
                }
                else
                {
                    result.Result = Result.Fail;
                    result.Message = "Db işlemi başarısız";
                }
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
                await _context.AddAsync(article);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = article.Id;
                }
                else
                {
                    result.Result = Result.Fail;
                    result.Message = "Db işlemi başarısız";
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultSet UpdateArticle(Article article)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Update(article);
                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = article.Id;
                }
                else
                {
                    result.Result = Result.Fail;
                    result.Message = "Db işlemi başarısız";
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultSet> UpdateArticleAsync(Article article)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Update(article);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = article.Id;
                }
                else
                {
                    result.Result = Result.Fail;
                    result.Message = "Db işlemi başarısız";
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultSet DeleteArticle(Article article)
        {
            ResultSet result = new ResultSet();
            _context.Remove(article);
            int count = _context.SaveChanges();
            if (count <= 0)
            {
                result.Result = Result.Fail;
                result.Message = "Silme işlemi başarısız";
            }
            else
            {
                result.Result = Result.Success;
                result.Message = "Silme işlemi başarılı";
            }
            return result;
        }

        /// <summary>
        /// Yorumlarla birlikte makaleyi siler.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultSet> DeleteArticleAsync(Article article)
        {
            ResultSet result = new ResultSet();
            _context.Remove(article);
            int count = await _context.SaveChangesAsync();
            if (count <= 0)
            {
                result.Result = Result.Fail;
                result.Message = "Silme işlemi başarısız";
            }
            else
            {
                result.Result = Result.Success;
                result.Message = "Silme işlemi başarılı";
            }
            return result;
        }


    }
}
