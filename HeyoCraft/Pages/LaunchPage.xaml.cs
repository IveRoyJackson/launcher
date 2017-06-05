using Heyo.Class;
using Heyo.Class.Data;
using Heyo.Class.Helper;
using Heyo.Pages;
using Heyo.Workers;
using HeyoCraft.Class.Data;
using HeyoCraft.Class.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Heyo.Class.Helper.ZIP.ZIP;

namespace HeyoCraft.Pages
{
    /// <summary>
    /// LaunchPage.xaml 的交互逻辑
    /// </summary>
    public partial class LaunchPage : DialogPage
    {
        public Launcher Launcher { get; set; }
        public LaunchPage(Launcher launcher)
        {
            InitializeComponent();
            Detailed.Text = launcher.Game.id;
            Loaded += DialogPage_Loaded;
            Launcher = launcher;
            Title = "";
            ShowCloseButton = false;
        }
        private void DialogPage_Loaded(object sender, EventArgs e)
        {
            try
            {

                if (!File.Exists(Environment.CurrentDirectory + "\\.minecraft\\assets\\indexes\\" + Launcher.Game.assets + ".json"))
                {
                    DownloadAssets();
                }
                Launcher.Launch();
                Close();
            }
            catch (WebException)
            {
                NavigationService.Navigate(new YesOrNoPage("下载失败", "操作超时"));
            }


            //Console.WriteLine(Launcher.BuildArguments());
        }
        private string bmclapi = "http://bmclapi2.bangbang93.com/";
        private void DownloadAssets()
        {
            Operation.Text = "下载资源文件";
            
            WebClient wc = new WebClient();
            Minecraft mc = Launcher.Game;
            string indexPath = Environment.CurrentDirectory + "\\.minecraft\\assets\\indexes\\" + mc.assets + ".json";
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(indexPath));
            wc.DownloadFile(bmclapi + "/indexes/" + mc.assets + ".json", indexPath);
            string jsonString = wc.DownloadString(bmclapi + "/indexes/" + mc.assets + ".json");
            var assets = AssetsIndexDeserializer.Deserialize(jsonString);
            foreach (var item in assets)
            {
                string path;
                if (item.name.Contains(".lang"))
                {
                    path = Environment.CurrentDirectory + "\\.minecraft\\assets\\lang\\" + item.name;
                }
                else
                {
                    path = Environment.CurrentDirectory + "\\.minecraft\\assets\\objects\\" + item.hash.Substring(0, 2) + "\\" + item.hash;
                }
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                }
                string url = bmclapi + "/assets/" + item.hash.Substring(0, 2) + "/" + item.hash;
                Detailed.Text = item.name;
                NavigationService.Refresh();
                wc.DownloadFile(url, path);
            }
        }
    }
}
