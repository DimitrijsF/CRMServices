using DataAccess.Entities;
using DataAccess.Helpers;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderItemDAO
    {
        private NHibernateHelper helper = new NHibernateHelper();
        public List<long> GetByModifiedDate(DateTime input)
        {
            using (ISession session = helper.OpenSession())
            {
                return session.QueryOver<OrderItem>().Where(x => x.Modified >= input).List().Select(x => x.OrderId).Distinct().ToList();
            }
        }
        public DateTime? MaxDeliveryDateIfAllDelivered(long orderID)
        {
            using(ISession session = helper.OpenSession())
            { 
                var data = session.QueryOver<OrderItem>().Where(x => x.OrderId == orderID).List()
                    .Select(x => new { x.StatusId, x.DeliveryDate });
                if(data.Where(x=> !OrderItem.CanceledStatuses.Contains(x.StatusId)).All(x=> x.StatusId == OrderItem.C_STATUS_DELIVERED_TO_CUSTOMER))
                {
                    return data.Max(x => x.DeliveryDate);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
