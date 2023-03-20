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
    public class OrderItemMap :ClassMapping<OrderItem>
    {
        public OrderItemMap()
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
            Property(x => x.StatusId, map => { map.Column("STATUS_ID"); map.NotNullable(true); });
            Property(x => x.Modified, map => map.NotNullable(true));
            Property(x => x.DeliveryDate, map => map.Column("RECEIVED_DATE"));
        }

    }
}
