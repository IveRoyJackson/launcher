using Ay.Framework.WPF.Controls;
using Heyo.Pages;
using Heyo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using Ay.Framework.WPF.Applications;
using Heyo.Class.Events;
using HeyoCraft.Pages;
using System.IO;
using Heyo.Class.Helper;

namespace HeyoCraft
{
    /// <summary>
    /// BaseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BaseWindow : AyWindow
    {
        public BaseWindow()
        {
            InitializeComponent();
            //WindowEntranceBackgroundMode = 2;
            BackgroundStretch = Stretch.UniformToFill;

            Loaded += Window_Loaded;
            SkinWindowMethodOverride = ShowSkinWindow;

        }
        MainWindow mainWindow = new MainWindow();
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /* 当鼠标左键点击窗体Border时，使背景动画暂停，并且开启窗体拖动 */
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //grainsBack.Stop();
                DragMove();
                //grainsBack.Start();
            }
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            if(SystemHelper.IsWindow10)
            {
                WindowPositionChangeWithAero();
            }
            else
            {
                WindowPositionChange();
            }
            base.OnLocationChanged(e);
        }
        //位置发生变化  
        void WindowPositionChange()
        {
            //Background="#99F0E600"
            mainWindow.Owner = this;
            mainWindow.Left = Left + 21;
            mainWindow.Top = Top + 46;
        }
        void WindowPositionChangeWithAero()
        {
            //Background="#99F0E600"
            mainWindow.Owner = this;
            mainWindow.Left = Left;// + 21;
            mainWindow.Top = Top + 26;// + 46;
        }
        public void SetBackgroundBlur(bool isBlur)
        {
            if (isBlur)
            {
                backOpPanel.To = 0;
            }
            else
            {
                backOpPanel.To = 1;
            }
            backOpPanel.Anim();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow.Closed += (s, ee) =>
            {
                try
                {
                    Close();
                }
                catch { }
            };
            string bgPath = Environment.CurrentDirectory + ConfigSetting.GetXmlValue("skinBg", "path");
            SetBackgroundFromName(bgPath);
            //(backOpPanel.Background as ImageBrush).ImageSource = new BitmapImage(new Uri(bgPath, UriKind.Absolute));

            if (SystemHelper.IsWindow10)//判断是否为win10
            {
                //CompositionTarget.Rendering += WindowPositionChangeWithAero;
                AeroHelper.EnableAero(this, true);
                Width = Width - ShadowMargin.Left - ShadowMargin.Right - 2;
                Height = Height - ShadowMargin.Top - ShadowMargin.Bottom - 2;
                mainGrid.Margin = new Thickness(-1, -25, -1, -1);
                ShadowMargin = new Thickness(0);

                backOpPanel.SetValue(Grid.RowSpanProperty, 1);
                backBlur.SetValue(Grid.RowSpanProperty, 1);
                backWhite.SetValue(Grid.RowSpanProperty, 1);
                WindowEntranceBackgroundMode = 5;
            }
            else
            {
                //CompositionTarget.Rendering += WindowPositionChange;
            }

            mainWindow.Show();
            OnLocationChanged(new EventArgs());
            mainGrid.Visibility = Visibility.Visible;
        }
        private void ShowSkinWindow()
        {
            SkinPage skinPage = new SkinPage();
            skinPage.BackgroundImageChanged += SkinPage_BackgroundImageChanged;
            mainWindow.ShowDialog(skinPage);
            //SetBackgroundFromName
            //SkinWindow skinWindow = new SkinWindow();
            //skinWindow.ShowDialog();
        }
        public void SetBackgroundFromName(string name)
        {
            SetBackgroundFromName(name, true);
            (backOpPanel.Background as ImageBrush).ImageSource = new BitmapImage(new Uri(name));
            backBlur.Source = name;
        }
        private void SkinPage_BackgroundImageChanged(object sender, StringEventArgs e)
        {
            SetBackgroundFromName(e.Source);
        }
    }
}
