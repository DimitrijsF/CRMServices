using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Invoice
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string InvoiceNumber { get; set; }
        public string CrmOpportunityOwner { get; set; }
        public double? Price { get; set; }
    }
}
