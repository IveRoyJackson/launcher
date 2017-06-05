using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Serialization;
using Heyo.Class.Helper;
using Heyo.Class.Helper.ZIP;
using System.IO;
using Heyo.Class.Data;

namespace HeyoCraft.Class.Data
{
    public class Minecraft : Json , ICloneable
    {
        public string id { get; set; }
        public string time { get; set; }
        public string releaseTime { get; set; }
        public string type { get; set; }
        public string minecraftArguments { get; set; }
        public string minimumLauncherVersion { get; set; }
        public string assets { get; set; }
        public List<Library> libraries { get; set; }
        public string mainClass { get; set; }
        public string inheritsFrom { get; set; }
        public string jar { get; set; }
        public string description { get; set; }
        public string web { get; set; }
        public bool forge { get { return libraries.Exists(o => o.name.ToLower().Contains("forge")); } }

        public string JsonDownloadUri { get; set; }
        public string JarDownloadUri { get; set; }

        /// <summary>
        /// 参考参数格式,{0}最大内存，{1}native库路径,{2}jar库,{3}主类,{4}游戏参数
        /// </summary>
        public string RefArguments { get; }= " -Xincgc -Xmx{0}M  -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true  -Djava.library.path=\"{1}\" -cp \"{2}\" {3} {4}";
        
        public string AbsolutelyJarPath
        {
            get
            {
                string directory = string.Format("{0}\\versions\\{1}\\{1}.jar", PublicDirectory, jar == null ? id : jar);
                return directory;
            }
        }
        private string _absolutelyJsonDirectory;

        /// <summary>
        /// Json所在路径，包括Json
        /// </summary>
        public string AbsolutelyJsonDirectory
        {
            get
            {
                return _absolutelyJsonDirectory;
            }
            set
            {
                //Console.WriteLine(value.ToLower().LastIndexOf("versions"));
                PublicDirectory = value.Substring(0, value.ToLower().LastIndexOf("versions"));
                AbsolutelyDirectory = value.Substring(0, value.LastIndexOf('\\') + 1);
                _absolutelyJsonDirectory = value;
            }
        }
        /// <summary>
        /// 与Jar路径无关，是JSON文件所在位置(不包括Json)
        /// </summary>
        public string AbsolutelyDirectory { get; set; }

        /// <summary>
        /// 所在的.minecraft文件夹
        /// </summary>
        public string PublicDirectory { get; set; }

        public string icon {get;set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
