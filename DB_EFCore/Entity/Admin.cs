using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Entity
{
    public class Admin
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RegisterId { get; set; }
        public DateTime RegisterDate { get; set; }
        public int? UpdateAdminId { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public List<Article> Articles { get; set; }
        public List<Setting> Settings { get; set; }
    }
}
