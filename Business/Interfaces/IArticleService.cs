﻿using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IArticleService
    {
        public Article? GetArticle(int id);
        public Task<Article?> GetArticleAsync(int id);
        public List<Article> GetArticles();
        public Task<List<Article>> GetArticlesAsync();
        public ResultSet SaveArticle(Article article);
        public Task<ResultSet> SaveArticleAsync(Article article);
        public ResultSet DeleteArticle(int id);
        public Task<ResultSet> DeleteArticleAsync(int id);
    }
}
