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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Heyo.Controls
{
    public class MaterialCard : ClippingBorder
    {
        /// <summary>
        /// 动画速度
        /// </summary>
        public double AnimationSpeed { get; set; } = 2;
        /// <summary>
        /// 鼠标移入移出动画
        /// </summary>
        public bool MouseEnterLeaveAnim { get; set; } = true;
        /// <summary>
        /// 阴影动画
        /// </summary>
        public bool ShadowAnim { get; set; } = true;
        /// <summary>
        /// 水波反馈
        /// </summary>
        public bool WaveFeedback { get; set; } = false;
        /// <summary>
        /// 开启阴影自适应
        /// </summary>
        public bool OpenShadowSelfAdaption { get; set; } = false;
        /// <summary>
        /// 开启阴影
        /// </summary>
        public bool Shadow
        {
            get
            {
                return _shadow;
            }
            set
            {
                _shadow = value;
                if (value)
                {
                    ShadowSelfAdaption();
                }
                else
                {
                    Effect = null;
                }
            }
        }
        private bool _shadow = false;
        protected DoubleAnimation sizeAnim = new DoubleAnimation();
        private ColorAnimation backgroundAnim = new ColorAnimation();
        private DoubleAnimation depthAnim = new DoubleAnimation();
        private DoubleAnimation opacityAnim = new DoubleAnimation();
        private DoubleAnimation radiusAnim = new DoubleAnimation();
        private ThicknessAnimation marginAnim = new ThicknessAnimation();
        private DoubleAnimation ellipse2SizeAnim = new DoubleAnimation() { From = 5 };
        private DoubleAnimation ellipse2OpacityAnim = new DoubleAnimation();
        private ThicknessAnimation ellipseAnim = new ThicknessAnimation();
        private ThicknessAnimation fixAnim = new ThicknessAnimation();//你不知道这是什么的，不用看了
        private ColorAnimationUsingKeyFrames backgroundAnimUsingKey = new ColorAnimationUsingKeyFrames();
        private PowerEase easeFunction = new PowerEase() { EasingMode = EasingMode.EaseInOut, Power = 4 };
        private Ellipse ellipse = new Ellipse(), ellipse2 = new Ellipse();

        public double upDepth = 4;//抬升时高度
        public double upRadius = 3;//抬升时阴影半径倍率
        public double downDepth = 1;//下降时高度
        public double downRadius = 3;//下降时阴影半径(会随自适应改变,默认值为3)
        public int shadowDireection = 260;
        public double shadowOpacity = 0.1;
        public MaterialCard()
        {
            Loaded += MaterialCard_Loaded;
            UseLayoutRounding = true;

            backgroundAnim.EasingFunction
                = sizeAnim.EasingFunction
                = opacityAnim.EasingFunction
                = marginAnim.EasingFunction
                = ellipse2SizeAnim.EasingFunction
                = fixAnim.EasingFunction
                = ellipse2OpacityAnim.EasingFunction
                = easeFunction;

            ellipseAnim.EasingFunction =
                depthAnim.EasingFunction =
                radiusAnim.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 3 };

            ellipse2OpacityAnim.SpeedRatio = AnimationSpeed / 2;
            opacityAnim.SpeedRatio
                = ellipse2SizeAnim.SpeedRatio
                = fixAnim.SpeedRatio
                = backgroundAnimUsingKey.SpeedRatio
                = sizeAnim.SpeedRatio
                = marginAnim.SpeedRatio
                = backgroundAnim.SpeedRatio
                = AnimationSpeed;

            depthAnim.SpeedRatio = radiusAnim.SpeedRatio = AnimationSpeed * 2;
            ellipseAnim.SpeedRatio = AnimationSpeed * 1.5;
            marginAnim.Completed += (s, e) => { UseLayoutRounding = true; };
            sizeAnim.Completed += (s, e) => { UseLayoutRounding = true; };


            MouseEnter += (s, e) => { BeginMouseEnterAnim(); };
            MouseLeave += (s, e) =>
            {
                //Point mouse = Mouse.GetPosition(this);
                //if (mouse.X < 4 || mouse.X > ActualWidth - 4 || mouse.Y < 4 || mouse.Y > ActualHeight - 4)
                //{
                    BeginMouseLeaveAnim();
                //}

            };
            MouseDown += (s, e) => { if (Shadow && ShadowAnim) { BeginShadowAnimation(downDepth, downRadius); } };
            MouseUp += (s, e) => { if (Shadow && ShadowAnim) { BeginShadowAnimation(upDepth, downRadius * upRadius); } };
            MouseDown += MaterialCard_MouseDown;


            ellipse2SizeAnim.From = 0;
            ellipse2OpacityAnim.From = 1;
            ellipse2OpacityAnim.To = 0;
            fixAnim.From = new Thickness(0);

            ellipse.HorizontalAlignment = HorizontalAlignment.Center;
            ellipse.VerticalAlignment = VerticalAlignment.Top;
            ellipse.Margin = new Thickness(0, ActualHeight, 0, 0);
            ellipse.Effect = new BlurEffect() { Radius = 20 };
            ellipse2.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse2.VerticalAlignment = VerticalAlignment.Top;
            ellipse2.Width = ellipse2.Height = 0;
            ellipse2.Effect = new BlurEffect() { Radius = 20 };
            Grid grid = new Grid();
            grid.Children.Add(ellipse);
            grid.Children.Add(ellipse2);
            base.Child = grid;
            Shadow = Shadow;
        }

        private void MaterialCard_Loaded(object sender, RoutedEventArgs e)
        {
            if(Background is SolidColorBrush)
            {
                originBrush = (base.Background as SolidColorBrush).Clone();

            }
            ShadowSelfAdaption();
        }

        /// <summary>
        /// 阴影自适应
        /// </summary>
        private void ShadowSelfAdaption()
        {
            if (Effect == null && Shadow)
            {
                Effect = new DropShadowEffect() { BlurRadius = downRadius, Opacity = shadowOpacity, ShadowDepth = downDepth, Direction = shadowDireection };
            }
            if (Background != null && OpenShadowSelfAdaption)
            {
                Color c;
                if (Background is SolidColorBrush)
                {
                    c = (Background as SolidColorBrush).Color;
                }
                else if (Background is LinearGradientBrush)
                {
                    c = (Background as LinearGradientBrush).GradientStops[0].Color;
                }
                else
                {
                    return;
                }
                double opacity = 0.0004 * (765D - c.R - c.G - c.B) + 0.1;
                downRadius = 0.01 * (765D - c.R - c.G - c.B) + 3;
                if (Effect is DropShadowEffect)
                {
                    DropShadowEffect effect = (Effect as DropShadowEffect);
                    effect.Opacity = opacity;
                    effect.BlurRadius = downRadius;
                }
                Console.WriteLine(c + "," + downRadius + "," + opacity);
            }
        }
        public new Brush Background
        {
            get
            {
                return base.Background;
            }
            set
            {
                if (value is SolidColorBrush && base.Background is SolidColorBrush)
                {
                    SolidColorBrush background = base.Background as SolidColorBrush;
                    backgroundAnim.To = (value as SolidColorBrush).Color;
                    backgroundAnim.Completed += (s, e) => { ShadowSelfAdaption(); };
                    background.BeginAnimation(SolidColorBrush.ColorProperty, backgroundAnim);
                }
            }
        }
        private SolidColorBrush originBrush;

        private void BeginMouseEnterAnim()
        {
            if (Shadow && ShadowAnim) { BeginShadowAnimation(upDepth, downRadius * upRadius); }

            if (MouseEnterLeaveAnim)
            {
                Color c = new Color();
                ColorAnimation ca = new ColorAnimation()
                {
                    SpeedRatio = AnimationSpeed * 2,
                    EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 4 }
                };
                if (Background is SolidColorBrush)
                {
                    c = (Background as SolidColorBrush).Color;
                    
                    base.Background = new SolidColorBrush(new Color() { A = c.A, R = c.R, G = c.G, B = c.B });
                    ca.To = new Color() { A = (byte)(Math.Min(((byte)1.2 * c.A) + 20, 255)), R = (byte)(c.R * 0.95), B = (byte)(c.B * 0.95), G = (byte)(c.G * 0.95) };
                    (Background as SolidColorBrush).BeginAnimation(SolidColorBrush.ColorProperty, ca);
                }
                else if (BorderBrush is SolidColorBrush && Background==null)
                {
                    c = (BorderBrush as SolidColorBrush).Color;
                    ca.To = new Color() { A = (byte)(Math.Min(((byte)1.2 * c.A) + 20, 255)), R = (byte)(c.R * 0.95), B = (byte)(c.B * 0.95), G = (byte)(c.G * 0.95) };
                    Background = new SolidColorBrush();
                    (Background as SolidColorBrush).BeginAnimation(SolidColorBrush.ColorProperty, ca);
                }
                
                /*ellipse.Fill = new SolidColorBrush(new Color() { A = (byte)(Math.Min(((byte)1.2 * c.A) + 40, 255)), R = (byte)(c.R * 0.93), B = (byte)(c.B * 0.93), G = (byte)(c.G * 0.93) });

                ellipse.Width = ellipse.Height = Math.Max(ActualWidth, ActualHeight) * 1.6;
                //ellipseAnim.From = new Thickness(0, ActualHeight, 0, 0);
                ellipseAnim.To = new Thickness(0, -(Math.Max(ActualHeight, ActualWidth) * 0.8 - ActualHeight * 0.5), 0, 0);
                ellipse.BeginAnimation(MarginProperty, ellipseAnim);*/


            }

        }
        private void BeginMouseLeaveAnim()
        {
            if (Shadow && ShadowAnim) { BeginShadowAnimation(upDepth, downRadius); }
            if (MouseEnterLeaveAnim)
            {
                Color c = new Color();
                if (Background is SolidColorBrush)
                {
                    c = (Background as SolidColorBrush).Color;
                    //base.Background =new SolidColorBrush( new Color() {A=c.A,R=c.R,G=c.G,B=c.B });
                }
                else if (BorderBrush is SolidColorBrush)
                {
                    c = (BorderBrush as SolidColorBrush).Color;
                }
                else
                {
                    return;
                }
                /*ellipse.Fill = new SolidColorBrush(new Color() { A = (byte)(Math.Min(((byte)1.2 * c.A) + 40, 255)), R = (byte)(c.R * 0.93), B = (byte)(c.B * 0.93), G = (byte)(c.G * 0.93) });
                ellipse.Width = ellipse.Height = Math.Max(ActualWidth, ActualHeight) * 1.6;
                ellipseAnim.To = new Thickness(0, ActualHeight, 0, 0);
                //ellipseAnim.From = new Thickness(0, -(Math.Max( ActualHeight,ActualWidth)*0.8- ActualHeight * 0.5), 0, 0);
                ellipse.BeginAnimation(MarginProperty, ellipseAnim);*/

                ColorAnimation ca = new ColorAnimation()
                {
                    To = originBrush.Color,
                    SpeedRatio = AnimationSpeed * 2,
                    EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 4 }
                };

                (Background as SolidColorBrush).BeginAnimation(SolidColorBrush.ColorProperty, ca);
            }

        }
        private void BeginShadowAnimation(double depth, double radius)
        {
            depthAnim.To = depth;
            radiusAnim.To = radius;
            (Effect as DropShadowEffect).BeginAnimation(DropShadowEffect.ShadowDepthProperty, depthAnim);
            (Effect as DropShadowEffect).BeginAnimation(DropShadowEffect.BlurRadiusProperty, radiusAnim);
        }
        public void SetSizeWithOutAnim(double width, double height)
        {
            base.Height = height;
            base.Width = width;
        }
        public new Thickness Margin
        {
            get
            {
                return Margin;
            }
            set
            {
                marginAnim.To = value;
                UseLayoutRounding = false;
                BeginAnimation(MarginProperty, marginAnim);
            }
        }
        public new double Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                sizeAnim.To = value;
                UseLayoutRounding = false;
                BeginAnimation(HeightProperty, sizeAnim);
            }
        }
        public new double Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                sizeAnim.To = value;
                UseLayoutRounding = false;
                BeginAnimation(WidthProperty, sizeAnim);
            }
        }
        public new UIElement Child
        {
            get
            {
                if ((base.Child as Grid).Children.Count > 2)
                {
                    return (base.Child as Grid).Children[2];
                }
                return null;
            }
            set
            {
                if ((base.Child as Grid).Children.Count < 3)
                {
                    (base.Child as Grid).Children.Add(value);
                }
                else
                {
                    (base.Child as Grid).Children[2] = value;
                }
            }
        }
        public new double Opacity
        {
            get
            {
                return base.Opacity;
            }
            set
            {
                opacityAnim.To = value;
                BeginAnimation(OpacityProperty, opacityAnim);
            }
        }
        private void MaterialCard_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (WaveFeedback)
            {
                double radius = Math.Max(ActualHeight, ActualWidth) * 2;
                ellipse2SizeAnim.To = radius;
                Point p = e.GetPosition((IInputElement)sender);
                fixAnim.To = new Thickness(-radius / 2, -radius / 2, 0, 0);
                Color c = new Color();
                if (Background is SolidColorBrush)
                {
                    c = (Background as SolidColorBrush).Color;

                    SolidColorBrush background = (Background as SolidColorBrush);
                    originBrush = background.Clone();
                    Color c3 = originBrush.Color;
                    Color c2 = Color.FromArgb(255, (byte)(c.R * 0.96), (byte)(c.G * 0.96), (byte)(c.B * 0.96));
                    backgroundAnimUsingKey.KeyFrames.Clear();
                    backgroundAnimUsingKey.KeyFrames.Add(new EasingColorKeyFrame(c2) { EasingFunction = easeFunction });
                    backgroundAnimUsingKey.KeyFrames.Add(new EasingColorKeyFrame(c) { EasingFunction = easeFunction });
                    background = new SolidColorBrush(Color.FromArgb(c3.A, c3.R, c3.G, c3.B));//防冻结
                    background.BeginAnimation(SolidColorBrush.ColorProperty, backgroundAnimUsingKey);
                }
                else if (BorderBrush is SolidColorBrush)
                {
                    c = (BorderBrush as SolidColorBrush).Color;
                }

                ellipse2.Fill = new SolidColorBrush(new Color() { A = (byte)(Math.Min(((byte)1.2 * c.A), 255) + 40), R = (byte)(c.R * 0.8), B = (byte)(c.B * 0.8), G = (byte)(c.G * 0.8) });

                ellipse2.RenderTransform = new TranslateTransform(p.X, p.Y);
                ellipse2.BeginAnimation(WidthProperty, ellipse2SizeAnim);
                ellipse2.BeginAnimation(HeightProperty, ellipse2SizeAnim);
                ellipse2.BeginAnimation(OpacityProperty, ellipse2OpacityAnim);
                ellipse2.BeginAnimation(MarginProperty, fixAnim);

            }
        }
    }
}
