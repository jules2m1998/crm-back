using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Extensions
{
    public static class DictionnaryAdd
    {
        public static Dictionary<string, List<string>> AddOrCreate(
            this Dictionary<string, List<string>> dictionnary, 
            KeyValuePair<string, string> value)
        {
            var val = new List<string>();
            if(dictionnary.TryGetValue(value.Key, out val))
            {
                val.Add(value.Value);
                dictionnary[value.Key] = val;
            } else dictionnary.Add(value.Key, new List<string> { value.Value });
            return dictionnary;
        }
    }
}
