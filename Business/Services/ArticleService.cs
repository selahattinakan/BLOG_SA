﻿using Constants.Enums;
using Constants;
using DB_EFCore.DataAccessLayer;
using DB_EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ArticleService
    {
        public Article? GetArticle(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Article.FirstOrDefault(x => x.Id == id);
            }
        }

        public async Task<Article?> GetArticleAsync(int id)
        {
            using (var context = new AppDbContext())
            {
                return await context.Article.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public List<Article> GetArticles()
        {
            using (var context = new AppDbContext())
            {
                return context.Article.ToList();
            }
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Article.ToListAsync();
            }
        }

        public ResultSet SaveArticle(Article article)
        {
            ResultSet result = new ResultSet();
            try
            {
                using (var context = new AppDbContext())
                {
                    DbState state = DbState.Update;
                    Article? data = context.Article.FirstOrDefault(x => x.Id == article.Id);
                    if (data == null)
                    {
                        data = new Article();
                        state = DbState.Insert;
                    }
                    data.Title = article.Title;
                    data.Content = article.Content;
                    data.PublishDate = article.PublishDate;
                    data.Enable = article.Enable;

                    if (state == DbState.Update)
                    {
                        data.LastUpdateDate = DateTime.Now;
                        data.UpdateAdminId = Service.GetActiveUserId();
                    }
                    else
                    {
                        data.RegisterDate = DateTime.Now;
                        data.AdminId = Service.GetActiveUserId();
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

        public async Task<ResultSet> SaveArticleAsync(Article article)
        {
            ResultSet result = new ResultSet();
            try
            {
                using (var context = new AppDbContext())
                {
                    DbState state = DbState.Update;// context changetracker'dan da bakılabilir
                    Article? data = await context.Article.FirstOrDefaultAsync(x => x.Id == article.Id);
                    if (data == null)
                    {
                        data = new Article();
                        state = DbState.Insert;
                    }
                    data.Title = article.Title;
                    data.Content = article.Content;
                    data.PublishDate = article.PublishDate;
                    data.Enable = article.Enable;

                    if (state == DbState.Update)
                    {
                        data.LastUpdateDate = DateTime.Now;
                        data.UpdateAdminId = Service.GetActiveUserId();
                    }
                    else
                    {
                        data.RegisterDate = DateTime.Now;
                        data.AdminId = Service.GetActiveUserId();
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

        public ResultSet DeleteArticle(int id)
        {
            ResultSet result = new ResultSet();
            using (var context = new AppDbContext())
            {
                Article? article = context.Article.FirstOrDefault(x => x.Id == id);
                if (article != null)
                {
                    context.Remove(article);
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

        public async Task<ResultSet> DeleteArticleAsync(int id)
        {
            ResultSet result = new ResultSet();
            using (var context = new AppDbContext())
            {
                Article? article = await context.Article.FirstOrDefaultAsync(x => x.Id == id);
                if (article != null)
                {
                    context.Remove(article);
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
    }
}
