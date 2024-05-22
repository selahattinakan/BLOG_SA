using Business.Interfaces;
using DB_EFCore.Entity;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.Models;
using Elasticsearch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly ArticleRepository _articleRepository;

        public ElasticsearchService(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<bool> SaveAsync(ES_Article article, int articleId)
        {
            return await _articleRepository.SaveAsync(article, articleId);
        }

        public async Task<bool> UpdateAsync(ES_Article article, int articleId)
        {
            return await _articleRepository.UpdateAsync(article, articleId);
        }
        public async Task<bool> DeleteAsync(int articleId)
        {
            return await _articleRepository.DeleteAsync(articleId);
        }

        public async Task<(List<ES_Article> list, long totalCount)> SearchAsync(string searchText, int page, int pageSize)
        {
            return await _articleRepository.SearchAsync(searchText, page, pageSize);
        }

        public bool Save(ES_Article article, int articleId)
        {
            return _articleRepository.Save(article, articleId);
        }

        public bool Update(ES_Article article, int articleId)
        {
            return _articleRepository.Update(article, articleId);
        }

        public bool Delete(int articleId)
        {
            return _articleRepository.Delete(articleId);
        }
    }
}
