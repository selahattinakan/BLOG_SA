using Business.Interfaces;
using Constants;
using Constants.Enums;
using DB_EFCore.Entity;
using DB_EFCore.Repositories.Interfaces;

namespace Business.Services
{
    public class ArticleCommentService : IArticleCommentService
    {
        private readonly IArticleCommentRepository _repository;

        public ArticleCommentService(IArticleCommentRepository repository)
        {
            _repository = repository;
        }

        public ArticleComment? GetArticleComment(int id)
        {
            return _repository.GetArticleComment(id);
        }

        public async Task<ArticleComment?> GetArticleCommentAsync(int id)
        {
            return await _repository.GetArticleCommentAsync(id);
        }

        public List<ArticleComment> GetArticleComments()
        {
            return _repository.GetArticleComments();
        }

        public async Task<List<ArticleComment>> GetArticleCommentsAsync()
        {
            return await _repository.GetArticleCommentsAsync();
        }

        public ResultSet SaveArticleComment(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            try
            {
                DbState state = DbState.Update;
                ArticleComment? data = _repository.GetArticleComment(articleComment.Id);
                if (data == null)
                {
                    data = new ArticleComment();
                    state = DbState.Insert;
                }

                data.ArticleId = articleComment.ArticleId;
                data.Content = articleComment.Content;
                data.FullName = articleComment.FullName;
                data.Mail = articleComment.Mail;
                data.IsConfirm = articleComment.IsConfirm;
                data.ParentCommentId = articleComment.ParentCommentId;

                if (state == DbState.Insert)
                {
                    data.RegisterDate = DateTime.Now;
                    result = _repository.SaveArticleComment(data);
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultSet> SaveArticleCommentAsync(ArticleComment articleComment)
        {
            ResultSet result = new ResultSet();
            try
            {
                if (await _repository.GetCommentsCount() > 1000) //max 1000 olsun, saldırı olursa. ilerde ayarlar içine al
                {
                    throw new Exception("Max kayıt adedi aşıldı");
                }

                DbState state = DbState.Update;// _context changetracker'dan da bakılabilir
                ArticleComment? data = await _repository.GetArticleCommentAsync(articleComment.Id);
                if (data == null)
                {
                    data = new ArticleComment();
                    state = DbState.Insert;
                }
                data.ArticleId = articleComment.ArticleId;
                data.Content = articleComment.Content;
                data.FullName = articleComment.FullName;
                data.Mail = articleComment.Mail;
                data.IsConfirm = articleComment.IsConfirm;
                data.ParentCommentId = articleComment.ParentCommentId;

                if (state == DbState.Insert)
                {
                    data.RegisterDate = DateTime.Now;
                    result = await _repository.SaveArticleCommentAsync(data);
                }
            }
            catch (Exception ex)
            {
                result.Result = Result.Fail;
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultSet DeleteArticleComment(int id)
        {
            ResultSet result = new ResultSet();
            ArticleComment? articleComment = _repository.GetArticleComment(id);
            if (articleComment != null)
            {
                result = _repository.DeleteArticleComment(articleComment);
            }
            return result;
        }

        public async Task<ResultSet> DeleteArticleCommentAsync(int id)
        {
            ResultSet result = new ResultSet();
            ArticleComment? articleComment = await _repository.GetArticleCommentAsync(id);
            if (articleComment != null)
            {
                result = await _repository.DeleteArticleCommentAsync(articleComment);
            }
            return result;
        }

        public async Task<ResultSet> SetConfirmAsync(int id, bool confirm)
        {
            ResultSet result = new ResultSet();
            ArticleComment? articleComment = await _repository.GetArticleCommentAsync(id);
            if (articleComment != null)
            {
                result = await _repository.SetConfirmAsync(articleComment, confirm);
            }
            return result;
        }

        public async Task<List<ArticleComment>> GetArticleCommentsAsync(int articleId)
        {
            return await _repository.GetArticleCommentsAsync(articleId);
        }

        public async Task<int> GetCommentsCount()
        {
            return await _repository.GetCommentsCount();
        }
    }
}
