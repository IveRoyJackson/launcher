using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heyo.Class.Events
{
    public class StringEventArgs : EventArgs
    {
        public StringEventArgs(string source)
        {
            Source = source;
        }
        public string Source { get; set; }
    }
}
