using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Repositories.Interfaces
{
    public interface IArticleCommentRepository
    {
        public ArticleComment? GetArticleComment(int id);
        public Task<ArticleComment?> GetArticleCommentAsync(int id);
        public List<ArticleComment> GetArticleComments();
        public Task<List<ArticleComment>> GetArticleCommentsAsync();
        public ResultSet SaveArticleComment(ArticleComment articleComment);
        public Task<ResultSet> SaveArticleCommentAsync(ArticleComment articleComment);
        public ResultSet UpdateArticleComment(ArticleComment articleComment);
        public Task<ResultSet> UpdateArticleCommentAsync(ArticleComment articleComment);
        public ResultSet DeleteArticleComment(ArticleComment articleComment);
        public Task<ResultSet> DeleteArticleCommentAsync(ArticleComment articleComment);
        public Task<ResultSet> SetConfirmAsync(ArticleComment articleComment, bool confirm);
        public Task<List<ArticleComment>> GetArticleCommentsAsync(int articleId);
        public Task<int> GetCommentsCount();
    }
}
