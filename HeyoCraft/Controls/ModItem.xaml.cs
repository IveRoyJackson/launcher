using Heyo.Class.Data;
using Heyo.Class.Helper;
using Heyo.Class.Helper.ZIP;
using HeyoCraft.Class.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class ModItem : GameItem
    {
        public ModItem()
        {
            InitializeComponent();
        }
        ModData _data;
        public ModData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                idText.Text = value.Name;
                Icon fileIcon = Win32.GetIcon(value.AbsolutelyFilePath);
                byte[] modIconBuffers = ZIP.ExtractFile(_data.AbsolutelyFilePath, new Regex(".png"),extractFileDirectory:"").GetBuffer();
                if(modIconBuffers.Length>1024)
                {
                    Bitmap bitmap = modIconBuffers.ToBitmap();
                    BitmapSource image = bitmap.SetSize(bitmap.Width*72/bitmap.Height, 72).ToBitmapImage();
                    IconHeigth = 72;
                    IconWidth = 128;
                    icon.Source = image;
                }else
                {
                    IconHeigth = 32;
                    IconWidth = 32;
                    icon.Source = fileIcon.ToImageSource();
                }
                
                
            }
        }
    }
}
