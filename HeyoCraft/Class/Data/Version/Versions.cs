using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyoCraft.Class.Data.Version
{
    public class Versions
    {
        public latest latest { get; set; }
        public List<Minecraft> versions { get; set; }
    }
    public class latest
    {
        public string snapshot { get; set; }
        public string release { get; set; }
    }
}
