using Heyo.Class.Data;
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

namespace Heyo.Controls
{
    /// <summary>
    /// MinecraftItem.xaml 的交互逻辑
    /// </summary>
    public partial class MinecraftItem : GameItem
    {
        public MinecraftItem()
        {
            InitializeComponent();
        }
        Minecraft _game;
        public Minecraft Game
        {
            get
            {
                return _game;
            }
            set
            {
                _game = value;
                string iconPath = value.AbsolutelyDirectory + value.icon;
                if (System.IO.File.Exists(iconPath))
                    icon.Source = new BitmapImage(new Uri(iconPath));
                idText.Text = _game.id;
            }
        }
    }
}
