using Heyo.Class.Data;
using HeyoCraft.Class.Data;
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
using System.Windows.Shapes;
using static Heyo.Class.Helper.ImageHelper;

namespace HeyoCraft.Controls
{
    /// <summary>
    /// MinecraftItem.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenshotItem : GameItem
    {
        public ScreenshotItem()
        {
            InitializeComponent();
        }
        ScreenshotData _data;
        public ScreenshotData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                idText.Text = value.Name;
                icon.Source = new Bitmap(value.AbsolutelyFilePath).SetSize(128,72).ToBitmapImage();
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(new BitmapImage(new Uri(_data.AbsolutelyFilePath)));
        }
    }
}
