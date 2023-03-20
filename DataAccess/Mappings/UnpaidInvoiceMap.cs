using DataAccess.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mappings
{
    public class UnpaidInvoiceMap : ClassMapping<UnpaidInvoice>
    {
        public UnpaidInvoiceMap()
        {
            Table("");
            Schema("");
            Lazy(false);

            Id(x => x.Id, map =>
            {
                map.Column("ID");
                map.Generator(Generators.Native);
            });

            Property(x => x.BillingOrganizationId, map => { map.Column("billing_org_id"); });
            Property(x => x.InvoiceDueDate, map => { map.Column("INVOICE_DUE_DATE"); });
            Property(x => x.BillingOrganizationName, map => { map.Column("billing_name"); });
            Property(x => x.BillingCode, map => { map.Column("billing_code"); });
        }
    }
}
