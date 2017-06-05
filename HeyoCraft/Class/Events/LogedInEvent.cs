using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeyoCraft.Class.Events
{
    public delegate void LogedInEventHandler(object sender, LogedInEventArgs logedInEventArgs);
    public class LogedInEventArgs : EventArgs
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public LogedInEventArgs(string id,string name,string avatarUrl)
        {
            ID = id;
            Name = name;
            AvatarUrl = avatarUrl;
        }
    }
}
