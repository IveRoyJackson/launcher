using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeyoCraft.Class.Data
{
    /// <summary>
    /// Asset数据模型
    /// </summary>
    public class Asset
    {
        public string name { get; set; }
        public string hash { get; set; }
        public int size { get; set; }
    }
}
