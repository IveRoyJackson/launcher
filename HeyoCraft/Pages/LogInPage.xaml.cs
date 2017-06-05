using HeyoCraft.BackgroundWorkers;
using HeyoCraft.Class.Helper;
using Heyo.Pages;
using System;
using System.Windows;

namespace HeyoCraft.Pages
{
    /// <summary>
    /// LogInPage.xaml 的交互逻辑
    /// </summary>
    public partial class LogInPage : DialogPage
    {
        public LogInPage()
        {
            InitializeComponent();
            if (SettingProxy.RemPW)
            {
                mcPWBox.Password = SettingProxy.Password;
            }
            mcLogInBtn.Click += McLogInBtn_Click;
            Loaded += LogInPage_Loaded;
            autoLoginCheckBox.Checked += AutoLoginCheckBox_Checked;
            remPWCheckBox.Unchecked += RemPWCheckBox_Unchecked;
        }

        private void RemPWCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            autoLoginCheckBox.IsChecked = false;
        }

        private void AutoLoginCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            remPWCheckBox.IsChecked = true;
        }

        private void LogInPage_Loaded(object sender, EventArgs e)
        {
            remPWCheckBox.IsChecked = SettingProxy.RemPW;
            autoLoginCheckBox.IsChecked = SettingProxy.AutoLogIn;
        }

        private void McLogInBtn_Click(object sender, RoutedEventArgs e)
        {
            YggdrasilWorker yw = new YggdrasilWorker((s, ee, u) =>
            {
                if (ee == Reference.ExchangeCode.SUCCESSFUL)
                {
                    SettingProxy.AutoLogIn = autoLoginCheckBox.IsChecked == true;
                    SettingProxy.RemPW = remPWCheckBox.IsChecked == true;
                    SettingProxy.MCID = mcIDBox.Text;
                    SettingProxy.Password = mcPWBox.Password;

                    SettingProxy.Name = u.Name;
                    SettingProxy.User = u;
                    SettingProxy.IsLogedIn = true;
                    OutputResult(this, u);
                    Close();
                }
                else
                {
                    autoLoginCheckBox.IsChecked = SettingProxy.AutoLogIn = false;
                    remPWCheckBox.IsChecked =  SettingProxy.RemPW = false;
                    Ay.Framework.WPF.AyMessageBox.Show("403", "登录失败");
                }
            }, mcIDBox.Text, mcPWBox.Password);
            yw.RunWorkerAsync();
        }
    }
}
