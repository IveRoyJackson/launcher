using Heyo.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HeyoCraft.Controls
{
    public class ChoosableCard : MaterialCard
    {
        public ChoosableCard()
        {
            WaveFeedback = true;
            //InitializeComponent();
            MouseLeftButtonDown += (s, e) => { Choose(this); };
        }
        public bool Choosable { get; protected set; } = true;
        public CardList ParentList { get; set; }
        public delegate void OnChooseEventHandler(ChoosableCard card);
        public event OnChooseEventHandler OnChoose;
        private bool _isChosen = false;
        public bool IsChosen
        {
            get
            {
                return _isChosen;
            }
            set
            {
                if(value && !_isChosen)
                {
                    SetOnChoose();
                }else if(!value && _isChosen)
                {
                    CancelChoose();
                }
                _isChosen = value;

            }
        }
        /// <summary>
        /// 传出被选中的消息
        /// </summary>
        /// <param name="card">卡片</param>
        protected void Choose(ChoosableCard card)
        {
            OnChoose?.Invoke(card);
        }
        /// <summary>
        /// 从外部设置该卡片被选中
        /// </summary>
        public virtual void SetOnChoose() { }
        /// <summary>
        /// 从外部取消选中
        /// </summary>
        public virtual void CancelChoose() { }
    }
}
