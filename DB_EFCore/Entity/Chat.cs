using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Entity
{
    public class Chat
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string NickName { get; set; }
        public string Message { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
