using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class ExtendedUser
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public short Isactive { get; set; }
        public DateTime? Modified { get; set; }
        public string Deleted { get; set; }
    }
}
