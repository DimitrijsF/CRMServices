using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class UnpaidInvoice
    {
        public long Id { get; set; }
        public long? BillingOrganizationId { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
        public string BillingOrganizationName { get; set; }
        public int BillingCode { get; set; }
    }
}
