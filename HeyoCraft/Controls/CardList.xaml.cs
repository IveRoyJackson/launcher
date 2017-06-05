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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeyoCraft.Controls
{
    /// <summary>
    /// CardList.xaml 的交互逻辑
    /// </summary>
    public partial class CardList : UserControl
    {
        List<ChoosableCard> _cards = new List<ChoosableCard>();
        public List<ChoosableCard> Cards
        {
            get
            {
                return _cards;
            }
            set
            {
                foreach (var item in value)
                {
                    AddCard(item);
                }
            }
        }
        ThicknessAnimation d = new ThicknessAnimation();
        public bool ShowMessages { get; set; } = false;

        public CardList()
        {
            InitializeComponent();
        }
        private ChoosableCard _chosenCard;
        public ChoosableCard ChosenCard
        {
            get
            {
                return _chosenCard;
            }
            set
            {

                if (_chosenCard != null)
                    _chosenCard.CancelChoose();

                value.SetOnChoose();
                _chosenCard = value;
            }
        }
        public int SelectedIndex
        {
            get
            {
                return list.SelectedIndex;
            }
            set
            {
                int index = value > list.Items.Count-1 ? list.Items.Count-1 : value;
                if (index == -1)
                    return;
                Card_OnChoose((list.Items[index] as ChoosableCard));
                ChosenCard.SetOnChoose();
                list.SelectedIndex = index;
            }
        }
        public int VisualCardsCount { get; set; }
        public void AddCard(ChoosableCard card)
        {
            if (!Cards.Contains(card))
            {
                Cards.Add(card);
                card.OnChoose += Card_OnChoose;

                list.Items.Add(card);
                UpdateLayout();
            }
        }

        public void RemoveCard(ChoosableCard card)
        {
            Cards.Remove(card);

            list.Items.Remove(card);

            UpdateLayout();
        }
        public event EventHandler OnSelectedChange;
        private void Card_OnChoose(ChoosableCard card)
        {
            ChosenCard = card as ChoosableCard;
            list.SelectedItem = card;
            OnSelectedChange?.Invoke(this, new EventArgs());
        }
    }
}
