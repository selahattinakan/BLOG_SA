using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string IntroContent { get; set; }
        public DateTime PublishDate { get; set; }
        public bool Enable { get; set; }
        public int PhotoIndex { get; set; }
        public int CommentCounts { get; set; }
        public int ReadMinute { get; set; }
    }
}
