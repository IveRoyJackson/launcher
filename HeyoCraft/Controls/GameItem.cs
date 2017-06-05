using Heyo.Class.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace HeyoCraft.Controls
{
    public class GameItem : UserControl
    {
        public Image icon;
        public TextBlock idText;
        public GameItem()
        {
            Width = Height = 128;
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(48) });
            icon = new Image() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Width = 64, Height = 64 ,Source=Properties.Resources.Minecraft.ToBitmapSource()};
            grid.Children.Add(icon);
            idText = new TextBlock() { MaxHeight = 42, MinHeight = 20, TextWrapping = TextWrapping.Wrap, FontSize = 14, TextAlignment = TextAlignment.Center, Margin = new Thickness(8, 0, 8, 4), VerticalAlignment = VerticalAlignment.Center,Text="text" };
            idText.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            idText.SetValue(Grid.RowProperty, 1);

            RelativeSource rs = new RelativeSource();
            rs.Mode = RelativeSourceMode.FindAncestor;
            rs.AncestorLevel = 1;
            rs.AncestorType = typeof(ListBoxItem);
            Binding binding = new Binding();
            binding.RelativeSource = rs;
            binding.Path = new PropertyPath("Foreground");
            idText.SetBinding(ForegroundProperty, binding);
            grid.Children.Add(idText);
            AddChild(grid);
        }
        public double IconWidth
        {
            get
            {
                return icon.Width;
            }
            set
            {
                icon.Width = value;
            }
        }
        public double IconHeigth
        {
            get
            {
                return icon.Height;
            }
            set
            {
                icon.Height = value;
            }
        }
    }
}
