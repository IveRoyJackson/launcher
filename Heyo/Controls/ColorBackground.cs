using Heyo.Controls.Class;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Heyo.Controls
{
    public class ColorBackground : Canvas
    {
        private DispatcherTimer updateTimer = new DispatcherTimer();
        private DispatcherTimer addRemoveTimer = new DispatcherTimer();
        public int GrainsCount { get; set; } = 10;
        public double Acceleration { get; set; } = 6;
        public int MinSize { get; set; } = 200;
        public int MaxSize { get; set; } = 600;

        private RadialGradientBrush opacityMask
            = new RadialGradientBrush(
                new GradientStopCollection() {
                    new GradientStop(Color.FromArgb(180 , 0, 0, 0),0),
                    new GradientStop(Color.FromArgb(0, 0, 0, 0), 1),
                    new GradientStop(Color.FromArgb(150, 0, 0, 0), 0.2),
                    new GradientStop(Color.FromArgb(16, 0, 0, 0), 0.7),
                });
        public ColorBackground()
        {
            UseLayoutRounding = false;
            SnapsToDevicePixels = false;
            ClipToBounds = true;
            Background = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
            addRemoveTimer.Tick += AddRemoveTimer_Tick;
            addRemoveTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 *5);
            updateTimer.Tick += DrawingAY;
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 40);

            Loaded += GrainsBackground_Loaded;
        }

        private void AddRemoveTimer_Tick(object sender, EventArgs e)
        {
            int i = rand.Next(0, grains.Count);
            grains.RemoveAt(i);
            ellipses.RemoveAt(i);
            AddGrains();
        }

        public void Stop()
        {
            updateTimer.Stop();
            //addRemoveTimer.Stop();
        }
        public void Start()
        {
            updateTimer.Start();
            //addRemoveTimer.Start();
        }

        private void GrainsBackground_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //// 添加粒子
            //// x，y为粒子坐标，xa, ya为粒子xy轴加速度，max为连线的最大距离     
            for (int i = 0; i < GrainsCount; i++)
            {
                AddGrains();
            }
        }
        private void AddGrains()
        {
            GrainBase gb = new GrainBase();
            gb.X = rand.NextDouble() * ActualWidth;
            gb.Y = rand.NextDouble() * ActualHeight;
            gb.Xa = rand.NextDouble() * Acceleration - 1;
            gb.Ya = rand.NextDouble() * Acceleration - 1;
            gb.Size = rand.Next(MinSize, MaxSize);
            gb.Color = GetColor(rand.Next(0,6));
            Ellipse elip = new Ellipse();
            elip.OpacityMask = opacityMask;
            elip.Height = elip.Width = gb.Size;
            elip.Fill = new SolidColorBrush(gb.Color);
            ellipses.Add(elip);
            grains.Add(gb);
        }

        private void DrawingAY(object sender, EventArgs e)
        {
            Next();
        }


        List<GrainBase> grains = new List<GrainBase>();
        List<Ellipse> ellipses = new List<Ellipse>();
        Random rand = new Random();

        public void Next()
        {
            Children.Clear();

            for (int i = 0; i < grains.Count; i++)
            {
                GrainBase grain = grains[i];
                if (grain.X == null || grain.Y == null) continue;
                // 粒子位移
                grain.X += grain.Xa;
                grain.Y += grain.Ya;
                // 遇到边界将加速度反向
                grain.Xa *= (grain.X.Value > this.ActualWidth || grain.X.Value < 0) ? -1 : 1;
                grain.Ya *= (grain.Y.Value > this.ActualHeight || grain.Y.Value < 0) ? -1 : 1;
                // 绘制点
                Ellipse elip = ellipses[i];

                SetLeft(elip, grain.X.Value);
                SetTop(elip, grain.Y.Value);
                Children.Add(elip);
            }
        }
	static List<Color> colors = new List<Color>()
        {
            Colors.LightBlue,
            Colors.Gold,
            Colors.PaleGreen,
            Colors.LightCoral,
            Colors.HotPink,
            Colors.Plum
        };
        public static Color GetColor(int color)
        {
            return colors[color];
        }
    }
}
