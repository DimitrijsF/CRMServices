using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using static Common.CommonObjects;
using DataAccess.DAO;

namespace DataAccess.Helpers
{
    public class CRMHelper
    {
        public ActionResult CheckEntityExists(CRMInput input, CRMDAO dao)
        {
            ActionResult result = new ActionResult();
            try
            {
                Entity entity = dao.GetObjectsByValue(input).FirstOrDefault();
                if (entity != null)
                {
                    result.Result = true;
                    result.Object = entity;
                }
                else
                {
                    result.Result = false;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
