using Heyo.Class.Data;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace HeyoCraft.Class.Data.Yggdrasil
{
    [DataContract]
	public class Agent : Json
    {
        public static Agent MINECRAFT = new Agent("Minecraft", "1");

        //public static Agent SCROLLS = new Agent("Scrolls", 1);
        public Agent(string name, string version)
        {
            this.name = name;
            this.version = version;
        }
        [DataMember]
        public string name {get; set; }
        [DataMember]
        public string version { get; set; }
	}
}
