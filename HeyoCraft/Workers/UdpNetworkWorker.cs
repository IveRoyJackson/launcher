using System.Text;
using System.ComponentModel;
using HeyoCraft.Reference;
using System.Net;
using Heyo.Class.Helper;

namespace HeyoCraft.BackgroundWorkers
{
    sealed class UdpNetworkWorker : NetworkWorker
    {
        //private int tryTimes = 4;
        UdpNetwork net =new UdpNetwork(IPAddress.Parse("127.0.0.1"), 20144);
        /// <summary>
        /// 网络操作后台线程
        /// </summary>
        /// <param name="completedEventHandler">完成时调用</param>
        /// <param name="operation">所需的操作</param>
        /// <param name="beSendedData">需要发送的数据</param>
        /// <param name="tryTimes">尝试次数</param>
        /// <param name="passDown">需要在完成时传给CompletedEventHandler委托的东西</param>
        public UdpNetworkWorker(CompletedEventHandler completedEventHandler, ExchangeCode operation, string beSendedData, object passDown=null)
            :base(completedEventHandler, operation,Encoding.UTF8.GetBytes(beSendedData),passDown)
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
        public UdpNetworkWorker(CompletedEventHandler completedEventHandler, ExchangeCode operation, byte[] beSendedData, object passDown = null)
            : base(completedEventHandler, operation, beSendedData, passDown)
        {
        }

        protected override void NetworkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string dataString = e.Result as string;
            //Console.WriteLine(dataString);
            completedEventHandler(this, ExchangeHelper.ToExchangeCode(dataString),Encoding.UTF8.GetBytes( dataString.Substring(4)), passDown);
        }

        protected override void NetworkWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] operationBytes = Encoding.UTF8.GetBytes(ExchangeHelper.ToString(operation));
            byte[] beSended = ByteHelper.LinkBytes(operationBytes,beSendedData);

            byte[] receive = net.Send(beSended, 10000, tryTimes);
            string originData = receive == null ? ExchangeHelper.ToString(ExchangeCode.TIME_OUT) : Encoding.UTF8.GetString(receive);
            e.Result = originData;
        }
    }
}
