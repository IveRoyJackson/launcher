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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeyoCraft.Pages
{
    /// <summary>
    /// YesOrNoPage.xaml 的交互逻辑
    /// </summary>
    public partial class YesOrNoPage : DialogPage
    {
        public YesOrNoPage(string title,object content,string detailText=null)
        {
            InitializeComponent();
            ContentLabel.Content = content;
            Title = title;
            if(detailText?.Length>0)
            {
                DetailTextBlock.Visibility = Visibility.Visible;
                DetailTextBlock.Text = detailText;
            }
        }
        private void NoButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
            OutputResult(this, false);
            
        }

        private void YesButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
            OutputResult(this, true);
            
        }
    }
}
