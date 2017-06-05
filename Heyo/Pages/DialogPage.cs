using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heyo.Pages
{
    public class DialogPage : System.Windows.Controls.Page
    {
        public bool ShowBackButton { get; set; } = false;
        public bool ShowCloseButton { get; set; } = true;

        public delegate void ResultEventHandler(DialogPage sender, object result);

        public event ResultEventHandler Result;
        public event EventHandler Closed;
        public new event EventHandler Loaded;
        public void LoadCompleted(object sender,EventArgs e)
        {
            if (Loaded == null)
                return;
            Loaded(sender,e);
        }
        protected void OutputResult(DialogPage sender, object result)
        {
            if(Result!=null)
                Result(sender, result);
        }
        public void Close()
        {
            Closed(this, new EventArgs());
        }
    }
}
