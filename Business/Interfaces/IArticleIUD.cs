using Constants;
using DB_EFCore.Entity;

namespace Business.Interfaces
{
    public interface IArticleIUD
    {
        public ResultSet SaveArticle(Article article);
        public Task<ResultSet> SaveArticleAsync(Article article);
        public ResultSet DeleteArticle(int id);
        public Task<ResultSet> DeleteArticleAsync(int id);
    }
}
