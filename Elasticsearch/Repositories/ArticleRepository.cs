using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Repositories
{
    public class ArticleRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "blog";

        public ArticleRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public bool Save(ES_Article article, int articleId)
        {
            article.Created = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            var response = _client.Index(article, x => x.Index(indexName).Id(articleId));

            return response.IsValidResponse;

        }

        public bool Update(ES_Article article, int articleId)
        {
            article.Updated = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            var response = _client.Update<ES_Article, ES_Article>(indexName, articleId, x => x.Doc(article));

            return response.IsValidResponse;
        }

        public bool Delete(int articleId)
        {
            var response = _client.Delete<ES_Article>(articleId, x => x.Index(indexName));

            return response.IsValidResponse;
        }

        public async Task<bool> SaveAsync(ES_Article article, int articleId)
        {
            article.Created = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            var response = await _client.IndexAsync(article, x => x.Index(indexName).Id(articleId));

            return response.IsValidResponse;

        }

        public async Task<bool> UpdateAsync(ES_Article article, int articleId)
        {
            article.Updated = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            var response = await _client.UpdateAsync<ES_Article, ES_Article>(indexName, articleId, x => x.Doc(article));

            return response.IsValidResponse;
        }

        public async Task<bool> DeleteAsync(int articleId)
        {
            var response = await _client.DeleteAsync<ES_Article>(articleId, x => x.Index(indexName));

            return response.IsValidResponse;
        }

        public async Task<(List<ES_Article> list, long totalCount)> SearchAsync(string searchText, int page, int pageSize)
        {
            List<Action<QueryDescriptor<ES_Article>>> ListQuery = new();
            Action<QueryDescriptor<ES_Article>> matchAll = (q) => q.MatchAll();
            Action<QueryDescriptor<ES_Article>> matchContent = (q) => q.Match(m => m
                .Field(f => f.Content)
                .Fuzziness(new Fuzziness(1))
                .Query(searchText));
            Action<QueryDescriptor<ES_Article>> contentMatchBoolPrefix = (q) => q.MatchBoolPrefix(m => m
                .Field(f => f.Content)
                .Fuzziness(new Fuzziness(1))
                .Query(searchText));
            Action<QueryDescriptor<ES_Article>> matchTitle = (q) => q.Match(m => m
                .Field(f => f.Title)
                .Fuzziness(new Fuzziness(1))
                .Query(searchText));
            Action<QueryDescriptor<ES_Article>> titleMatchBoolPrefix = (q) => q.MatchBoolPrefix(m => m
                .Field(f => f.Title)
                .Fuzziness(new Fuzziness(1))
                .Query(searchText));

            if (string.IsNullOrEmpty(searchText))
            {
                ListQuery.Add(matchAll);
            }
            else
            {
                ListQuery.Add(matchContent);
                ListQuery.Add(contentMatchBoolPrefix);
                ListQuery.Add(matchTitle);
                ListQuery.Add(titleMatchBoolPrefix);
            }

            var pageFrom = (page - 1) * pageSize;

            var result = await _client.SearchAsync<ES_Article>(s => s.Index(indexName)
            .Size(pageSize).From(pageFrom)
                .Query(q => q
                    .Bool(b => b
                        .Should(ListQuery.ToArray()))));

            if (!result.IsValidResponse) return new(new List<ES_Article>(), 0);

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return (list: result.Documents.ToList(), result.Total);
        }
    }
}
