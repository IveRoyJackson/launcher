using HeyoCraft.Class;
using HeyoCraft.Class.Helper.Login;
using HeyoCraft.Reference;
using System.ComponentModel;
using System.Net;

namespace HeyoCraft.BackgroundWorkers
{
    public class YggdrasilWorker : BackgroundWorker
    {
        public delegate void CompletedEventHandler(YggdrasilWorker sender, ExchangeCode resultExchangeCode, User user);
        protected CompletedEventHandler completedEventHandler;

        private string ID, PW;
        private User user;
        /// <summary>
        /// 登录MC正版账号
        /// </summary>
        /// <param name="completedEventHandler"></param>
        /// <param name="ID"></param>
        /// <param name="PW"></param>
        public YggdrasilWorker(CompletedEventHandler completedEventHandler, string ID, string PW)
        {
            this.ID = ID;
            this.PW = PW;
            this.completedEventHandler = completedEventHandler;
            WorkerSupportsCancellation = true;
            DoWork += YggdrasilWorker_DoWork;
            RunWorkerCompleted += YggdrasilWorker_RunWorkerCompleted;
        }

        private void YggdrasilWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (user?.Name != null)
                completedEventHandler(this, ExchangeCode.SUCCESSFUL, user);
            else
                completedEventHandler(this, ExchangeCode.FAIL, user);
        }

        private void YggdrasilWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                user = YggdrasilLogin.Login(ID, PW);
            }
            catch (WebException)
            {
                user = new User() { Errinfo = "403" };
            }
        }
    }
}
