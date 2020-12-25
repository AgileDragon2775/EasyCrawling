using EasyCrawling.Models;

namespace EasyCrawling.ViewModels
{
    public class NameViewModel : Base.ViewModelBase
    {
        protected WordPointer wordPointer = new WordPointer();

        public string Title
        {
            get => wordPointer.Name;
            set
            {
                if (wordPointer.Name != value)
                {
                    wordPointer.Name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Example
        {
            get => wordPointer.Example;
        }

        public Word Word
        {
            get => wordPointer.Pointer;
            set
            {
                wordPointer.Pointer = value;
                RaisePropertyChanged("Example");
            }
        }

        public NameViewModel()
        {
            wordPointer = new WordPointer();
        }

        public NameViewModel(WordPointer inWord)
        {
            wordPointer = inWord;
        }
    }
}
