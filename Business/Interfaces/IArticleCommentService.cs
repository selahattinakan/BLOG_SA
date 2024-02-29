using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IArticleCommentService
    {
        public ArticleComment? GetArticleComment(int id);
        public Task<ArticleComment?> GetArticleCommentAsync(int id);
        public List<ArticleComment> GetArticleComments();
        public Task<List<ArticleComment>> GetArticleCommentsAsync();
        public ResultSet SaveArticleComment(ArticleComment articleComment);
        public Task<ResultSet> SaveArticleCommentAsync(ArticleComment articleComment);
        public ResultSet DeleteArticleComment(int id);
        public Task<ResultSet> DeleteArticleCommentAsync(int id);
        public Task<ResultSet> SetConfirmAsync(int id, bool confirm);
        public Task<List<ArticleComment>> GetArticleCommentsAsync(int articleId);
    }
}
