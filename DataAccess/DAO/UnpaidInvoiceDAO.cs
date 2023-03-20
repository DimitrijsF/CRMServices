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
    public class UnpaidInvoiceDAO
    {
        private NHibernateHelper helper = new NHibernateHelper();
        public List<UnpaidInvoice> GetList()
        {
            using(ISession session = helper.OpenSession())
            {
                return session.QueryOver<UnpaidInvoice>().List().ToList();
            }
        }
    }
}
