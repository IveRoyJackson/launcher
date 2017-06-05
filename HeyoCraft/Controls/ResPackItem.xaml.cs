using Heyo.Class.Data;
using Heyo.Class.Helper;
using Heyo.Class.Helper.ZIP;
using HeyoCraft.Class.Data;
using System;
using System.Collections.Generic;
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

namespace HeyoCraft.Controls
{
    /// <summary>
    /// MinecraftItem.xaml 的交互逻辑
    /// </summary>
    public partial class ResPackItem : GameItem
    {
        public ResPackItem()
        {
            InitializeComponent();
        }
        ResPackData _data;
        public ResPackData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                idText.Text = value.Name;
                BitmapSource image = ZIP.ExtractFile(_data.AbsolutelyFilePath,new System.Text.RegularExpressions.Regex("^pack.png")).GetBuffer().ToBitmapImage();
                icon.Source = image;
            }
        }
    }
}
