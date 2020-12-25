using EasyCrawling.Models;
using System.Collections.ObjectModel;

namespace EasyCrawling.ViewModels
{
    public class CustomWordViewModel : Base.ViewModelBase
    {
        #region Fields

        private readonly Word _word;

        #endregion

        #region Constructors
        public CustomWordViewModel() 
        {
            _word = new Word(); 
        }   

        public CustomWordViewModel(Word word)
        {
            _word = word;
        }

        #endregion

        #region Properties
        public Word Word
        {
            get => _word;
        }

        public bool IsIndex
        {
            get => Word.IsIndex;
            set
            {
                if (Word.IsIndex != value)
                {
                    Word.IsIndex = value;
                    RaisePropertyChanged();
                }
            }
        }

        public NodeTag Tag
        {
            get => Word.Tag;
            set
            {
                if (Word.Tag != value)
                {
                    Word.Tag = value;
                    RaisePropertyChanged();
                }
            }
        }  

        public uint Numbering
        {
            get => Word.Numbering;
            set
            {
                if (Word.Numbering != value)
                {
                    Word.Numbering = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Name
        {
            get => Word.Name;
            set
            {
                if (Word.Name != value)
                {
                    Word.Name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string EncodedExample
        {
            get => Word.Encoded;
            set
            {
                if(Word.Encoded != value)
                {
                    Word.Encoded = value;                    
                }
                RaisePropertyChanged();
            }
        }

        public string OriginalExample
        {
            get => Word.Original;
            set
            {
                if (Word.Original != value)
                {
                    Word.Original = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<WordOption> Options
        {
            get => new ObservableCollection<WordOption>(Word.Options);

        }

        #endregion
    }
}
