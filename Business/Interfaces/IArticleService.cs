using Constants.DTOs;
using DB_EFCore.Entity;

namespace Business.Interfaces
{
    public interface IArticleService: IArticleIUD
    {
		public Article? GetArticle(int id);
		public Task<Article?> GetArticleAsync(int id);
		public List<Article> GetArticles();
		public Task<List<Article>> GetArticlesAsync();
        public Task<List<Article>> GetArticlesForRssAsync();
        public Task<Article> GetArticleWithCommentsAsync(int id);
		public Task<List<Article>> GetArticlesWithCommentsAsync();
		public Task<List<ArticleDto>> GetArticlesWithCommentCountsAsync(int page, int pageSize);
		public Task<int> GetArticleCountAsync(bool enabled);
		public List<int> GetArticleIds();
	}
}
