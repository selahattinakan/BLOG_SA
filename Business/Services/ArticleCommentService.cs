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
        private readonly AppDbContext context;
        public ArticleCommentService(AppDbContext _context)
        {
            context = _context;
        }

        public ArticleComment? GetArticleComment(int id)
        {
            return context.ArticleComment.Find(id);
        }

        public async Task<ArticleComment?> GetArticleCommentAsync(int id)
        {
            return await context.ArticleComment.FindAsync(id);
        }

        public List<ArticleComment> GetArticleComments()
        {
            return context.ArticleComment.ToList();
        }

        public async Task<List<ArticleComment>> GetArticleCommentsAsync()
        {
            return await context.ArticleComment.ToListAsync();
        }

        public ResultSet SaveArticleComment(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;
                ArticleComment? data = context.ArticleComment.FirstOrDefault(x => x.Id == articleComment.Id);
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
                    context.Add(data);
                }

                int count = context.SaveChanges();
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
                DbState state = DbState.Update;// context changetracker'dan da bakılabilir
                ArticleComment? data = await context.ArticleComment.FirstOrDefaultAsync(x => x.Id == articleComment.Id);
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
                    await context.AddAsync(data);
                }

                int count = await context.SaveChangesAsync();
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
            ArticleComment? articleComment = context.ArticleComment.FirstOrDefault(x => x.Id == id);
            if (articleComment != null)
            {
                context.Remove(articleComment);
                int count = context.SaveChanges();
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
            ArticleComment? articleComment = await context.ArticleComment.FirstOrDefaultAsync(x => x.Id == id);
            if (articleComment != null)
            {
                context.Remove(articleComment);
                int count = await context.SaveChangesAsync();
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
            ArticleComment? articleComment = await context.ArticleComment.FirstOrDefaultAsync(x => x.Id == id);
            if (articleComment != null)
            {
                articleComment.IsConfirm = confirm;
                int count = await context.SaveChangesAsync();
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
            return await context.ArticleComment.Where(x => x.ArticleId == articleId).ToListAsync();
        }
    }
}
