using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// CircularProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class ArcProgressBar : UserControl
    {
        public ArcProgressBar()
        {
            InitializeComponent();
        }
        public double IndicatorThickness
        {
            get
            {
                return Indicator.ArcThickness;
            }
            set
            {
                Indicator.ArcThickness = value;
            }
        }
        public double TrackThickness
        {
            get
            {
                return Track.ArcThickness;
            }
            set
            {
                Track.ArcThickness = value;
            }
        }
        public double TrackRadius
        {
            get
            {
                return Track.Width;
            }
            set
            {
                Track.Width = Track.Height = value;
            }
        }
        public double IndicatorRadius
        {
            get
            {
                return Indicator.Width;
            }
            set
            {
                Indicator.Width = Indicator.Height = value;
            }
        }
        public Brush TrackBrush
        {
            get
            {
                return Track.Fill;
            }
            set
            {
                Track.Fill = value;
            }
        }
        public Brush IndicatorBrush
        {
            get
            {
                return Indicator.Fill;
            }
            set
            {
                Indicator.Fill = value;
            }
        }
        public double Minimun { get; set; } = 0;
        public double Maxmum { get; set; } = 100;
        private double _value = 0;
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                int per = (int)(value / (Maxmum - Minimun) * 100D);
                per = per >= 99 ? 100 : per;
                percentage.Content = string.Format("{0}%", per);
                double angle= 360 * value / (Maxmum - Minimun);
                Indicator.EndAngle = angle > 360 ? 360 : angle;
            }
        }
        public bool ShowPercentage
        {
            get
            {
                return percentage.Visibility == Visibility.Visible;
            }
            set
            {
                percentage.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
