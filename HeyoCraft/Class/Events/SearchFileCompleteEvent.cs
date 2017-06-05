using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeyoCraft.Class.Events
{
    public delegate void SearchFileCompleteEventHandler(object sender, SearchFileCompleteEventArgs e);
    public class SearchFileCompleteEventArgs : EventArgs
    {
        public List<string> Paths { get; set; } = new List<string>();
        public SearchFileCompleteEventArgs(List<string> paths)
        {
            Paths = paths;
        }
    }
}
