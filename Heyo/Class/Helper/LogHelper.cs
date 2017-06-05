using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Heyo.Class.Helper
{
    public class LogHelper
    {
        /// <summary>
        /// 向启动器日志文件写入报告
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLog(string log)
        {
            FileStream fileStream = new FileStream(Path.Combine(Environment.CurrentDirectory, "HeyoMC.log"), FileMode.Append);
            byte[] buffers = string.Join(" ",DateTime.Now.ToString("yy-MM-dd-HH-mm-ss") , ":" , log , Environment.NewLine).ToBytes();
            fileStream.Write(buffers, 0, buffers.Length);
            fileStream.Close();
        }
        /// <summary>
        /// 通过WriteLog方法向启动器日志文件写入错误
        /// </summary>
        /// <param name="e"></param>
        public static void WriteError(Exception e)
        {
            WriteLog("Error-" + e.TargetSite.Name);
            WriteLog("Error-" + e.Message);
            WriteLog("Error-" + e.StackTrace);
        }
    }
}
