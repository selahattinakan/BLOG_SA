using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Entity
{
    public class Log
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int TableId { get; set; }
        public string Type { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public string Ip { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
