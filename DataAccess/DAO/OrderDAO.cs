using DataAccess.Entities;
using DataAccess.Helpers;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Common.CommonObjects;
using LoggerForServices;
using Common;

namespace DataAccess.DAO
{
    public class OrderDAO
    {
        private NHibernateHelper helper = new NHibernateHelper();
        public List<Order> GetOrdersNotInCRM()
        {
            using(ISession session = helper.OpenSession())
            {
                return session.QueryOver<Order>().Where(x=> (x.IsInCRM == null || x.IsInCRM == 0) && (x.Created >= DateTime.Now.AddDays(-7))).List().ToList();
            }
        }
        public Order GetByName(string input)
        {
            using (ISession session = helper.OpenSession())
            {
                return session.QueryOver<Order>().Where(x => x.MolportOrderNumber == input).List().FirstOrDefault();
            }
        }
        public void Update(Order input)
        {
            using (ISession session = helper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(input);
                transaction.Commit();
            }
        }
        public Order GetById(long id)
        {
            using(ISession session = helper.OpenSession())
            {
                return session.Load<Order>(id);
            }
        }
        public List<Order> GetByCreatedDate(DateTime from, DateTime till)
        {
            using (ISession session = helper.OpenSession())
            {
                return session.QueryOver<Order>().Where(x => x.Created >= from && x.Created <= till).List().ToList();
            }
        }
        public string GetNameById(long id)
        {
            using(ISession session = helper.OpenSession())
            {
                return session.QueryOver<Order>().Where(x => x.Id == id).List().ToList().Select(x => x.MolportOrderNumber).FirstOrDefault();
            }
        }
    }
}
