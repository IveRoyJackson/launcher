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

namespace Heyo.Controls
{
    /// <summary>
    /// TextBox.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxEx : System.Windows.Controls.TextBox
    {
        public TextBoxEx()
        {
            InitializeComponent();
            Password = "";
        }
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }
        public readonly static DependencyProperty PasswordProperty =
            DependencyProperty.Register(
            "Password",
            typeof(string),
            typeof(TextBoxEx)
        );
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public readonly static DependencyProperty TipForegroundBrushProperty =
            DependencyProperty.Register(
            "TipForegroundBrush",
            typeof(Brush),
            typeof(TextBoxEx)
        );
        public Brush TipForegroundBrush
        {
            get { return (Brush)GetValue(TipForegroundBrushProperty); }
            set {
                SetValue(TipForegroundBrushProperty, value);
                _hintTextColor = value;
            }
        }
        public readonly static DependencyProperty TipTextProperty =
            DependencyProperty.Register(
            "TipText",
            typeof(string),
            typeof(TextBoxEx)
        );
        public string TipText
        {
            get { return (string)GetValue(TipTextProperty); }
            set { SetValue(TipTextProperty, value); }
        }
        public bool OnlyNumber { get; set; }
        private void Init()
        {
            originForeground = Foreground.Clone();
            if (Foreground is SolidColorBrush)
            {
                Color color = (Foreground as SolidColorBrush).Color;
                //_hintTextColor = BorderBrush;//new SolidColorBrush(Color.FromArgb((byte)(color.A * 0.9), color.R, color.G, color.B));
            }
            else
            {
                //throw new Exception("暂时没做");
            }

            TextChanged += TextBoxEx_TextChanged;
            PreviewKeyDown += TextBoxEx_PreviewKeyDown;

            CaretBrush = Foreground;
            Foreground = _hintTextColor;
        }

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

        private Brush _hintTextColor=new SolidColorBrush( Colors.Gray);
        private Brush originForeground;

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
                        int select = SelectionStart;
                        string addedString = base.Text.Substring(item.Offset, item.AddedLength);
                        Password = Password.Insert(item.Offset, addedString);
                        base.Text = base.Text.Remove(item.Offset, 1).Insert(item.Offset, RandomString());
                        SelectionStart = select;
                    }
                    if (item.RemovedLength > 0 && Password.Length >= item.RemovedLength)
                    {
                        Password = Password.Remove(item.Offset, item.RemovedLength);
                    }
                }
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
                    //if(c<'0' c="">'9')//最好的方法,在下面测试数据中再加一个0，然后这种方法效率会高10毫秒左右
                    return false;
            }
            return true;
        }
    }
}
