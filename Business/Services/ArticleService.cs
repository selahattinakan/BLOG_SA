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
using Business.Interfaces;
using Business.DTOs;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Elasticsearch.Models;

namespace Business.Services
{
    //db işlemleri reposityor altına alınacak, reposityr dönüşlerine göre elastic search eklenecek, yani elastic ile main db işlemleri tamamen ayrı katmanlarda olup aynı katman altında sıralı işlem yapacak
    public class ArticleService : IArticleService
    {
        private readonly AppDbContext _context;
        private readonly IService _service;
        private readonly IElasticsearch _elasticsearch;
        private readonly ISettingService _settingService;
        public ArticleService(AppDbContext context, IService service, IElasticsearch elasticsearch, ISettingService settingService)
        {
            _context = context;
            _service = service;
            _elasticsearch = elasticsearch;
            _settingService = settingService;
        }
        public Article? GetArticle(int id)
        {
            return _context.Article.Find(id);
        }

        public async Task<Article?> GetArticleAsync(int id)
        { // find faster than firstordefault
            return await _context.Article.FindAsync(id);
        }

        public List<Article> GetArticles()
        {
            return _context.Article.IgnoreQueryFilters().ToList();
        }

        public async Task<List<Article>> GetArticlesAsync()
        {//admin tarafında tüm kayıtları çekiyor
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
                DbState state = DbState.Update;
                Article? data = _context.Article.IgnoreQueryFilters().FirstOrDefault(x => x.Id == article.Id);
                if (data == null)
                {
                    data = new Article();
                    state = DbState.Insert;
                }
                data.Title = article.Title;
                data.Content = article.Content;
                data.IntroContent = article.IntroContent;
                data.PublishDate = article.PublishDate;
                data.Enable = article.Enable;
                data.PhotoIndex = article.PhotoIndex;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = _service.GetActiveUserId();
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = _service.GetActiveUserId();
                    _context.Add(data);
                }

                int count = _context.SaveChanges();
                if (count > 0)
                {
                    result.Id = data.Id;
                    var setting = _settingService.GetSetting();
                    if (setting.IsElasticsearchEnable)
                    {
                        if (state == DbState.Insert)
                        {
                            ES_Article es_article = new ES_Article()
                            {
                                Content = data.IntroContent,
                                Title = data.Title,
                                Tags = string.Empty
                            };
                            var resultElastic = _elasticsearch.Save(es_article, data.Id);
                            if (!resultElastic) throw new Exception("Elasticsearch kayıt işlemi başarısız");
                        }
                        else
                        {
                            ES_Article es_article = new ES_Article()
                            {
                                Content = data.IntroContent,
                                Title = data.Title,
                                Tags = string.Empty
                            };
                            var resultElastic = _elasticsearch.Update(es_article, data.Id);
                            if (!resultElastic) throw new Exception("Elasticsearch güncelleme işlemi başarısız");
                        } 
                    }
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
                DbState state = DbState.Update;// _context changetracker'dan da bakılabilir
                Article? data = await _context.Article.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == article.Id);
                if (data == null)
                {
                    data = new Article();
                    state = DbState.Insert;
                }
                data.Title = article.Title;
                data.Content = article.Content;
                data.IntroContent = article.IntroContent;
                data.PublishDate = article.PublishDate;
                data.Enable = article.Enable;
                data.PhotoIndex = article.PhotoIndex;

                if (state == DbState.Update)
                {
                    data.LastUpdateDate = DateTime.Now;
                    data.UpdateAdminId = _service.GetActiveUserId();
                }
                else
                {
                    data.RegisterDate = DateTime.Now;
                    data.AdminId = _service.GetActiveUserId();
                    await _context.AddAsync(data);
                }

                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    result.Id = data.Id;
                    var setting = await _settingService.GetSettingAsync();
                    if (setting.IsElasticsearchEnable)
                    {
                        if (state == DbState.Insert)
                        {
                            ES_Article es_article = new ES_Article()
                            {
                                Content = data.IntroContent,
                                Title = data.Title,
                                Tags = string.Empty
                            };
                            var resultElastic = await _elasticsearch.SaveAsync(es_article, data.Id);
                            if (!resultElastic) throw new Exception("Elasticsearch kayıt işlemi başarısız");
                        }
                        else
                        {
                            ES_Article es_article = new ES_Article()
                            {
                                Content = data.IntroContent,
                                Title = data.Title,
                                Tags = string.Empty
                            };
                            var resultElastic = await _elasticsearch.UpdateAsync(es_article, data.Id);
                            if (!resultElastic) throw new Exception("Elasticsearch güncelleme işlemi başarısız");
                        } 
                    }
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

        public ResultSet DeleteArticle(int id)
        {
            ResultSet result = new ResultSet();
            Article? article = _context.Article.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);
            if (article != null)
            {
                _context.Remove(article);
                int count = _context.SaveChanges();
                if (count <= 0)
                {
                    result.Result = Result.Fail;
                    result.Message = "Silme işlemi başarısız";
                }
                else
                {
                    var setting = _settingService.GetSetting();
                    if (setting.IsElasticsearchEnable)
                    {
                        var resultElastic = _elasticsearch.Delete(id);
                        if (!resultElastic) throw new Exception("Elasticsearch silme işlemi başarısız"); 
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Yorumlarla birlikte makaleyi siler.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultSet> DeleteArticleAsync(int id)
        {
            ResultSet result = new ResultSet();
            Article? article = await _context.Article.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (article != null)
            {
                _context.Remove(article);
                int count = await _context.SaveChangesAsync();
                if (count <= 0)
                {
                    result.Result = Result.Fail;
                    result.Message = "Silme işlemi başarısız";
                }
                else
                {
                    var setting = await _settingService.GetSettingAsync();
                    if (setting.IsElasticsearchEnable)
                    {
                        var resultElastic = await _elasticsearch.DeleteAsync(id);
                        if (!resultElastic) throw new Exception("Elasticsearch silme işlemi başarısız"); 
                    }
                }
            }
            return result;
        }
    }
}
