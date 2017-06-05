using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeyoCraft.Class.Data
{
    public class ItemData
    {
        public ItemData()
        {

        }
        /// <summary>
        /// 文件名字
        /// </summary>
        public string Name { get; private set; }
        public string AbsolutelyDirectory { get; private set; }
        private string _absolutelyFilePath;
        public string AbsolutelyFilePath
        {
            get
            {
                return _absolutelyFilePath;
            }
            set
            {
                _absolutelyFilePath = value;
                string jarName = value.Substring(value.LastIndexOf('\\')+1);
                Name = jarName.Substring(0,jarName.IndexOf('.'));
                AbsolutelyDirectory = value.Replace(jarName, "");
            }
        }

        public string FileLocation { get; protected set; }
    }
}
