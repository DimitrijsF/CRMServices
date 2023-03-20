using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public long? AccountManagerId { get; set; }
        public string MolportOrderNumber { get; set; }
        public DateTime Created { get; set; }
        public long? CRMOrderOwnerId { get; set; }
        public DateTime OrderDate { get; set; }
        public short? IsInCRM { get; set; }
    }
}
