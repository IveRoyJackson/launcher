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
    public class BlurAnimationPanel : Border , IAnimationPanel
    {

        public double AnimationSpeed { get; set; } = 4;
        public double From { get; set; }
        public double To { get; set; }
        public event EventHandler Complete;
        private DoubleAnimation radiusAnim = new DoubleAnimation();
        public EasingFunctionBase Function { get; set; } = new PowerEase() { Power = 4, EasingMode = EasingMode.EaseOut };
        public BlurAnimationPanel()
        {
            Effect = new BlurEffect() { Radius = 0 };
            radiusAnim.EasingFunction = Function;
            radiusAnim.SpeedRatio = AnimationSpeed;
        }


        public void Anim()
        {
            radiusAnim.To = To;
            if (Complete != null)
            {
                radiusAnim.Completed += Complete;
            }

            (Effect as BlurEffect).BeginAnimation(BlurEffect.RadiusProperty, radiusAnim);
        }
    }
}
