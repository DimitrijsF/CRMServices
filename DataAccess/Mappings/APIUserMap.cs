using DataAccess.Entities;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mappings
{
    public class APIUserMap : ClassMapping<APIUser>
    {
        public APIUserMap()
        {
            Table("");
            Schema("");
            Lazy(false);
            Property(x => x.Id, map => { map.Column("ID");});
            Property(x => x.ApiUsername, map => { map.Column("APIUSER");  });
            Property(x => x.LastRequested, map => { map.Column("LAST_REQUEST"); });
            Property(x => x.Created, map => { map.Column("CREATED"); });
            Property(x => x.Modified, map => { map.Column("MODIFIED");  });
            Property(x => x.Active, map => { map.Column("ACTIVE");  });
            Property(x => x.RequestCount, map => { map.Column("REQUESTS");  });
            Property(x => x.ActiveWeeks, map => { map.Column("ACTIVE_WEEKS");  });
            Property(x => x.Username, map => { map.Column("USERNAME");  });
            Property(x => x.Email, map => { map.Column("EMAIL");  });
        }
    }
}
