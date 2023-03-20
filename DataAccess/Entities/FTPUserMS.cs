using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace DataAccess.Entities
{
    public class FTPUserMS
    {
        public string FTPUsername { get; set; }
        public DateTime? LastActivity { get; set; }
        public int ActiveMonth { get; set; }
        public int RequestCount { get; set; }
    }
}
