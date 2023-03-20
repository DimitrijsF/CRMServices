using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class APIUser
    {
        public long Id { get; set; }
        public string ApiUsername { get; set; }
        public DateTime? LastRequested { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public int Active { get; set; }
        public int ActiveWeeks { get; set; }
        public int RequestCount { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
