using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using NHibernate;
using DataAccess.Helpers;
using System.Data.Linq;

namespace DataAccess.DAO
{
    public class FTPUserDAO
    {
        private const string ConnRemote = @"";
        private const string ConnLocal = @"";
        private NHibernateHelper helper = new NHibernateHelper();
        public List<FTPUser> GetDataFromOracle()
        {            
            using (ISession session = helper.OpenSession())
            {
                return session.QueryOver<FTPUser>().List().ToList();
            }
        }
        public List<FTPUserMS> GetDataFromMs()
        {

            DataContext context = null;
            if(Environment.MachineName == "")
            {
                context = new DataContext(new SqlConnection(ConnLocal));
            }
            else
            {
                context = new DataContext(new SqlConnection(ConnRemote));
            }
            string select = @"";
            return context.ExecuteQuery<FTPUserMS>(select, DateTime.Now.Date.AddMonths(-6).AddDays(-1).ToString("yyyy-MM-dd")).Where(x=> x.FTPUsername.Length > 4).ToList();
        }
        public FTPUser GetByUsername(string input)
        {
            using(ISession session = helper.OpenSession())
            {
                return session.QueryOver<FTPUser>().Where(x => x.Username == input).List().FirstOrDefault();
            }
        }
    }
}
