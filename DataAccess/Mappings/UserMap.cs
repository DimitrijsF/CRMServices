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
    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Schema("");
            Table("");
            Lazy(false);
            Id(x => x.Id, map =>
            {
                map.Column("ID");
                map.Generator(Generators.Sequence, g => g.Params(new { sequence = "" }));
            });
            Property(x => x.Username);
        }
    }
}
