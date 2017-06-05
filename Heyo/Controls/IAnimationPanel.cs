using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;
using System.Windows.Media.Animation;
namespace Heyo.Controls
{
    public interface IAnimationPanel
    {
        double AnimationSpeed { get; set; }
        EasingFunctionBase Function { get; set; }
        void Anim();
    }
}
