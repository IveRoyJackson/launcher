using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Heyo.Controls
{
    public class OpacityAnimationPanel : Border,IAnimationPanel
    {

        public double AnimationSpeed { get; set; } = 4;

        public double From { get; set; } = 0;
        public double To { get; set; } = 1;
        public event EventHandler Completed;
        public EasingFunctionBase Function { get; set; }= new PowerEase() { Power = 2, EasingMode = EasingMode.EaseOut };

        private DoubleAnimation opacityAnim = new DoubleAnimation();
        public OpacityAnimationPanel()
        {
            opacityAnim.EasingFunction = Function;
            opacityAnim.SpeedRatio = AnimationSpeed;
        }

        public void Anim()
        {
            opacityAnim.From = From;
            opacityAnim.To = To;
            if (Completed != null)
            {
                opacityAnim.Completed += Completed;
            }

            BeginAnimation(OpacityProperty, opacityAnim);
        }
    }
}
