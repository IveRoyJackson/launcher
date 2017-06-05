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
    /// MaterialBoard.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialBoard : MaterialButton
    {
        public MaterialBoard()
        {
            InitializeComponent();

            SizeChanged += MaterialBoard_SizeChanged;

        }

        private void MaterialBoard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((Height == double.NaN ? ActualHeight : Height) < 200)
            {
                userLabel.Visibility = Visibility.Collapsed;
                userAvatar.Height = userAvatar.Width = 24;
                Row1.Height = new GridLength(46);
            }
            else
            {
                userLabel.Visibility = Visibility.Visible;
                userAvatar.Height = userAvatar.Width = 36;
                Row1.Height = new GridLength(64);
            }
        }

        public object Title
        {
            get
            {
                return titleLabel.Content;
            }
            set
            {
                titleLabel.Content = value;
            }
        }
        public string Intruduction
        {
            get
            {
                return titleBlcok.Text;
            }
            set
            {
                titleBlcok.Text = value;
            }
        }
    }
}
