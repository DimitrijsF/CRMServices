using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoggerForServices;
using static Common.CommonObjects;

namespace Common
{
    public static  class Data
    {
        public static Logger Logger { get; set; }
        public static Logger DebugLogger { get; set; }
        public static bool Debug { get; set; } = false;
        public static bool GetAll { get; set; } = false;
        public static List<LockedOrder> LockedOrders { get; set; }
        public static Socket Listener { get; set; } = null;
        public static bool StopHost { get; set; } = false;
        public static List<UpdateAttribute> ContactFields { get; set; } = new List<UpdateAttribute>()
        {
            new UpdateAttribute(){ Attribute = "firstname", UserField = "Firstname" },
            new UpdateAttribute(){ Attribute = "lastname", UserField = "Lastname" },
            new UpdateAttribute(){ Attribute = "new_web_registration_date", UserField = "Created" },
            new UpdateAttribute(){ Attribute = "sh_lastwebactivitydate", UserField = "Lastlogin" },
            new UpdateAttribute(){ Attribute = "sh_lastlistsearchusedate", UserField = "LSUsed" },
            new UpdateAttribute(){ Attribute = "new_listsearchitemacountinlast3months", UserField = "ItemsCount" },
            new UpdateAttribute(){ Attribute = "new_lastshoppingcartcreationdate", UserField = "ShoppingCartDate" },
            new UpdateAttribute(){ Attribute = "new_lastshoppingcartpaiddat", UserField = "PayDate" },
            new UpdateAttribute(){ Attribute = "sh_listsearchusecountinlast3months", UserField = "RequestCount" },
        };
    }
}
