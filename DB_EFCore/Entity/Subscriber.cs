using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.Entity
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
