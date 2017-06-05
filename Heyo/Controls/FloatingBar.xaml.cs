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
using System.Windows.Media.Animation;
using System.Windows.Markup;

namespace Heyo.Controls
{
    /// <summary>
    /// FloatingBar.xaml 的交互逻辑
    /// </summary>
    public partial class FloatingBar : UserControl
    {
        public enum Direction
        {
            Top,
            Buttom,
            Left,
            Right
        }
        public UIElementCollection Grid
        {
            get
            {
                return grid.Children;
            }
        }
        private Storyboard show,hide;
        public FloatingBar()
        {
            InitializeComponent();
            show = FindResource("Show") as Storyboard;
            hide = FindResource("Hide") as Storyboard;
            MouseEnter += (s, e) => { Show(); };
        }
        public Thickness TipMargin
        {
            get
            {
                return TopTip.Margin;
            }
            set
            {
                TopTip.Margin = ButtomTip.Margin = LeftTip.Margin = RightTip.Margin = value;
            }
        }
        private bool isShow = false;
        public void Show()
        {
            if(!isShow)
            {
                isShow = true;
                PreDirection();
                BeginStoryboard(show);
            }
        }
        public void Hide()
        {
            
            if (isShow)
            {
                isShow = false;
                PreDirection();
                BeginStoryboard(hide);
            }
        }
        private void PreDirection()
        {
            TopTip.Visibility = Visibility.Collapsed;
            ButtomTip.Visibility = Visibility.Collapsed;
            LeftTip.Visibility = Visibility.Collapsed;
            RightTip.Visibility = Visibility.Collapsed;

            Point start, end;
            switch (TipDirection)
            {
                default:
                    {
                        start = new Point(0.5, 0);
                        end = new Point(0.5, 1);
                        if (!ShowTip)
                            break;
                        TopTip.Visibility = Visibility.Visible;
                        break;
                    }
                case Direction.Top:
                    {
                        start = new Point(0.5, 0);
                        end = new Point(0.5, 1);
                        if (!ShowTip)
                            break;
                        TopTip.Visibility = Visibility.Visible;
                        break;
                    }
                case Direction.Buttom:
                    {
                        start = new Point(0.5, 1);
                        end = new Point(0.5, 0);
                        if (!ShowTip)
                            break;
                        ButtomTip.Visibility = Visibility.Visible;
                        break;
                    }
                case Direction.Left:
                    {
                        start = new Point(0, 0.5);
                        end = new Point(1, 0.5);
                        if (!ShowTip)
                            break;
                        LeftTip.Visibility = Visibility.Visible;
                        break;
                    }
                case Direction.Right:
                    {
                        start = new Point(1, 0.5);
                        end = new Point(0, 0.5);
                        if (!ShowTip)
                            break;
                        RightTip.Visibility = Visibility.Visible;
                        break;
                    }
            }


            (grid1.OpacityMask as LinearGradientBrush).StartPoint = start;
            (grid1.OpacityMask as LinearGradientBrush).EndPoint = end;
        }
        public Direction TipDirection { get; set; }
        private bool _showTip = true;
        public bool ShowTip
        {
            get
            {
                return _showTip;
            }
            set
            {
                _showTip = value;

                
            }
        }
    }
}
