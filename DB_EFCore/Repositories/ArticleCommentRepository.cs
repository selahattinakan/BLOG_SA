using Constants;
using Constants.Enums;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DB_EFCore.Repositories
{
    public class ArticleCommentRepository : IArticleCommentRepository
    {
        private readonly AppDbContext _context;
        public ArticleCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public ArticleComment? GetArticleComment(int id)
        {
            return _context.ArticleComment.Find(id);
        }

        public async Task<ArticleComment?> GetArticleCommentAsync(int id)
        {
            return await _context.ArticleComment.FindAsync(id);
        }

        public List<ArticleComment> GetArticleComments()
        {
            return _context.ArticleComment.ToList();
        }

        public async Task<List<ArticleComment>> GetArticleCommentsAsync()
        {
            return await _context.ArticleComment.ToListAsync();
        }

        public ResultSet SaveArticleComment(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Add(articleComment);
                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = articleComment.Id;
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

        public async Task<ResultSet> SaveArticleCommentAsync(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Add(articleComment);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = articleComment.Id;
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

        public ResultSet UpdateArticleComment(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Update(articleComment);
                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = articleComment.Id;
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

        public async Task<ResultSet> UpdateArticleCommentAsync(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            try
            {
                _context.Update(articleComment);
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = articleComment.Id;
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

        public ResultSet DeleteArticleComment(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            _context.Remove(articleComment);
            int count = _context.SaveChanges();
            if (count <= 0)
            {
                result.Result = Result.Fail;
                result.Message = "Silme işlemi başarısız";
            }
            return result;
        }

        public async Task<ResultSet> DeleteArticleCommentAsync(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            _context.Remove(articleComment);
            int count = await _context.SaveChangesAsync();
            if (count <= 0)
            {
                result.Result = Result.Fail;
                result.Message = "Silme işlemi başarısız";
            }
            return result;
        }

        public async Task<ResultSet> SetConfirmAsync(ArticleComment articleComment, bool confirm)
        {
            ResultSet result = new ResultSet();
            articleComment.IsConfirm = confirm;
            int count = await _context.SaveChangesAsync();
            if (count <= 0)
            {
                result.Result = Result.Fail;
                result.Message = "İşlem başarısız";
            }
            return result;
        }

        public async Task<List<ArticleComment>> GetArticleCommentsAsync(int articleId)
        {
            return await _context.ArticleComment.Where(x => x.ArticleId == articleId).ToListAsync();
        }

        public async Task<int> GetCommentsCount()
        {
            return (await _context.ArticleComment.ToListAsync()).Count;
        }


    }
}
