using Constants;
using Constants.DTOs;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        public Article? GetArticle(int id);
        public Article? GetArticleAsNoTracking(int id);
        public Task<Article?> GetArticleAsync(int id);
        public Task<Article?> GetArticleAsNoTrackingAsync(int id);
        public List<Article> GetArticles();
        public Task<List<Article>> GetArticlesAsync();
        public Task<List<Article>> GetArticlesForRssAsync();
        public Task<Article> GetArticleWithCommentsAsync(int id);
        public Task<List<Article>> GetArticlesWithCommentsAsync();
        public Task<List<ArticleDto>> GetArticlesWithCommentCountsAsync(int page, int pageSize);
        public Task<int> GetArticleCountAsync(bool enabled);
        public ResultSet SaveArticle(Article article);
        public Task<ResultSet> SaveArticleAsync(Article article);
        public ResultSet UpdateArticle(Article article);
        public Task<ResultSet> UpdateArticleAsync(Article article);
        public ResultSet DeleteArticle(Article article);
        public Task<ResultSet> DeleteArticleAsync(Article article);
        public List<int> GetArticleIds();
        public List<Article> GetArticlesNoTracking();
    }
}
