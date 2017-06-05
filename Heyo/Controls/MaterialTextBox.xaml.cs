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

namespace Heyo.Controls
{
    /// <summary>
    /// TextBoxEx.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialTextBox : UserControl
    {
        private static PowerEase easeFaunction = new PowerEase() { EasingMode = EasingMode.EaseIn, Power = 4 };
        private const int AnimationSpeed = 6;
        private DoubleAnimation lineX1Anim = new DoubleAnimation() { EasingFunction = easeFaunction, SpeedRatio = AnimationSpeed };
        private DoubleAnimation lineX2Anim = new DoubleAnimation() { EasingFunction = easeFaunction, SpeedRatio = AnimationSpeed };
        private ThicknessAnimation tipAnim = new ThicknessAnimation() { EasingFunction = easeFaunction, SpeedRatio = AnimationSpeed };
        public event TextChangedEventHandler TextChanged;
        public MaterialTextBox()
        {
            InitializeComponent();
            
            textBox.GotFocus += (s, e) => { if (textBox.Text.Length < 1) { SetLineLength(5, ActualWidth); SetTipMargin(new Thickness(0, 0, 0, 25)); } };
            textBox.LostFocus += (s, e) => { if (textBox.Text.Length < 1) { SetLineLength(0, 0); SetTipMargin(new Thickness(5, 20, 0, 0)); } };

            textBox.TextChanged += TextBoxEx_TextChanged;
            textBox.PreviewKeyDown += TextBoxEx_PreviewKeyDown;
        }
        public object WarningContent
        {
            get
            {
                return warnLabel.Content;
            }
            set
            {
                warnLabel.Content = value;
            }
        }
        private void SetLineLength(double x1,double x2)
        {
            lineX1Anim.To = x1;
            lineX2Anim.To = x2;
            line.BeginAnimation(Line.X1Property, lineX1Anim);
            line.BeginAnimation(Line.X2Property, lineX2Anim);
        }
        private void SetTipMargin(Thickness margin)
        {
            tipAnim.To = margin;
            viewbox.BeginAnimation(MarginProperty, tipAnim);
        }
        public string Text
        {
            get
            {
                return textBox.Text;
            }
            set
            {
                textBox.Text = value;
                if (value.Length > 0)
                {
                    line.X1 = 5;
                    line.X2 = ActualWidth;
                    viewbox.Margin = new Thickness(0, 0, 0, 25);
                }
            }
        }
        public object Tip
        {
            get
            {
                return tipLabel.Content;
            }
            set
            {
                tipLabel.Content = value;
            }
        }
        public string TipText { get; set; }
        public bool OnlyNumber { get; set; }

        private void TextBoxEx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            keyDown = true;

            if (e.Key == Key.Space && (OnlyNumber || PasswordBox))
                e.Handled = true;

            if (PasswordBox)
            {
                if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    e.Handled = true;
                }
            }
        }

        bool keyDown = false;

        public bool PasswordBox { get; set; } = false;

        private void TextBoxEx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordBox && keyDown)
            {
                keyDown = false;
                var changes = e.Changes;
                foreach (var item in changes)
                {
                    if (item.AddedLength > 0)
                    {
                        int select = textBox.SelectionStart;
                        string addedString = textBox.Text.Substring(item.Offset, item.AddedLength);
                        Password = Password.Insert(item.Offset, addedString);
                        textBox.Text = textBox.Text.Remove(item.Offset, 1).Insert(item.Offset, RandomString());
                        textBox.SelectionStart = select;
                    }
                    if (item.RemovedLength > 0 && Password.Length >= item.RemovedLength)
                    {
                        Password = Password.Remove(item.Offset, item.RemovedLength);
                    }
                }
            }
            TextChanged?.Invoke(sender, e);
        }
        private string _password = "";
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                
                if(value.Length-_password.Length>1)
                {
                    Text = "";
                    for (int i = 0; i < value.Length; i++)
                    {
                        Text += RandomString();
                    }
                }
                _password = value;
            }
        }

        string RandomString()
        {
            Random r = new Random();
            return chars[r.Next(0, chars.Length - 1)];
        }
        public string[] chars = { "@", "#", "$", "%", "&" };

        private void textBox1_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!isNumberic(text))
                { e.CancelCommand(); }
            }
            else { e.CancelCommand(); }

            if (PasswordBox)
            {
                e.CancelCommand();
            }
        }



        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (OnlyNumber)
            {
                if (!isNumberic(e.Text))
                {
                    e.Handled = true;
                }
                else
                    e.Handled = false;
            }
        }

        public static bool isNumberic(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))
                    //if(c<'0' c="">'9')//最好的方法,在下面测试数据中再加一个0，然后这种方法效率会搞10毫秒左右
                    return false;
            }
            return true;
        }
    }
}
