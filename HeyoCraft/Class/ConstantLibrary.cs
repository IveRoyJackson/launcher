using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HeyoCraft.Class
{
    class ConstantLibrary
    {
        public const string BaseUri = "http://www.heyox.net/";
        /// <summary>
        /// {0}是当前版本
        /// </summary>
        public const string UpdateUri = BaseUri + "update.php?ver={0}";
        public const string DownloadUri = BaseUri + "download.php";
        public const string HomePageUri = BaseUri + "HomePage.xaml";
        public static string HomePageFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), "homepage.xaml");
    }
}
