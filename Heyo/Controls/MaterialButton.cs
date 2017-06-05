using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace Heyo.Controls
{
    public class MaterialButton : ButtonBase
    {

        static MaterialButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialButton), new FrameworkPropertyMetadata(typeof(MaterialButton)));
        }
        public int CornerRadius
        {
            get => (int)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public readonly static DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
            "CornerRadius",
            typeof(int),
            typeof(ImageSwitchViewer),
            new PropertyMetadata(2)
        );
    }
}
