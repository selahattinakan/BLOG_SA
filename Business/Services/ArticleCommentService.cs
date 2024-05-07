using Constants.Enums;
using Constants;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Business.Interfaces;

namespace Business.Services
{
    public class ArticleCommentService : IArticleCommentService
    {
        private readonly AppDbContext _context;
        public ArticleCommentService(AppDbContext context)
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
                DbState state = DbState.Update;
                ArticleComment? data = _context.ArticleComment.FirstOrDefault(x => x.Id == articleComment.Id);
                if (data == null)
                {
                    data = new ArticleComment();
                    state = DbState.Insert;
                }

                data.ArticleId = articleComment.ArticleId;
                data.Content = articleComment.Content;
                data.FullName = articleComment.FullName;
                data.Mail = articleComment.Mail;
                data.IsConfirm = articleComment.IsConfirm;
                data.ParentCommentId = articleComment.ParentCommentId;

                if (state == DbState.Insert)
                {
                    data.RegisterDate = DateTime.Now;
                    _context.Add(data);
                }

                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = data.Id;
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
                if (await GetCommentsCount() > 1000) //max 1000 olsun, saldırı olursa. ilerde ayarlar içine al
                {
                    throw new Exception("Max kayıt adedi aşıldı");
                }

                DbState state = DbState.Update;// _context changetracker'dan da bakılabilir
                ArticleComment? data = await _context.ArticleComment.FirstOrDefaultAsync(x => x.Id == articleComment.Id);
                if (data == null)
                {
                    data = new ArticleComment();
                    state = DbState.Insert;
                }
                data.ArticleId = articleComment.ArticleId;
                data.Content = articleComment.Content;
                data.FullName = articleComment.FullName;
                data.Mail = articleComment.Mail;
                data.IsConfirm = articleComment.IsConfirm;
                data.ParentCommentId = articleComment.ParentCommentId;

                if (state == DbState.Insert)
                {
                    data.RegisterDate = DateTime.Now;
                    await _context.AddAsync(data);
                }

                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = data.Id;
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

        public ResultSet DeleteArticleComment(int id)
        {
            ResultSet result = new ResultSet();
            ArticleComment? articleComment = _context.ArticleComment.FirstOrDefault(x => x.Id == id);
            if (articleComment != null)
            {
                _context.Remove(articleComment);
                int count = _context.SaveChanges();
                if (count <= 0)
                {
                    result.Result = Result.Fail;
                    result.Message = "Silme işlemi başarısız";
                }
            }
            return result;
        }

        public async Task<ResultSet> DeleteArticleCommentAsync(int id)
        {
            ResultSet result = new ResultSet();
            ArticleComment? articleComment = await _context.ArticleComment.FirstOrDefaultAsync(x => x.Id == id);
            if (articleComment != null)
            {
                _context.Remove(articleComment);
                int count = await _context.SaveChangesAsync();
                if (count <= 0)
                {
                    result.Result = Result.Fail;
                    result.Message = "Silme işlemi başarısız";
                }
            }
            return result;
        }

        public async Task<ResultSet> SetConfirmAsync(int id, bool confirm)
        {
            ResultSet result = new ResultSet();
            ArticleComment? articleComment = await _context.ArticleComment.FirstOrDefaultAsync(x => x.Id == id);
            if (articleComment != null)
            {
                articleComment.IsConfirm = confirm;
                int count = await _context.SaveChangesAsync();
                if (count <= 0)
                {
                    result.Result = Result.Fail;
                    result.Message = "Silme işlemi başarısız";
                }
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
