using Heyo.Class.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HeyoCraft.Class.Data
{
    //[DataContract]
    public class Library : Json
    {
        public Minecraft Owner { get; set; }
        //[DataMember(Order = 0, IsRequired = true)]
        public string name;

        //[DataMember(Order = 1, IsRequired = false)]
        public OS natives;

        //[DataMember(Order = 2, IsRequired = false)]
        public extract extract;

        //[DataMember(IsRequired = false)]
        public string url;

        //[DataMember(Order = 4, IsRequired = false)]
        public rules[] rules;

        private const string bmcl = "http://bmclapi2.bangbang93.com/";
        public string AbsolutelyJarPath
        {
            get
            {
                string[] array = name.Split(':');
                array[0] = array[0].Replace(".", "\\");
                string type = Environment.Is64BitOperatingSystem ? "64" : "32";
                string nativeName = natives == null ? "" : "-" + natives.windows.Replace("${arch}", type);
                string directory = string.Format("{0}\\libraries\\{1}\\{2}\\{3}\\{2}-{3}{4}.jar", Owner.PublicDirectory, array[0], array[1], array[2], nativeName);
                return directory;
            }
        }
        public string BmclUri
        {
            get
            {
                string[] array = name.Split(':');
                array[0] = array[0].Replace(".", "\\");
                string type = Environment.Is64BitOperatingSystem ? "64" : "32";
                string nativeName = natives == null ? "" : "-" + natives.windows.Replace("${arch}", type);
                return string.Format("{0}/libraries/{1}/{2}/{3}/{2}-{3}{4}.jar", bmcl, array[0], array[1], array[2], nativeName);
            }
        }
        public string AbsolutelyDirectory
        {
            get {
                string[] array = name.Split(':');
                array[0]= array[0].Replace(".", "\\");
                string type = Environment.Is64BitOperatingSystem ?"64" : "32";
                string nativeName = natives == null ? "" : "-" + natives.windows.Replace("${arch}", type);
                string directory = string.Format("{0}\\libraries\\{1}\\{2}\\{3}\\", Owner.PublicDirectory, array[0],array[1],array[2]);
                return directory;
            }
        }
    }
    //[DataContract]
    public class extract
    {
        //[DataMember(Order = 0, IsRequired = false)]
        public string[] exclude;
    }
    //[DataContract]
    public class OS
    {
        //[DataMember(Order = 0, IsRequired = false)]
        public string windows;

        //[DataMember(Order = 1, IsRequired = false)]
        public string linux;

        //[DataMember(Order = 2, IsRequired = false)]
        public string oxs;
    }
    //[DataContract]
    public class ros
    {
        //[DataMember(Order = 0, IsRequired = true)]
        public string name;

        //[DataMember(Order = 1, IsRequired = false)]
        public string version;
    }
    //[DataContract]
    public class rules
    {
        //[DataMember(Order = 0, IsRequired = true)]
        public string action;
        
        //[DataMember(Order = 1, IsRequired = false)]
        public ros os;

        //[DataMember(Order = 2, IsRequired = false)]
        public string version;
    }
}
