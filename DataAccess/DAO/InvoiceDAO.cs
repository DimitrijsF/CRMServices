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
    public class InvoiceDAO
    {
        private NHibernateHelper helper = new NHibernateHelper();
        public List<Invoice> GetByOrderID(long input)
        {
            using (ISession session  = helper.OpenSession())
            {
                return session.QueryOver<Invoice>().Where(x => x.OrderId == input).List().ToList();
            }
        }
        public void Update(Invoice input)
        {
            using (ISession session = helper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(input);
                transaction.Commit();
            }
        }
        public Invoice GetById(long id)
        {
            using (ISession session = helper.OpenSession())
            {
                return session.Load<Invoice>(id);
            }
        }
    }
}
