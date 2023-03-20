using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class DateTimeHelper
    {
        public static int GetUntilMidnight()
        {
            return (int)(DateTime.Now.AddDays(1).Date - DateTime.Now).TotalMilliseconds;
        }
        public static int GetUntilNextHour()
        {
            return (int)(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1) - DateTime.Now).TotalMilliseconds;
        }
        public static int GetUntilNextHalfHour()
        {
            if (DateTime.Now.Minute < 30)
            {
                return (int)(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 0.5) - DateTime.Now).TotalMilliseconds;
            }
            else
            {
                return (int)(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1.5) - DateTime.Now).TotalMilliseconds;
            }
        }
        public static int GetUntilNextHourQuater()
        {
            if (DateTime.Now.Minute < 15)
            {
                return (int)(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 0.25) - DateTime.Now).TotalMilliseconds;
            }
            else
            {
                return (int)(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1.25) - DateTime.Now).TotalMilliseconds;
            }
        }
        public static int GetNext12H()
        {
            if(DateTime.Now.Hour < 12)
            {
                return (int)(DateTime.Now.Date.AddHours(12) - DateTime.Now).TotalMilliseconds;
            }
            else
            {
                return GetUntilMidnight();
            }
        }
    }
}
