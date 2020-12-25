using EasyCrawling.Enums;

namespace EasyCrawling.Models
{
    [System.Serializable]
    public class WordOption
    {
        private string leftWord { get; set; }
        private string rightWord { get; set; }
        public int LeftNum { get; set; }
        public int RightNum { get; set; }
        public int Order { get; set; }
        public EncodingOptionType Type { get; set; }

        public WordOption() { }
        public WordOption(WordOption option)
        {
            leftWord = option.leftWord;
            rightWord = option.rightWord;
            LeftNum = option.LeftNum;
            RightNum = option.RightNum;
            Order = option.Order;
            Type = option.Type;
        }

        public string LeftWord
        {
            get
            {
                return leftWord;
            }
            set
            {
                leftWord = value;
                int outIndex;
                int.TryParse(value, out outIndex);
                LeftNum = outIndex;
            }
        }

        public string RightWord
        {
            get
            {
                return rightWord;
            }
            set
            {
                rightWord = value;
                int outIndex;
                int.TryParse(value, out outIndex);
                RightNum = outIndex;
            }
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            var item = obj as WordOption;
            if (item == null) 
            {
                return false;
            }

            if(ReferenceEquals(obj,this))
            {
                return true;
            }

            if ((LeftWord != item.LeftWord) ||
                (RightWord != item.RightWord) ||
                (LeftNum != item.LeftNum) ||
                (RightNum != item.RightNum) ||
                (Order != item.Order) ||                
                (Type != item.Type))
                return false;

            return true;
        }
    }
}