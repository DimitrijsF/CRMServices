using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Helpers;
using static Common.CommonObjects;
using NHibernate;
using NHibernate.Transform;

namespace DataAccess.DAO
{
    public class ExtendedUserDAO
    {
        private NHibernateHelper helper = new NHibernateHelper();
        public List<ExtendedUser> GetListByDates(DateTime input)
        {
            using (ISession session = helper.OpenSession())
            {
                return session.CreateSQLQuery("")
                .SetResultTransformer(
                     Transformers.AliasToBean(typeof(ExtendedUser)))
                .List<ExtendedUser>().ToList();
            }
        }
        public List<ExtendedUser> GetByUsername(string input)
        {
            using(ISession session = helper.OpenSession())
            {
                return session.CreateSQLQuery("")
                .SetResultTransformer(
                     Transformers.AliasToBean(typeof(ExtendedUser)))
                .List<ExtendedUser>().ToList();
            }
        }
    }
}
