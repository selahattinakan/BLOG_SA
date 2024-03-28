using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Entity
{
    public class Setting
    {
        public int Id { get; set; }
        public bool MaintenanceMode { get; set; }
        public string MaintenanceImgPath { get; set; }
        public string MaintenanceText { get; set; }
        public bool SubscribeMode { get; set; }
        public bool IsCommentEnable { get; set; }
        public bool IsElasticsearchEnable { get; set; }
        public int AdminId { get; set; }
        public DateTime RegisterDate { get; set; }
        public int? UpdateAdminId { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public Admin Admin { get; set; }

    }
}
