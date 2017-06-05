using System.Text;
using System.ComponentModel;
using HeyoCraft.Reference;
using System.Net;

namespace HeyoCraft.BackgroundWorkers
{
    public class NetworkWorker : BackgroundWorker
    {
        protected byte[] beSendedData;
        protected ExchangeCode operation;
        protected int tryTimes = 4;
        /// <summary>
        /// 在完成时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="resultExchangeCode">通信结果</param>
        /// <param name="originData">服务器返回的数据</param>
        /// <param name="passDown">被传递的东西，通常是要被操作的</param>
        public delegate void CompletedEventHandler(NetworkWorker sender, ExchangeCode resultExchangeCode, byte[] originData, object passDown);
        protected CompletedEventHandler completedEventHandler;
        UdpNetwork net =new UdpNetwork(IPAddress.Parse("127.0.0.1"), 20143);
        protected object passDown=null;
        /// <summary>
        /// 网络操作后台线程
        /// </summary>
        /// <param name="completedEventHandler">完成时调用</param>
        /// <param name="operation">所需的操作</param>
        /// <param name="beSendedData">需要发送的数据</param>
        /// <param name="tryTimes">尝试次数</param>
        /// <param name="passDown">需要在完成时传给CompletedEventHandler委托的东西</param>
        public NetworkWorker(CompletedEventHandler completedEventHandler, ExchangeCode operation, string beSendedData, object passDown)
            :this(completedEventHandler, operation,Encoding.UTF8.GetBytes(beSendedData),passDown)
        {
        }
        /// <summary>
        /// 网络操作后台线程
        /// </summary>
        /// <param name="completedEventHandler">完成时调用</param>
        /// <param name="operation">所需的操作</param>
        /// <param name="beSendedData">需要发送的数据</param>
        /// <param name="tryTimes">尝试次数</param>
        /// <param name="passDown">需要在完成时传给CompletedEventHandler委托的东西</param>
        public NetworkWorker(CompletedEventHandler completedEventHandler, ExchangeCode operation, byte[] beSendedData, object passDown)
        {
            this.beSendedData = beSendedData;
            this.operation = operation;
            this.completedEventHandler = completedEventHandler;
            WorkerSupportsCancellation = true;
            DoWork += NetworkWorker_DoWork;
            RunWorkerCompleted += NetworkWorker_RunWorkerCompleted;
            this.passDown = passDown;
        }

        protected virtual void NetworkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        protected virtual void NetworkWorker_DoWork(object sender, DoWorkEventArgs e)
        {
        }
    }
}
