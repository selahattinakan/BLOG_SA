using Business.Interfaces;
using Constants;
using Constants.Enums;
using DB_EFCore.Entity;
using Elasticsearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business.Decorators
{
    public class ArticleElasticsearchDecorator : BaseArticleDecorator
    {
        private readonly IElasticsearchService _elasticsearch;
        private readonly ISettingService _settingService;

        public ArticleElasticsearchDecorator(IArticleService articleService, IElasticsearchService elasticsearch, ISettingService settingService) : base(articleService)
        {
            _elasticsearch = elasticsearch;
            _settingService = settingService;
        }

        public override ResultSet DeleteArticle(int id)
        {
            ResultSet result = base.DeleteArticle(id);
            if (result.Result == Result.Success)
            {
                var setting = _settingService.GetSetting();
                if (setting.IsElasticsearchEnable)
                {
                    var resultElastic = _elasticsearch.Delete(id);
                    if (!resultElastic) throw new Exception("Elasticsearch silme işlemi başarısız");
                }
            }

            return result;
        }

        public override async Task<ResultSet> DeleteArticleAsync(int id)
        {
            ResultSet result = await base.DeleteArticleAsync(id);
            if (result.Result == Result.Success)
            {
                var setting = await _settingService.GetSettingAsync();
                if (setting.IsElasticsearchEnable)
                {
                    var resultElastic = await _elasticsearch.DeleteAsync(id);
                    if (!resultElastic) throw new Exception("Elasticsearch silme işlemi başarısız");
                }
            }
            return result;
        }

        public override ResultSet SaveArticle(Article article)
        {
            ResultSet result = base.SaveArticle(article);
            if (result.Result == Result.Success)
            {
                var setting = _settingService.GetSetting();
                if (setting.IsElasticsearchEnable)
                {
                    var state = (DbState)result.TempData;
                    var data = (Article)result.Object;

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
            return result;
        }

        public override async Task<ResultSet> SaveArticleAsync(Article article)
        {
            ResultSet result = await base.SaveArticleAsync(article);
            if (result.Result == Result.Success)
            {
                var setting = await _settingService.GetSettingAsync();
                if (setting.IsElasticsearchEnable)
                {
                    var state = (DbState)result.TempData;
                    var data = (Article)result.Object;

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
            return result;
        }
    }
}
