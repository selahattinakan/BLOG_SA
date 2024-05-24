using Business.Interfaces;
using Constants;
using DB_EFCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Decorators
{
    public class BaseArticleDecorator : IArticleIUD
    {
        private readonly IArticleService _articleService;

        public BaseArticleDecorator(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public virtual ResultSet DeleteArticle(int id)
        {
            return _articleService.DeleteArticle(id);
        }

        public virtual async Task<ResultSet> DeleteArticleAsync(int id)
        {
            return await _articleService.DeleteArticleAsync(id);
        }

        public virtual ResultSet SaveArticle(Article article)
        {
            return _articleService.SaveArticle(article);
        }

        public virtual async Task<ResultSet> SaveArticleAsync(Article article)
        {
            return await _articleService.SaveArticleAsync(article);
        }
    }
}
