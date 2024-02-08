using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Entity
{
    public class Contact
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsAnswered { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
