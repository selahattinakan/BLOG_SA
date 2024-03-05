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

namespace Business.Services
{
	public class ArticleService : IArticleService
	{
		private readonly AppDbContext context;
		private readonly IService service;
		public ArticleService(AppDbContext _context, IService _service)
		{
			context = _context;
			service = _service;
		}
		public Article? GetArticle(int id)
		{
			return context.Article.Find(id);
		}

		public async Task<Article?> GetArticleAsync(int id)
		{ // find faster than firstordefault
			return await context.Article.FindAsync(id);
		}

		public List<Article> GetArticles()
		{
			return context.Article.IgnoreQueryFilters().ToList();
		}

		public async Task<List<Article>> GetArticlesAsync()
		{//admin tarafında tüm kayıtları çekiyor
			return await context.Article.IgnoreQueryFilters().ToListAsync();
		}

		public List<Article> GetArticlesNoTracking()
		{// as no tracking ile her bir kayıt için state durumu tutulmuyor(flagler) ve performans artıyor. bu sorgu sonucu 1m kayıt dönseydi 1m flag ramde tutulacaktı
		 // bu kayıtlardan birinde update,insert gibi bir işlem yapılacaksa flagler takip edilmediği için savechanges öncesi manuel işlemler yapılması gerekir
			return context.Article.AsNoTracking().IgnoreQueryFilters().ToList();
		}

		public async Task<Article> GetArticleWithCommentsAsync(int id)
		{
			return await context.Article.Include(x => x.ArticleComments).FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<Article>> GetArticlesWithCommentsAsync()
		{
			return await context.Article.Include(x => x.ArticleComments).ToListAsync();
		}

		public List<int> GetArticleIds()
		{
			var dataTable = context.GetDataTableFromSP($"SP_GetArticleIds");
			return dataTable.Rows.OfType<DataRow>().Select(dr => dr.Field<int>("Id")).ToList();
		}

		public async Task<List<ArticleDto>> GetArticlesWithCommentCountsAsync(int page, int pageSize)
		{
			var articles = await context.Article.Select(x => new ArticleDto
			{
				Id = x.Id,
				PhotoIndex = x.PhotoIndex,
				Title = x.Title,
				PublishDate = x.PublishDate,
				ReadMinute = x.ReadMinute,
				CommentCounts = x.ArticleComments.Count,
				IntroContent = x.IntroContent,
				Enable = x.Enable
			}).OrderByDescending(x => x.PublishDate).ThenByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
			return articles;
		}

		public async Task<int> GetArticleCountAsync(bool enabled)
		{
			return await context.Article.CountAsync();
		}

		public ResultSet SaveArticle(Article article)
		{
			ResultSet result = new ResultSet();
			try
			{
				DbState state = DbState.Update;
				Article? data = context.Article.FirstOrDefault(x => x.Id == article.Id);
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
					data.UpdateAdminId = service.GetActiveUserId();
				}
				else
				{
					data.RegisterDate = DateTime.Now;
					data.AdminId = service.GetActiveUserId();
					context.Add(data);
				}

				int count = context.SaveChanges();
				if (count > 0)
				{
					result.Id = data.Id;
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
				DbState state = DbState.Update;// context changetracker'dan da bakılabilir
				Article? data = await context.Article.FirstOrDefaultAsync(x => x.Id == article.Id);
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
					data.UpdateAdminId = service.GetActiveUserId();
				}
				else
				{
					data.RegisterDate = DateTime.Now;
					data.AdminId = service.GetActiveUserId();
					context.Add(data);
				}

				int count = await context.SaveChangesAsync();
				if (count > 0)
				{
					result.Id = data.Id;
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
			Article? article = context.Article.FirstOrDefault(x => x.Id == id);
			if (article != null)
			{
				context.Remove(article);
				int count = context.SaveChanges();
				if (count <= 0)
				{
					result.Result = Result.Fail;
					result.Message = "Silme işlemi başarısız";
				}
			}
			return result;
		}

		public async Task<ResultSet> DeleteArticleAsync(int id)
		{
			ResultSet result = new ResultSet();
			Article? article = await context.Article.FirstOrDefaultAsync(x => x.Id == id);
			if (article != null)
			{
				context.Remove(article);
				int count = await context.SaveChangesAsync();
				if (count <= 0)
				{
					result.Result = Result.Fail;
					result.Message = "Silme işlemi başarısız";
				}
			}
			return result;
		}
	}
}
