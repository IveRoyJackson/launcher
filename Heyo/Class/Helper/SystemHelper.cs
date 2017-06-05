using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heyo.Class.Helper
{
    public class SystemHelper
    {
        public static bool IsWindow10 { get; private set; }
        static SystemHelper()
        {
            Version v = Environment.OSVersion.Version;
            IsWindow10 = v.Major == 6 && v.Build == 9200;//判断是否为win10
        }
    }
}
