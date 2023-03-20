using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    public class NHibernateHelper
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        private static ModelMapper _mapper;
        private static HbmMapping _mappings;

        public ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _configuration = new Configuration();
                    _configuration.Configure();
                    _mapper = new ModelMapper();
                    _mapper.AddMapping<Mappings.ExtendedUserMap>();
                    _mapper.AddMapping<Mappings.OrderMap>();
                    _mapper.AddMapping<Mappings.InvoiceMap>();
                    _mapper.AddMapping<Mappings.UnpaidInvoiceMap>();
                    _mapper.AddMapping<Mappings.UserMap>();
                    _mapper.AddMapping<Mappings.APIUserMap>();
                    _mapper.AddMapping<Mappings.FTPUserMap>();
                    _mapper.AddMapping<Mappings.OrderItemMap>();
                    _mappings = _mapper.CompileMappingForAllExplicitlyAddedEntities();
                    _configuration.AddDeserializedMapping(_mappings, null);
                    _sessionFactory = _configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
