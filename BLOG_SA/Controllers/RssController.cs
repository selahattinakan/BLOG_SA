using BLOG_SA.Models;
using Business.Interfaces;
using DB_EFCore.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace BLOG_SA.Controllers
{
    public class RssController : Controller
    {
        private readonly IArticleService _articleService;

        public RssController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            List<Article> articles = await _articleService.GetArticlesForRssAsync();
            List<RssModel> rssItems = new();
            foreach (var item in articles)
            {
                rssItems.Add(new RssModel
                {
                    Title = item.Title,
                    Description = item.Title,
                    Link = "https://sakan.dev/Home/Article?articleId=" + item.Id
                });
            }

            var rssFeed = new
            {
                Title = "Blog SAkan",
                Description = "Blog SAkan bir developer blogdur.",
                Link = "https://sakan.dev",
                Items = rssItems
            };

            var rssXml = new XmlDocument();
            var rssDeclaration = rssXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            rssXml.AppendChild(rssDeclaration);

            var rssRoot = rssXml.CreateElement("rss");
            rssRoot.SetAttribute("version", "2.0");
            rssXml.AppendChild(rssRoot);

            var channel = rssXml.CreateElement("channel");
            rssRoot.AppendChild(channel);

            var title = rssXml.CreateElement("title");
            title.InnerText = rssFeed.Title;
            channel.AppendChild(title);

            var description = rssXml.CreateElement("description");
            description.InnerText = rssFeed.Description;
            channel.AppendChild(description);

            var link = rssXml.CreateElement("link");
            link.InnerText = rssFeed.Link;
            channel.AppendChild(link);

            foreach (var item in rssFeed.Items)
            {
                var rssItem = rssXml.CreateElement("item");

                var itemTitle = rssXml.CreateElement("title");
                itemTitle.InnerText = item.Title;
                rssItem.AppendChild(itemTitle);

                var itemDescription = rssXml.CreateElement("description");
                itemDescription.InnerText = item.Description;
                rssItem.AppendChild(itemDescription);

                var itemLink = rssXml.CreateElement("link");
                itemLink.InnerText = item.Link;
                rssItem.AppendChild(itemLink);

                channel.AppendChild(rssItem);
            }

            return Content(rssXml.OuterXml, "application/xml");
        }

    }
}
