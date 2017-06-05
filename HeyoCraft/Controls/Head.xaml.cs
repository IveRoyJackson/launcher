using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Net;
using static Heyo.Class.Helper.ImageHelper;

namespace HeyoCraft.Controls
{
    /// <summary>
    /// Head.xaml 的交互逻辑
    /// </summary>
    public partial class Head : UserControl
    {
        public Head()
        {
            InitializeComponent();
            PlayerName = "";
        }
        public event EventHandler HeadShowed;

        public readonly static DependencyProperty  PlayerNameProperty =
            DependencyProperty.Register(
            "PlayerName",
            typeof(string),
            typeof(Head)
        );
        public string PlayerName
        {
            get {
                return (string)GetValue(PlayerNameProperty);
            }
            set {

                if (value?.Length > 0)
                {
                    WebClient wc = new WebClient();
                    wc.DownloadDataCompleted += Wc_DownloadDataCompleted;
                    wc.DownloadDataAsync(new Uri(string.Format(uri, value)));
                }
                else
                {
                    Skin = Properties.Resources.Char;
                }
                SetValue(PlayerNameProperty, value);
            }
        }
        private const string uri = "http://skins.minecraft.net/MinecraftSkins/{0}.png";

        private void Wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Result != null)
                Skin = e.Result.ToBitmap();
        }

        private Bitmap _skin;
        public Bitmap Skin
        {
            get
            {
                return _skin;
            }
            set
            {
                _skin = value.SetSize(1024, 512,
                    interpolationMode: System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor,
                    smoothingMode: System.Drawing.Drawing2D.SmoothingMode.None,
                    compositingQuality: System.Drawing.Drawing2D.CompositingQuality.Default
                    );
                int w = _skin.Width;
                int h = _skin.Height;
                HeadFrontImage.ImageSource = _skin.Cut((w * 75 / 640), (h * 75 / 320), w / 8, h / 4).ToBitmapSource();
                HeadBackImage.ImageSource = _skin.Cut((w * 220 / 640), (h * 75 / 320), w / 8, h / 4).ToBitmapSource();
                HeadLeftImage.ImageSource = _skin.Cut((w * 155 / 640), (h * 75 / 320), w / 8, h / 4).ToBitmapSource();
                HeadRightImage.ImageSource = _skin.Cut((w * -5 / 640), (h * 75 / 320), w / 8, h / 4).ToBitmapSource();
                HeadTopImage.ImageSource = _skin.Cut((w * 75 / 640), (h * -5 / 320), w / 8, h / 4).ToBitmapSource();
                HeadBottomImage.ImageSource = _skin.Cut((w * 155 / 640), (h * -5 / 320), w / 8, h / 4).ToBitmapSource();
                //这个我真的不知道要怎么跟你解释，因为我只懂哲学♂
                if (HeadShowed != null)
                    HeadShowed(this, new EventArgs());
            }
        }
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public extern static System.IntPtr GetDC(System.IntPtr hWnd);
    }
}
