using Business.Interfaces;
using Elasticsearch.Models;
using Elasticsearch.Repositories;

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
