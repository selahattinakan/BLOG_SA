using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Entity
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string IntroContent { get; set; }
        public DateTime PublishDate { get; set; }
        public bool Enable { get; set; }
        public int AdminId { get; set; }/*RegisterAdminId*/
        public DateTime RegisterDate { get; set; }
        public int? UpdateAdminId { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int PhotoIndex { get; set; }

        public int ReadMinute => IntroContent.Split(' ').Length / 238;

        public Admin Admin { get; set; }
        public List<ArticleComment> ArticleComments { get; set; }

    }
}
