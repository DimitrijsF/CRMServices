using DataAccess.Entities;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mappings
{
    public class ExtendedUserMap : ClassMapping<ExtendedUser>
    {
        public ExtendedUserMap()
        {
            Table("");
            Schema("");
            Lazy(false);
            Property(x => x.Id, map => { map.Column("ID"); map.NotNullable(true); });
            Property(x => x.Firstname, map => { map.Column("FIRSTNAME"); });
            Property(x => x.Lastname, map => { map.Column("LASTNAME"); });
            Property(x => x.Username, map => { map.Column("USERNAME"); });
            Property(x => x.Isactive, map => { map.Column("ISACTIVE"); });
            Property(x => x.Modified, map => { map.Column("MODIFIED");  });
            Property(x => x.Deleted, map => { map.Column("DELETED"); });
        }
    }
}
