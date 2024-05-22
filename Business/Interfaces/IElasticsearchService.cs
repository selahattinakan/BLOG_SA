using DB_EFCore.Entity;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IElasticsearchService
    {
        public bool Save(ES_Article article, int articleId);
        public bool Update(ES_Article article, int articleId);
        public bool Delete(int articleId);
        public Task<bool> SaveAsync(ES_Article article, int articleId);
        public Task<bool> UpdateAsync(ES_Article article, int articleId);
        public Task<bool> DeleteAsync(int articleId);
        public Task<(List<ES_Article> list, long totalCount)> SearchAsync(string searchText, int page, int pageSize);
    }
}
