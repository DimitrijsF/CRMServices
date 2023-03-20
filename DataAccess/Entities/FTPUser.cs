using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class FTPUser : FTPUserMS
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public DateTime? Modified { get; set; }
        public int Active { get; set; }
        public string Email { get; set; }

        public void MergeWithMS(FTPUserMS input)
        {
            FTPUsername = input.FTPUsername;
            LastActivity = input.LastActivity;
            RequestCount = input.RequestCount;
            ActiveMonth = input.ActiveMonth;
        }

    }
}
