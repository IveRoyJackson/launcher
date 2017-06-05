using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using HeyoCraft.Reference;
using System.Windows.Threading;
using System.ComponentModel;

namespace HeyoCraft.Workers
{
    public class HttpDownloader : BackgroundWorker
    {
        int times = 4;
        public HttpDownloader(string uri,string path)
        {
            this.FromHttp = uri;
            this.FilePath = path;
            DoWork += Work;
            WorkerReportsProgress = true;
        }
        public void Work(object sender, DoWorkEventArgs e)
        {
            HttpWebRequest request =(HttpWebRequest) WebRequest.Create(FromHttp);
            int _times = 1;
            HttpWebResponse response;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch(WebException)
            {
                if(_times<times)
                {
                    _times++;
                    Work(sender, e);
                }
                else
                {
                    e.Result = new WebException();
                    //throw new WebException();
                }
                return;
            }
            Stream responseStream = response.GetResponseStream();
            if (!Directory.Exists(Path.GetDirectoryName( FilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            FileStream fileStream = new FileStream(FilePath, FileMode.Create);
            Size = response.ContentLength;
            byte[] kiloBytes = new byte[1048576];
            int obytes = 0;
            int start = Environment.TickCount;///开始计时时间
            do
            {
                obytes = responseStream.Read(kiloBytes, 0, kiloBytes.Length);
                fileStream.Write(kiloBytes, 0, obytes);
                LengthGot += obytes;

                int time = Environment.TickCount - start;
                if(time>0)
                {
                    Speed = LengthGot * (1000D / time);
                }
                //Console.WriteLine("{0}正在下载,速度为{1}", FilePath, Speed);
                ReportProgress((int)((double)LengthGot / Size * 100D));
            } while (obytes > 0);
            e.Result = FilePath;
            responseStream.Close();
            fileStream.Close();
            
        }
        /// <summary>
        /// 下载速度,字节每秒
        /// </summary>
        public double Speed { get; private set; }
        /// <summary>
        /// 总字节数
        /// </summary>
        public long Size { get; private set; } = 0;
        /// <summary>
        /// 已经得到的长度
        /// </summary>
        public long LengthGot { get; private set; } = 0;
        /// <summary>
        /// HTTP源
        /// </summary>
        public string FromHttp { get; private set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; private set; }
    }
}
