using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heyo.Class.Helper;
using System.Runtime.Serialization;

namespace Heyo.Class.Data
{
    [DataContract]
    public class Json
    {
        public new string ToString()
        {
            return JsonHelper.JsonSerializerBySingleData(this);
        }
    }
}
