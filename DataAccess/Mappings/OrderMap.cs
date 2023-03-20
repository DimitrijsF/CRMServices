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
    public class OrderMap : ClassMapping<Order>
    {
        public OrderMap()
        {
            Table("");
            Schema("");
            Lazy(false);
            Id(x => x.Id, map =>
            {
                map.Column("ID");
                map.Generator(Generators.Sequence, g => g.Params(new { sequence = "" }));
            });
            Property(x => x.MolportOrderNumber, map => { map.Column("MOLPORT_ORDER_NUMBER"); map.NotNullable(true); });
            Property(x => x.AccountManagerId, map => map.Column("ACCOUNT_MANAGER_ID"));
            Property(x => x.Created, map => map.NotNullable(true));
            Property(x => x.IsInCRM, map => { map.Column("is_crm_purchase_order"); });
            Property(x => x.CRMOrderOwnerId, map => { map.Column("CRM_ORDER_OWNER_ID"); });
            Property(x => x.OrderDate, map => { map.Column("ORDER_DATE"); map.NotNullable(true); });
        }
    }
}
