using HeyoCraft.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;

namespace Heyo.Workers.xxx
{
    public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
    public delegate void RunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);
    public class ProgressChangedEventArgs
    {
        public ProgressChangedEventArgs(double percentage,object userState)
        {
            ProgressPercentage = percentage;
            UserState = userState;
        }
        public double ProgressPercentage { get; private set; }
        public object UserState { get; private set; }
    }
    public class RunWorkerCompletedEventArgs
    {
        public RunWorkerCompletedEventArgs(bool cancelled, Exception error, ExchangeCode result, object userState)
        {
            Cancelled = cancelled;
            Error = error;
            Result = result;
            UserState = userState;
        }
        public bool Cancelled { get; private set; }
        public Exception Error { get; private set; }
        public ExchangeCode Result { get; private set; }
        public object UserState { get; private set; }
    }
    public abstract class Worker
    {
        public event ProgressChangedEventHandler ProgressChanged;
        public event RunWorkerCompletedEventHandler OnWorkerCompleted;
        public Dispatcher dispatcher { get; set; }
        public Worker(Dispatcher dis)
        {
            dispatcher = dis;
        }
        /// <summary>
        /// 进度是否可低于上次报告的进度
        /// </summary>
        public bool Rreversible { get; set; } = false;
        public void RunWorkAsyn()
        {
            Thread t = new Thread(new ThreadStart(Work));
            t.Start();
        }
        protected void Completed(object sender, bool cancelled = false, Exception error = null, ExchangeCode result = ExchangeCode.SUCCESSFUL, object userState = null)
        {
            if (OnWorkerCompleted == null)
                return;
            dispatcher.Invoke(OnWorkerCompleted, sender, new RunWorkerCompletedEventArgs(cancelled, error, result, userState));
        }
        private double lastPercentage =0;
        /// <summary>
        /// 发送进度
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="percentage">进度百分比</param>
        /// <param name="userState">用户状态</param>
        /// <param name="isAsync">异步发送,不会阻塞代码,但不能保证消息到达顺序</param>
        protected virtual void ReportProgress(object sender ,double percentage , object userState=null,bool isAsync=false)
        {
            if (ProgressChanged == null)
                throw new Exception("ProgressChanged事件为null");
            if (!Rreversible && percentage <= lastPercentage)
                return;

            lastPercentage = percentage;
            if (isAsync)
                dispatcher.BeginInvoke(ProgressChanged,sender, new ProgressChangedEventArgs(percentage, userState));
            else
                dispatcher.Invoke(ProgressChanged, sender, new ProgressChangedEventArgs(percentage, userState));
        }
        protected abstract void Work();
    }
}
