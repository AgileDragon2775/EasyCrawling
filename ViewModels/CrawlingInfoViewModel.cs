using EasyCrawling.Models;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Documents;

namespace EasyCrawling.ViewModels
{
    public class CrawlingInfoViewModel : Base.ViewModelBase
    {
        #region Fields
                
        private CrawlingViewModel _crawling;
        private readonly CrawlingInfo _info;
        private List<Word> _otherCrawlingWordList;

        #endregion

        #region Constructors

        public CrawlingInfoViewModel()
        {
            _crawling = new CrawlingViewModel();
            _info = new CrawlingInfo();
            _otherCrawlingWordList = new List<Word>();
        }
        public CrawlingInfoViewModel(CrawlingViewModel crawling) : this()
        {
            _crawling = crawling;
            _info.CrawlingPointer = crawling.Crawling;
            _info.CrawlingName = crawling.Name;           

            foreach(Word word in _info.CrawlingPointer.WordList)
            {
                _otherCrawlingWordList.Add(new Word(word));
            }
        }

        #endregion

        #region Properties

        public string Name
        {
            get
            {
                _info.CrawlingName = _crawling.Name;
                return _info.CrawlingName;
            } 
        }

        public string Option
        {
            get => _info.UrlOption.Name;
            set
            {
                if (_info.UrlOption.Name != value)
                {
                    _info.UrlOption.Name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public WordPointer OptionWord
        {
            get => _info.UrlOption;
        }

        public List<Word> OtherCrawlingWordList
        {
            get => _otherCrawlingWordList;
            set => Set(ref _otherCrawlingWordList, value);
        }

        public Crawling Crawling
        {
            get => _info.CrawlingPointer;
        }

        public CrawlingInfo CrawlingInfo
        {
            get => _info;
        }

        #endregion

    }
}
