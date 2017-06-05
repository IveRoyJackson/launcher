using System;
using System.Collections.Generic;
using System.Linq;
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
using System.ComponentModel;
using Heyo.Controls;

namespace Heyo.Controls
{
    /// <summary>
    /// Avatar.xaml 的交互逻辑
    /// </summary>
    public partial class Avatar : ClippingBorder
    {
        public Avatar()
        {
            InitializeComponent();
            //Loaded += (s,e)=>{ defaultImageSource = AvatarImage.Clone(); } ;
        }
        private ImageSource defaultImageSource=new BitmapImage();
        public double AvatarBorderThickness
        {
            get
            {
                return BorderThickness.Left;
            }
            set
            {
                BorderThickness=new Thickness (value);
            }
        }
        public ImageSource AvatarImage
        {
            get
            {
                return Background == null ? null : ((ImageBrush)Background).ImageSource;
            }
            set
            {
                if(value==null)
                {
                    Background = new ImageBrush(defaultImageSource);
                }
                else
                {
                    Background = new ImageBrush(value);
                }
            }
        }
    }
}
