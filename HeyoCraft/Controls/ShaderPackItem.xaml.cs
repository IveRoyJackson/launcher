using HeyoCraft.Class.Data;

namespace HeyoCraft.Controls
{
    /// <summary>
    /// MinecraftItem.xaml 的交互逻辑
    /// </summary>
    public partial class ShaderDataItem : GameItem
    {
        public ShaderDataItem()
        {
            InitializeComponent();
        }
        ShaderPackData _pata;
        public ShaderPackData Data
        {
            get
            {
                return _pata;
            }
            set
            {
                _pata = value;
                idText.Text = value.Name;
                //icon.Source = new BitmapImage(new Uri(value.AbsolutelyFilePath));
            }
        }
    }
}
