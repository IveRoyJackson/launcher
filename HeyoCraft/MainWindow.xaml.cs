using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Heyo.Class.Helper;
using Heyo.Pages;
using Heyo;
using Heyo.Controls;
using HeyoCraft.Class.Data;
using HeyoCraft.Class;
using HeyoCraft.Class.Helper;
using HeyoCraft.BackgroundWorkers;
using HeyoCraft.Controls;
using HeyoCraft.Pages;
using Ay.Framework.WPF;
using System;
using System.IO;
using Heyo.Class;
using System.Windows.Media;

namespace HeyoCraft
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Minecraft> games = new List<Minecraft>();
        public AsyncWorker AsyncWorker { get; set; } = new AsyncWorker();
        public MainWindow()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            //new Window1().Show();
            AsyncWorker.Dispatcher = Dispatcher;

            //RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.LowQuality);
            runBtn.Click += RunBtn_Click;
            CleanMemoryButton.Click += (s, e) =>
            {
                MemoryHelper.CleanAllMemory();
                memorySlider.Maximum = MemoryHelper.GetAvailablePhysicalMemory();
            };

            //如果要启用在游戏库存中“游戏”选项卡的游戏数量标记功能，请解除注释。
            //if(games.Count>0)
            //   gamelist_Button.Content += string.Format(" ({0})",games.Count);

            pageMovePanel.AnimationSpeed = 4;
            LogInPage lg = new LogInPage();
            lg.Result += Lg_Result;
            logInEntrance.MouseDown += (s, e) => { ShowDialog(lg); };
            logOutBtn.MouseDown += LogOutBtn_MouseDown;
            homeRadio.Click += HomeRadio_Click;
            libRadio.Click += LibRadio_Click;
            setRadio.Click += SetRadio_Click;
            helpRadio.Click += HelpRadio_Click;
            libRadioGame.Click += (s, e) => { LibPageTurnTo(0); };
            libRadioSave.Click += (s, e) => { LibPageTurnTo(1); };
            libRadioMods.Click += (s, e) => { LibPageTurnTo(2); };
            libRadioResource.Click += (s, e) => { LibPageTurnTo(3); };
            libRadioEffect.Click += (s, e) => { LibPageTurnTo(4); };
            libRadioPhoto.Click += (s, e) => { LibPageTurnTo(5); };
            //throw new Exception("快生成");
        }
        private void LogOutBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            nameBox.IsReadOnly = false;
            SettingProxy.IsLogedIn = false;
            SettingProxy.User = new User();
            UserAreaLogIn.Visibility = Visibility.Collapsed;
            UserAreaLogOut.Visibility = Visibility.Visible;
        }

        private void Lg_Result(DialogPage sender, object result)
        {
            User u = (User)result;
            SettingProxy.Name = u.Name;
            SettingProxy.User = u;
            SettingProxy.IsLogedIn = true;
            nameBox.Text = u.Name;
            nameBox.IsReadOnly = true;
            UserAreaLogIn.Visibility = Visibility.Visible;
            UserAreaLogOut.Visibility = Visibility.Collapsed;

        }

        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {
            /* 启动游戏 */
            SettingProxy.User.Name = nameBox.Text.Length < 1 ? "Player" : nameBox.Text;
            SettingProxy.Name = SettingProxy.User.Name;
            if (javaComboBox.Items.Count == 0)
            {
                MessageBox.Show("请安装JAVA，如果你装了，请再装一次←_←");
            }
            else if (javaComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("请选择JAVA");
            }
            else if (gameList.Items.Count == 0)
            {
                MessageBox.Show("没有Minecraft");
            }
            else if (gameList.SelectedIndex < 0)
            {
                MessageBox.Show("请选择游戏");
            }
            else
            {
                Launcher launcher = new Launcher((gameList.SelectedItem as MinecraftItem).Game, SettingProxy.Javas[javaComboBox.SelectedItem.ToString()], int.Parse(memoryTextBox.Text), SettingProxy.User);
                launcher.Launch();
                Close();
            }
        }


        /* 实现点击一次打开页面，再点一次关闭页面的功能*/
        private int lastChecked = 0;

        private void HomeRadio_Click(object sender, RoutedEventArgs e)
        {
            if (lastChecked == 1)
            {
                PageTurnTo(0);
                lastChecked = 0;
                (e.Source as RadioButton).IsChecked = false;
            }
            else
            {
                PageTurnTo(1);
                lastChecked = 1;
            }


        }

        private void LibRadio_Click(object sender, RoutedEventArgs e)
        {
            if (lastChecked == 2)
            {
                PageTurnTo(0);
                lastChecked = 0;
                (e.Source as RadioButton).IsChecked = false;
            }
            else
            {
                PageTurnTo(2);
                lastChecked = 2;
            }


        }
        private void SetRadio_Click(object sender, RoutedEventArgs e)
        {
            if (lastChecked == 3)
            {
                PageTurnTo(0);
                lastChecked = 0;
                (e.Source as RadioButton).IsChecked = false;
            }
            else
            {
                PageTurnTo(3);
                lastChecked = 3;
            }
        }
        private void HelpRadio_Click(object sender, RoutedEventArgs e)
        {
            if (lastChecked == 4)
            {
                PageTurnTo(0);
                lastChecked = 0;
                (e.Source as RadioButton).IsChecked = false;
            }
            else
            {
                PageTurnTo(4);
                lastChecked = 4;
            }
        }
        /// <summary>
        /// 移动到指定页面(0为无页面)
        /// </summary>
        /// <param name="to"></param>
        private void PageTurnTo(int to)
        {
            /* pageHost内的内容全部隐藏*/
            foreach (UIElement item in pageHost.Children)
            {
                item.Visibility = Visibility.Collapsed;
            }
            /* 将要显示的页面设置为可见 */
            pageHost.Children[to].Visibility = Visibility.Visible;

            /* 播放淡入动画 */
            pageOpacityPanel.Anim();
            /* 将页面上移以作为切换动画*/
            pageMovePanel.MoveTo(new Point(0, 14), new Point(0, 0), null);
            (Owner as BaseWindow).SetBackgroundBlur(to != 0);
        }
        /// <summary>
        /// 库存内容 移动到指定页面
        /// </summary>
        /// <param name="to"></param>
        private void LibPageTurnTo(int to)
        {
            /* pageHost内的内容全部隐藏*/
            foreach (UIElement item in libPageHost.Children)
            {
                item.Visibility = Visibility.Collapsed;
            }
            /* 将要显示的页面设置为可见 */
            libPageHost.Children[to].Visibility = Visibility.Visible;

            /* 播放淡入动画 */
            libPageOpacityPanel.Anim();
            /* 将页面上移以作为切换动画*/
            libPageMovePanel.MoveTo(new Point(0, 14), new Point(0, 0), null);

        }

        private void AyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateHelper.StartCheckUpdate((s2, e2) =>
            {
                YesOrNoPage yon = new YesOrNoPage("发现更新", "是否开始更新？", e2.Source);
                yon.Result += (s3, e3) =>
                {
                    if ((bool)e3)
                    {
                        UpdateHelper.UpdateApp();

                    }
                };
                ShowDialog(yon);
            });

            /*if(!File.Exists(ConstantLibrary.HomePageFilePath))
            {
                AsyncWorker.InvokeAsync(DownloadLoadHomePage, UpdateLoadHomePage);
            }
            else
            {
                UpdateLoadHomePage();
            }*/

            //AsyncWorker.InvokeAsync(DownloadLoadHomePage, UpdateLoadHomePage);
            /* 程序启动时开始播放背景动画 */
            //grainsBack.Start();
            if (SettingProxy.AutoLogIn)
            {
                YggdrasilWorker yw = new YggdrasilWorker((s, ee, u) =>
                {
                    if (ee == HeyoCraft.Reference.ExchangeCode.SUCCESSFUL)
                    {
                        Lg_Result(null, u);
                    }
                    else
                    {
                        Ay.Framework.WPF.AyMessageBox.Show("403", "登录失败");
                    }
                }, SettingProxy.MCID, SettingProxy.Password);
                yw.RunWorkerAsync();
            }


            games = SettingProxy.Minecrafts;
            foreach (var item in games)
            {
                gameList.Items.Add(new MinecraftItem() { Game = item });
            }
            foreach (var item in SettingProxy.Mods)
            {
                modList.Items.Add(new ModItem() { Data = item });
            }
            foreach (var item in SettingProxy.ShaderPack)
            {
                sharderList.Items.Add(new Controls.ShaderDataItem() { Data = item });
            }
            foreach (var item in SettingProxy.Screenshot)
            {
                photoList.Items.Add(new ScreenshotItem() { Data = item });
            }
            foreach (var item in SettingProxy.ResPack)
            {
                resList.Items.Add(new ResPackItem() { Data = item });
            }
            foreach (var item in SettingProxy.Saves)
            {
                saveList.Items.Add(new SaveItem() { Data = item });
            }
            foreach (var item in SettingProxy.Javas.Keys)
            {
                javaComboBox.Items.Add(item);
            }

            LibPageTurnTo(0);
            memorySlider.Minimum = 512;
            memorySlider.Maximum = MemoryHelper.GetAvailablePhysicalMemory();
        }

        public void ShowDialog(DialogPage page)
        {
            pageBlurPanel.To = 10;
            pageBlurPanel.Anim();
            page.Closed += (s, e) =>
            {
                pageBlurPanel.To = 0;
                pageBlurPanel.Anim();
            };
            new DialogWindow(page, this).Show();

        }
        private void DownloadLoadHomePage()
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            wc.DownloadFile(ConstantLibrary.HomePageUri, ConstantLibrary.HomePageFilePath);
        }
        private void UpdateLoadHomePage()
        {
            if(File.Exists(ConstantLibrary.HomePageFilePath))
            {
                homePage.Navigate(new Uri(ConstantLibrary.HomePageFilePath));

            }
            else
            {
                StackPanel sp = new StackPanel() { Margin = new Thickness(32), VerticalAlignment = VerticalAlignment.Center };
                sp.Children.Add(new Label() { Content = "404", FontSize = 72 });
                homePage.Navigate(sp);
            }
        }
    }
}
