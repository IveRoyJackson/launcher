using Heyo.Pages;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Heyo
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogPage Page { get; set; }
        public DialogWindow(DialogPage page, Window owner=null)
        {
            InitializeComponent();
            Owner = owner;
            DialogFrame.Navigated += DialogFrame_Navigated;
            Loaded += DialogWindow_Loaded;
            CloseDialogButton.MouseDown += (s,e) => { Close(); };
            page.Closed += (s, e) => { Close(); };
            Page = page;
        }
        public new event EventHandler Closed;
        public new void Close()
        {
            Closed?.Invoke(this, new EventArgs());
            DoubleAnimation d = new DoubleAnimation();
            d.To = 0;
            PowerEase pe = new PowerEase();
            pe.EasingMode = EasingMode.EaseInOut;
            pe.Power = 4;
            d.EasingFunction = pe;
            d.SpeedRatio = SpeedRatio;
            d.Completed += (s, e) => { base.Close(); };
            BeginAnimation(OpacityProperty, d);
        }
        public double SpeedRatio { get; set; } = 1.4;
        private void DialogFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            DialogPage page = e.Content as DialogPage;

            if (page?.ShowBackButton==true)
            {
                DialogTitle.Visibility = Visibility.Visible;
                //BackButton.Visibility = Visibility.Visible;
            }
            if (page?.Title?.Length > 0)
            {
                DialogTitle.Visibility = Visibility.Visible;
                DialogTitleLabel.Content = page.Title;
            }
            if (page?.ShowCloseButton==true)
            {
                DialogTitle.Visibility = Visibility.Visible;
                CloseDialogButton.Visibility = Visibility.Visible;
            }
            //Width = page.Width;
            //Height = page.Height;
            UpdateLayout();
            if(Owner!=null)
            {
                Left = Owner.Left + Owner.Width / 2 - ActualWidth / 2;
                Top = Owner.Top + Owner.Height / 2 - ActualHeight / 2;
            }

        }


        private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DialogFrame.Navigate(Page);
            Window window = sender as Window;
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = 1;
            PowerEase pe = new PowerEase();
            pe.EasingMode = EasingMode.EaseInOut;
            pe.Power = 4;
            d.SpeedRatio = SpeedRatio;
            d.EasingFunction = pe;
            d.Completed += (s, ee) => { Page.LoadCompleted(this, new EventArgs()); };
            window.BeginAnimation(OpacityProperty, d);
        }

        private void CloseDialogButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Page.Close();
        }

        private void DialogTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
