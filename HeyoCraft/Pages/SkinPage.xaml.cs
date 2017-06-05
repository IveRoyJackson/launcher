using Ay.Framework.WPF.Applications;
using Heyo.Class.Events;
using Heyo.Controls;
using Heyo.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static Heyo.Class.Helper.ImageHelper;
namespace HeyoCraft.Pages
{
    /// <summary>
    /// SkinPage.xaml 的交互逻辑
    /// </summary>
    public partial class SkinPage : DialogPage
    {
        public event EventHandler<StringEventArgs> BackgroundImageChanged;
        public SkinPage()
        {
            InitializeComponent();
            Loaded += SkinPage_Loaded;
            addBtn.Click += AddBtn_Click;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "PNG文件|*.png|BMP文件|*.bmp|JPG文件|*.jpg"
            };
            if (openFileDialog.ShowDialog()==true)
            {
                string bgPath = AyuiConfig.AYUI_BackgroundImageDirectoryPath+"z自定义\\"+Path.GetFileName(openFileDialog.FileName);
                if(!Directory.Exists(Path.GetDirectoryName(bgPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(bgPath));
                }
                //string thumbPath= Path.Combine(AyuiConfig.AYUI_BackgroundImageThumbDirectoryPath, Path.GetFileName(openFileDialog.FileName));
                File.Copy(openFileDialog.FileName, bgPath);
                ImageRadioButton imageRadioButton = new ImageRadioButton() { Source = new BitmapImage(new Uri(bgPath)), GroupName = "images" };
                imageRadioButton.Checked += ImageRadioButton_Checked;
                defPanel.Children.Insert(defPanel.Children.Count - 1, imageRadioButton);

                SelectImage(bgPath);
            }
        }
        private void SkinPage_Loaded(object sender, EventArgs e)
        {
            var files=Directory.EnumerateFiles(Environment.CurrentDirectory+ "\\Contents\\bg\\bg","*.*",SearchOption.AllDirectories);
            string nowPath = ConfigSetting.GetXmlValue("skinBg", "path");
            foreach (string item in files)
            {
                if(Path.GetExtension(item)==".png" || Path.GetExtension(item) == ".jpg"|| System.IO.Path.GetExtension(item) == ".bmp")
                {
                    ImageRadioButton imageRadioButton = new ImageRadioButton() { Source = new BitmapImage(new Uri(item)), GroupName = "images" };
                    imageRadioButton.Checked += ImageRadioButton_Checked;
                    defPanel.Children.Insert(defPanel.Children.Count-1,imageRadioButton);
                    if(nowPath==item)
                    {
                        imageRadioButton.IsChecked = true;
                    }
                }
            }
        }

        private void ImageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ImageRadioButton irb = sender as ImageRadioButton;
            string name = (irb.Source as BitmapImage).UriSource.OriginalString;
            SelectImage(name);
        }
        /// <summary>
        /// 选择图片
        /// </summary>
        /// <param name="bgPath">被选择的图片路径</param>
        private void SelectImage(string bgPath)
        {
            string thumbPath = Path.Combine(AyuiConfig.AYUI_BackgroundImageThumbDirectoryPath,"z自定义", Path.GetFileName(bgPath));
            ConfigSetting.SetXmlValue("skinBg", "path",bgPath.Replace(Environment.CurrentDirectory,""));
            if(!File.Exists(thumbPath))
            {
                Bitmap bm = new Bitmap(bgPath);
                BitmapImage bi = bm.SetSize(128, 72).ToBitmapImage();
                byte[] thumbBuffer = bi.ToBytes();
                if (!Directory.Exists(Path.GetDirectoryName(thumbPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(thumbPath));
                }
                FileStream fs = new FileStream(thumbPath, FileMode.Create);
                fs.Write(thumbBuffer, 0, thumbBuffer.Length);
                fs.Close();
            }
            ConfigSetting.SetXmlValue("skinBg", "thumb", thumbPath.Replace(Environment.CurrentDirectory, ""));
            //更新主题色,使出反编译大法才找到下面这个方法↓
            ThemeManager.AYUI(Application.Current);
            BackgroundImageChanged?.Invoke(this, new StringEventArgs(bgPath));
        }
    }
}
