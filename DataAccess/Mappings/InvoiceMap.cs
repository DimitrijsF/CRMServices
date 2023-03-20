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
    public class InvoiceMap : ClassMapping<Invoice>
    {

        public InvoiceMap()
        {
            Table("");
            Schema("");
            Lazy(false);
            Id(x => x.Id, map =>
            {
                map.Column("ID");
                map.Generator(Generators.Sequence, g => g.Params(new { sequence = "" }));
            });
            Property(x => x.OrderId, map => { map.Column("ORDER_ID"); map.NotNullable(true); });
            Property(x => x.InvoiceNumber, map => { map.Column("INVOICE_NUMBER"); map.NotNullable(true); });
            Property(x => x.CrmOpportunityOwner, map => { map.Column("crm_opportunity_owner"); });
            Property(x => x.Price);
        }
    }
}
