using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class OrderItem
    {
        #region Statuses
        public const long C_STATUS_CANCELLED = 181;
        public const long C_STATUS_CANCELLED_AMOUNT = 341;
        public const long C_STATUS_CANCELLED_REPLACED = 381;       
        public const long C_STATUS_DELIVERED_TO_CUSTOMER = 402;
        #endregion
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long StatusId { get; set; }
        public DateTime Modified { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public static List<long> CanceledStatuses { get; set; } = new List<long>()
        {
            C_STATUS_CANCELLED, C_STATUS_CANCELLED_AMOUNT,C_STATUS_CANCELLED_REPLACED
        };
    }
}
