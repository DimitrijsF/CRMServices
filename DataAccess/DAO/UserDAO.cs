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
    public class UserDAO
    {
        private NHibernateHelper helper = new NHibernateHelper();
        public User GetByUserName(string input)
        {
            using (ISession session = helper.OpenSession())
            {
                return session.QueryOver<User>().Where(x => x.Username == input).List().FirstOrDefault();
            }
        }
        public User GetById(long id)
        {
            using(ISession session = helper.OpenSession())
            {
                return session.Load<User>(id);
            }
        }
    }
}
