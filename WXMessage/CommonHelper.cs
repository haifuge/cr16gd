using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace WXMessage
{
    public class CommonHelper
    {
        public static XDocument ParseJsonToXML(string json, string rootName)
        {
            return JsonConvert.DeserializeXNode(json, rootName);
        }
    }
}
