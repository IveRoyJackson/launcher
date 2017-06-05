using HeyoCraft.Class.Data;
using System;
using System.Windows.Media.Imaging;

namespace HeyoCraft.Controls
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
