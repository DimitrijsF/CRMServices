using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DataAccess.Helpers
{
    public static class ExtensionHelper
    {
        public static void AddOrSet(this AttributeCollection attributes, string value, object obj)
        {
            if (attributes.ContainsKey(value))
            {
                attributes[value] = obj;
            }
            else
            {
                attributes.Add(value, obj);
            }
        }
    }
}
