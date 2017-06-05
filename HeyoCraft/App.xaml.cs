using Heyo.Class.Helper;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace HeyoCraft
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            //其他参考设置
            //string windowuser = @"D:\";
            //AyuiConfig.AYUI_BackgroundImageDirectoryPath = windowuser + @"\44\ayuiresource\bg\";
            //AyuiConfig.AYUI_BackgroundImageThumbDirectoryPath = windowuser + @"44\t\thumb\";
            //AyuiConfig.AYUI_RelativeBg_BgPath = @"\ayuiresource\bg\";
            //AyuiConfig.AYUI_RelativeBg_ThumbPath = @"\t\thumb\";
            //AyuiConfig.AYUI_DiyColorTabTitle = "AY推荐颜色";
            //AyuiConfig.AYUI_DiyImageDirectoryName = "历史图片";
            //AyuiConfig.AYUI_ConfigFileNamePath = windowuser + @"44\ayuiresource\application.xml";
            //AyuiConfig.AYUI_ColorsCollectionBuilder(new List<string> { "#FFFFFF" }, true);

            
            //Heyo.Properties.Resources.Char.
            if (!Directory.Exists("Contents\\bg\\"))
            {
                Directory.CreateDirectory("Contents");
                Directory.CreateDirectory("Contents\\Config");
                Directory.CreateDirectory("Contents\\bg");
                Directory.CreateDirectory("Contents\\bg\\bg");
                Directory.CreateDirectory("Contents\\bg\\bg\\a默认");
                Directory.CreateDirectory("Contents\\bg\\thumb");
                Directory.CreateDirectory("Contents\\bg\\thumb\\a默认");
                FileStream fs = new FileStream("Contents\\Config\\application.xml", FileMode.Create);
                byte[] appXml = Encoding.UTF8.GetBytes(HeyoCraft.Properties.Resources.application.ToCharArray());
                fs.Write(appXml, 0, appXml.Length);
                fs.Close();
                fs = new FileStream("Contents\\Config\\Config.xml", FileMode.Create);
                byte[] configXml = Encoding.UTF8.GetBytes(HeyoCraft.Properties.Resources.Config.ToCharArray());
                fs.Write(configXml, 0, configXml.Length);
                fs.Close();
                fs = new FileStream("Contents\\bg\\bg\\a默认\\BG.jpg", FileMode.Create);
                byte[] bg = HeyoCraft.Properties.Resources.BG.ToBytes();
                fs.Write(bg, 0, bg.Length);
                fs.Close();
                fs = new FileStream("Contents\\bg\\thumb\\a默认\\BG.jpg", FileMode.Create);
                byte[] bg_t = HeyoCraft.Properties.Resources.BG_T.ToBytes();
                fs.Write(bg_t, 0, bg_t.Length);
                fs.Close();
            }


            //可以这里.AddFonts("aydemo", "#iconfont") 添加第三方字体
            Current.InitGlobalPackUri().AddResourceDictionary(AyExtension.CreatePackUriString("/Contents/Styles/AYUIProjectDictionary.xaml")).AYUI();
            base.OnStartup(e);
        }
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("出现了异常,请在启动器目录下查阅heyo.log." + Environment.NewLine + e.Exception.Message);
            LogHelper.WriteError(e.Exception);
            e.Handled = true;
        }
    }
}
