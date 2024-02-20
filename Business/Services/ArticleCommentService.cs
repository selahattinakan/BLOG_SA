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

namespace Business.Services
{
    public class ArticleCommentService
    {
        public ArticleComment? GetArticleComment(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.ArticleComment.FirstOrDefault(x => x.Id == id);
            }
        }

        public async Task<ArticleComment?> GetArticleCommentAsync(int id)
        {
            using (var context = new AppDbContext())
            {
                return await context.ArticleComment.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public List<ArticleComment> GetArticleComments()
        {
            using (var context = new AppDbContext())
            {
                return context.ArticleComment.ToList();
            }
        }

        public async Task<List<ArticleComment>> GetArticleCommentsAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.ArticleComment.ToListAsync();
            }
        }

        public ResultSet SaveArticle(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            try
            {
                using (var context = new AppDbContext())
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
                using (var context = new AppDbContext())
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
                        context.Add(data);
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
            using (var context = new AppDbContext())
            {
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
            }
            return result;
        }

        public async Task<ResultSet> DeleteArticleCommentAsync(int id)
        {
            ResultSet result = new ResultSet();
            using (var context = new AppDbContext())
            {
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
            }
            return result;
        }

        public async Task<ResultSet> SetConfirmAsync(int id, bool confirm)
        {
            ResultSet result = new ResultSet();
            using (var context = new AppDbContext())
            {
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
            }
            return result;
        }

        public async Task<List<ArticleComment>> GetArticleCommentsAsync(int articleId)
        {
            using (var context = new AppDbContext())
            {
                return await context.ArticleComment.Where(x => x.ArticleId == articleId).ToListAsync();
            }
        }
    }
}
