using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Heyo.Controls
{
    class MaterialFloatingBox : MaterialCard
    {
        Viewbox viewbox;
        public MaterialFloatingBox()
        {
            viewbox = new Viewbox();
            base.Child = viewbox;
            ShadowAnim = true;
            Opacity = 0;
            Visibility = Visibility.Collapsed;
            AnimationSpeed = 8;
            downRadius = 10;
            shadowOpacity = 0.2;
            Loaded += (s, e) => {
                originWidth = Width;
                originHeight = Height;
            };

        }
        public new UIElement Child
        {
            get
            {
                return viewbox.Child;
            }
            set
            {
                viewbox.Child = value;
            }
        }
        private double originWidth, originHeight;
        public void Show()
        {
            Visibility = Visibility.Visible;
            SetSizeWithOutAnim(0, 0);
            Width = originWidth;
            Height = originHeight;
            Opacity = 1;
        }
        public void Hide()
        {
            Width = 0;
            Height = 0;
            Opacity = 0;
            sizeAnim.Completed += SizeAnim_Completed;
        }

        private void SizeAnim_Completed(object sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;
            sizeAnim.Completed -= SizeAnim_Completed;
        }
    }
}
