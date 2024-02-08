using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Entity
{
    public class ArticleComment
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string Content { get; set; }
        public string FullName { get; set; }
        public string Mail { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsConfirm { get; set; }
        public int ParentCommentId { get; set; }

        public Article Article { get; set; }

    }
}
