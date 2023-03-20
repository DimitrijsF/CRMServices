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
    public class FTPUserMap : ClassMapping<FTPUser>
    {
        public FTPUserMap()
        {
            Table("");
            Schema("");
            Lazy(false);
            Id(x => x.Id, map =>
            {
                map.Column("ID");
                map.Generator(Generators.Sequence, g => g.Params(new { sequence = "" }));
            });
            Property(x => x.Username, map => { map.Column("USERNAME"); map.NotNullable(true); });
            Property(x => x.Active, map => { map.Column("ACTIVE"); map.NotNullable(true); });
            Property(x => x.Modified, map => { map.Column("MODIFIED"); map.NotNullable(true); });
            Property(x => x.Email, map => { map.Column("EMAIL"); map.NotNullable(true); });
            
        }
    }
}
