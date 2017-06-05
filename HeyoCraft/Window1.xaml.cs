using Heyo.Class.Helper;
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
using System.Windows.Shapes;
using System.Windows.Interop;
using Heyo;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace HeyoCraft
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : WindowPlus
    {
        public Window1()
        {
            InitializeComponent();
            Loaded += Window1_Loaded;
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            //WindowStyleHelper.SetOpacity(this, 0.5);
            //Content = new Button() { Margin = new Thickness(20) };
            //UpdateLayout();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Opacity = e.NewValue;
            WindowStyleHelper.SetOpacity(this, e.NewValue);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            /*DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromTicks(33);
            double op = 1;
            dt.Tick += (s, e2) => {
                if(op>0)
                {
                    op-=0.05;
                    WindowStyleHelper.SetOpacity(this, op);
                }
            };
            dt.Start();*/
        }
    }
}
