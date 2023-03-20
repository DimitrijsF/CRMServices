using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    public static class EntityHelper
    {
        public static void UpdateValue(this Entity input, string name, object value)
        {
            if (input.Attributes.ContainsKey(name))
            {
                input.Attributes[name] = value;
            }
            else
            {
                input.Attributes.Add(new KeyValuePair<string, object>(name, value));
            }
        }
    }
}
