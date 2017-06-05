using Heyo.Class.Events;
using Heyo.Class.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace HeyoCraft.Class
{
    class UpdateHelper
    {
        //public const string CheckUrl = "http://www.heyox.net/update.php?ver={0}";
        public const string CheckUrl = "http://45.32.137.71/update.php?ver={0}";
        public const string DownloadUrl = "http://45.32.137.71/download.php";
        public static string FileName { get; set; }
        public static int LatestVersion { get; set; } = 0;
        public static void StartCheckUpdate(EventHandler<StringEventArgs> hasUpdateEvent)
        {

            int nowVer = GetAppVersion();
            WebClient wc = new WebClient();
            string returnData;
            wc.DownloadDataCompleted += (s, e) =>
            {
                try
                {
                    if (e?.Result != null)
                    {
                        returnData = Encoding.UTF8.GetString(e.Result);
                        string[] versAndLogs = returnData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        LatestVersion = versAndLogs[0].Substring(0, versAndLogs[0].IndexOf('#')).ToInt();
                        FileName = Path.GetFileName(versAndLogs[0].Split('#')[1]);
                        //StringBuilder logBuilder = new StringBuilder();
                        //logBuilder.Append(versAndLogs[1].Replace("\n", "\n+"));
                        if (HasUpdate())
                        {
                            hasUpdateEvent?.Invoke(s, new StringEventArgs(string.Format("新版本{0}的更改如下:\n{1}", LatestVersion, versAndLogs[1])));
                        }
                    }
                }
                catch
                {

                }
                
            };
            wc.DownloadDataAsync(new Uri(string.Format(CheckUrl, nowVer)));

        }
        /// <summary>
        /// 检测更新
        /// </summary>
        /// <returns>如果有新版本返回true</returns>
        public static bool HasUpdate()
        {
            return GetAppVersion() < LatestVersion;
        }
        public static int GetAppVersion()
        {
            Version ver = Application.ResourceAssembly.GetName(false).Version;
            return ver.Major * 1000 + ver.Minor * 100 + ver.Build * 10 + ver.Revision;
        }
        public static void UpdateApp()
        {
            WebClient wc = new WebClient();
            string newPath = Path.Combine(Environment.CurrentDirectory, FileName);
            wc.DownloadFileCompleted += (s, e) =>
            {
                Process.Start(newPath);
                Application.Current.Shutdown();
            };
            wc.DownloadFileAsync(new Uri(DownloadUrl), newPath);
        }
    }
}
