using DataAccess.Entities;
using DataAccess.Helpers;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class APIUserDAO
    {
        private NHibernateHelper helper = new NHibernateHelper();
        public List<APIUser> GetListFor1HUpdate()
        {
            using (ISession session = helper.OpenSession())
            {
                DateTime weeks12 = DateTime.Now.AddDays(-85);
                DateTime hours = DateTime.Now.AddHours(-3.5);
                return session.QueryOver<APIUser>().Where(x => x.Created >= hours || x.Modified >= hours || x.LastRequested >= weeks12).List().ToList();
            }
        }
        public List<APIUser> GetAll()
        {
            using (ISession session = helper.OpenSession())
            {
                DateTime yearStart = new DateTime(2022, 1, 1);
                return session.QueryOver<APIUser>().Where(x => x.Modified >= yearStart || x.Created >= yearStart || x.LastRequested >= yearStart).List().ToList();
            }
        }
        public APIUser GetByName(string input)
        {
            using(ISession session = helper.OpenSession())
            {
                return session.QueryOver<APIUser>().Where(x => x.ApiUsername == input).List().ToList().FirstOrDefault();
            }
        }
    }
}
