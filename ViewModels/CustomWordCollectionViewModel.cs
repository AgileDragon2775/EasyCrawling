using EasyCrawling.Models;
using System.Collections.ObjectModel;
using EasyCrawling.Helpers;
using System;
using EasyCrawling.Enums;
using HtmlAgilityPack;
using System.Linq;
using System.Windows;

namespace EasyCrawling.ViewModels
{
    public class CustomWordCollectionViewModel : Base.ViewModelBase
    {

        #region Fields

        private ObservableCollection<CustomWordViewModel> _wordList;
        private ObservableCollection<CrawlingInfoViewModel> _otherCrawlingList;
        readonly ObservableCollection<CrawlingViewModel> _crawlingList;

        private Crawling nowCrawling;
        private CustomWordViewModel _selectedWord;
        private Word _indexWord;
        private CrawlingInfoViewModel _selectedCrawling;
        private bool _isVisibleOtherCrawling;
        private bool _isVisibleExcept;

        private RelayCommand _indexWordChangedCommand;
        private RelayCommand _deleteWordCommand;
        private RelayCommand _deleteOptionCommand;
        private RelayCommand _addWordCommand;
        private RelayCommand _addOptionCommand;
        private RelayCommand _optionChangedCommand;
        private RelayCommand _addOtherCrawling;
        private RelayCommand _otherCrawlingOptionChangedCommand;
        private RelayCommand _testCrawlingCommand;
        private RelayCommand _deleteCrawlingCommand;
        private RelayCommand _refreshCommand;
        private RelayCommand _switchOtherCrawlingCommand;
        private RelayCommand _switchExceptCommand;
        private RelayCommand _addExceptCommand;
        private RelayCommand _deleteExceptCommand;
        

        #endregion

        #region Event

        public event EventHandler WordChanged;
        public event EventHandler WordAdded;
        public event EventHandler OtherCrawlingChanged;

        #endregion

        #region Constructors

        public CustomWordCollectionViewModel() { }

        public CustomWordCollectionViewModel(Crawling nowCrawling, ObservableCollection<CrawlingViewModel> crawlingList)
            : this()
        {
            this.nowCrawling = nowCrawling;
            this._crawlingList = crawlingList;
        }

        #endregion

        #region Properties

        public ObservableCollection<CustomWordViewModel> WordList
        {
            get => Get(ref _wordList);
            set => Set(ref _wordList, value);
        }

        public ObservableCollection<CustomWordViewModel> WordUnionList
        {
            get
            {
                var nowList = _wordList.Select(x => x).ToList();
                nowList.AddRange(OtherCrawlingList
                    .SelectMany(x => x.OtherCrawlingWordList)
                    .Select(c => new CustomWordViewModel(c)));
                return new ObservableCollection<CustomWordViewModel>(nowList);
            }
            set
            {
                if (_wordList != value)
                {
                    _wordList = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<CrawlingViewModel> CrawlingList
        {
            get => _crawlingList;
        }

        public ObservableCollection<CrawlingInfoViewModel> OtherCrawlingList
        {
            get => Get(ref _otherCrawlingList);
            set => Set(ref _otherCrawlingList, value);
        }

        public ObservableCollection<ExcpetWord> ExceptList
        {
            get => new ObservableCollection<ExcpetWord>(SelectedWord.Word.Excepts);           
        }

        public CustomWordViewModel SelectedWord
        {
            get => Get(ref _selectedWord);
            set
            {
                Set(ref _selectedWord, value);
                RaisePropertyChanged("ExceptList");
                RaisePropertyChanged("OptionList");
                RaisePropertyChanged("ExampleText");
            }
        }
        public Word IndexWord
        {
            get => Get(ref _indexWord);
            set => Set(ref _indexWord, value);
        }

        public ObservableCollection<WordOption> OptionList
        {
            get => SelectedWord.Options;
        }

        public string ExampleText
        {
            get => SelectedWord.EncodedExample;
        }
      
        public string OtherUrlOptionExample
        {
            get => SelectedCrawling.OptionWord.ToString();
        }

        public CrawlingInfoViewModel SelectedCrawling
        {
            get => Get(ref _selectedCrawling);
            set => Set(ref _selectedCrawling, value);
        }

        public Visibility IsVisibleOtherCrawling
        {
            get => _isVisibleOtherCrawling ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility IsVisibleExcept
        {
            get => _isVisibleExcept ? Visibility.Visible : Visibility.Collapsed;
        }
        
        #endregion

        #region Command
        public RelayCommand IndexWordChangedCommand
        {
            get => _indexWordChangedCommand ??
                    (_indexWordChangedCommand = new RelayCommand(OnChangeIndexWord));
            set => Set(ref _indexWordChangedCommand, value);
        }
        public RelayCommand DeleteWordCommand
        {
            get => _deleteWordCommand ??
                    (_deleteWordCommand = new RelayCommand(OnDeleteWord, CanDeleteWord));
            set => Set(ref _deleteWordCommand, value);
        }

        public RelayCommand DeleteOptionCommand
        {
            get => _deleteOptionCommand ??
                    (_deleteOptionCommand = new RelayCommand(OnDeleteOption));
            set => Set(ref _deleteOptionCommand, value);
        }
        public RelayCommand AddWordCommand
        {
            get => _addWordCommand ??
                    (_addWordCommand = new RelayCommand(OnAddWord));
            set => Set(ref _addWordCommand, value);
        }
        public RelayCommand AddOptionCommand
        {
            get => _addOptionCommand ??
                    (_addOptionCommand = new RelayCommand(OnAddOption, CanAddOption));
            set => Set(ref _addOptionCommand, value);
        }

        public RelayCommand OptionChangedCommand
        {
            get => _optionChangedCommand ??
                    (_optionChangedCommand = new RelayCommand(OnOptionChanged));
            set => Set(ref _optionChangedCommand, value);
        }

        public RelayCommand AddOtherCrawling
        {
            get => _addOtherCrawling ??
                    (_addOtherCrawling = new RelayCommand(OnAddOtherCrawling, IsOtherCrawling));
            set => Set(ref _addOtherCrawling, value);
        }

        public RelayCommand OtherCrawlingOptionChangedCommand
        {
            get => _otherCrawlingOptionChangedCommand ??
                    (_otherCrawlingOptionChangedCommand = new RelayCommand(OnOtherCrawlingOptionChanged));
            set => Set(ref _otherCrawlingOptionChangedCommand, value);
        }

        public RelayCommand TestCrawlingCommand
        {
            get => _testCrawlingCommand ??
                    (_testCrawlingCommand = new RelayCommand(OnTestCrawling));
            set => Set(ref _testCrawlingCommand, value);
        }

        public RelayCommand DeleteCrawlingCommand
        {
            get => _deleteCrawlingCommand ??
                    (_deleteCrawlingCommand = new RelayCommand(OnDeleteCrawling));
            set => Set(ref _deleteCrawlingCommand, value);
        }

        public RelayCommand RefershCommand
        {
            get => _refreshCommand ??
                    (_refreshCommand = new RelayCommand(OnRefresh));
            set => Set(ref _refreshCommand, value);
        }

        public RelayCommand SwitchOtherCrawlingCommand
        {
            get => _switchOtherCrawlingCommand ??
                    (_switchOtherCrawlingCommand = new RelayCommand(OnSwitchOtherCrawling));
            set => Set(ref _switchOtherCrawlingCommand, value);
        }

        public RelayCommand SwitchExceptCommand
        {
            get => _switchExceptCommand ??
                    (_switchExceptCommand = new RelayCommand(OnSwitchExcept));
            set => Set(ref _switchExceptCommand, value);
        }
        public RelayCommand AddExceptCommand
        {
            get => _addExceptCommand ??
                    (_addExceptCommand = new RelayCommand(OnAddExcept));
            set => Set(ref _addExceptCommand, value);
        }

        public RelayCommand DeleteExceptCommand
        {
            get => _deleteExceptCommand ??
                    (_deleteExceptCommand = new RelayCommand(OnDeleteExcept));
            set => Set(ref _deleteExceptCommand, value);
        }
               
        #endregion

        #region method

        public void RefreshWords(HtmlDocument html, string xPath)
        {
            UpdateOriginals(html, xPath);
            UpdateEncodeds();

            RaisePropertyChanged("ExampleText");
        }

        private void RefreshOrder(ObservableCollection<WordOption> options)
        {
            int order = 1;
            foreach (WordOption option in options)
            {
                option.Order = order++;
            }
        }

        private string SearchNotUsingName(string baseName, System.Collections.Generic.List<string> targetList)
        {
            int index = 1;

            while (targetList.Contains(baseName+index))
            {
                index++;
            }

            return baseName + index;
        }

        public void AddOriginalWord(string attr, NodeTag nodeTag, string text)
        {
            _wordList.Add(new CustomWordViewModel()
            {
                Numbering = uint.Parse(SearchNotUsingName("", _wordList.Select(w => w.Numbering.ToString()).ToList())),
                Name = SearchNotUsingName(attr, _wordList.Select(w=>w.Name).ToList()),
                Tag = nodeTag,
                OriginalExample = text,
                EncodedExample = text
            });

            WordAdded(null, null);
            WordChanged(null, null);
            RaisePropertyChanged("WordUnionList");
        }

        private void UpdateEncoded(CustomWordViewModel word)
        {
            word.EncodedExample = EncodingWordHelper.GetEncodedWord(
                word.Word,
                _wordList.Select(x => x.Word).ToList());
        }

        private void UpdateEncodeds()
        {
            foreach (CustomWordViewModel word in _wordList)
            {
                UpdateEncoded(word);
            }
        }

        private void UpdateOriginal(CustomWordViewModel word, HtmlNode node, NodeTag tag)
        {
            word.OriginalExample = CrawlingHelper.GetIfExist(node, tag);
        }

        private void UpdateOriginals(HtmlDocument html, string xPath)
        {
            HtmlNodeCollection nodes = CrawlingHelper.GetResults(html, xPath);

            if (nodes == null || nodes.Count < 1) return;

            foreach (CustomWordViewModel word in WordList)
            {
                UpdateOriginal(word, nodes[0], word.Tag);
            }
        }

        #endregion

        #region ExcuteCommand

        private void OnChangeIndexWord(object param)
        {
            IndexWord = param as Word;

            WordChanged(null, null);
        }

        private void OnAddWord(object param)
        {
            AddOriginalWord("new", new NodeTag(), "");

            RaisePropertyChanged("ExampleText");
        }

        private void OnDeleteWord(object param)
        {
            var now = param as CustomWordViewModel;
            now.Tag = null;

            _wordList.Remove(now);
            WordChanged(null, null);
            UpdateEncodeds();
            RaisePropertyChanged("WordUnionList");
        }

        private void OnAddOption(object param)
        {
            WordOption option = new WordOption
            {
                Type = (EncodingOptionType)param,
                Order = SelectedWord.Options.Count + 1
            };
            SelectedWord.Word.Options?.Add(option);

            WordChanged(null, null);
            RaisePropertyChanged("OptionList");
        }

        private void OnDeleteOption(object param)
        {
            SelectedWord.Word.Options.Remove(param as WordOption);
            WordChanged(null, null);
            UpdateEncoded(SelectedWord);
            RefreshOrder(SelectedWord.Options);

            RaisePropertyChanged("OptionList");
            RaisePropertyChanged("ExampleText");
            RaisePropertyChanged("WordUnionList");
        }

        private void OnOptionChanged(object param)
        {
            WordChanged(null, null);
            UpdateEncodeds();

            RaisePropertyChanged("WordUnionList");
            RaisePropertyChanged("ExampleText");
        }

        private void OnAddOtherCrawling(object param)
        {
            CrawlingViewModel now = param as CrawlingViewModel;
            if (now != null &&
                now.Crawling != null && 
                now.Crawling != nowCrawling &&
                !_otherCrawlingList.Any(n=>n.Name==now.Name))
            {
                OtherCrawlingList.Add(new CrawlingInfoViewModel(now));
                OtherCrawlingChanged(null, null);
                RaisePropertyChanged("OtherCrawlingList");
                RaisePropertyChanged("WordUnionList");
            }
        }

        private void OnOtherCrawlingOptionChanged(object param)
        {
            CrawlingInfoViewModel now = param as CrawlingInfoViewModel;
            now.OptionWord.Pointer = EncodingWordHelper.SearchSameWord(
                WordList.Select(x => x.Word).ToList(),
                now.Option);
            OtherCrawlingChanged(null, null);

            RaisePropertyChanged("OtherUrlOptionExample");
        }

        private void OnTestCrawling(object param)
        {
            CrawlingInfoViewModel now = param as CrawlingInfoViewModel;
            now.OtherCrawlingWordList = CrawlingHelper.CrawlingOne(now.Crawling, now.OptionWord.ToString());

            RaisePropertyChanged("WordUnionList");
        }

        private void OnDeleteCrawling(object param)
        {
            _otherCrawlingList.Remove(param as CrawlingInfoViewModel);
            OtherCrawlingChanged(null, null);

            RaisePropertyChanged("OtherCrawlingList");
            RaisePropertyChanged("WordUnionList");
        }

        private void OnRefresh(object param)
        {
            RaisePropertyChanged("OtherCrawlingList");
            RaisePropertyChanged("WordList");
        }

        private void OnSwitchOtherCrawling(object param)
        {
            _isVisibleOtherCrawling = !_isVisibleOtherCrawling;
            RaisePropertyChanged("IsVisibleOtherCrawling");
            RaisePropertyChanged("OtherCrawlingList");
        }

        private void OnSwitchExcept(object param)
        {
            _isVisibleExcept = !_isVisibleExcept;
            RaisePropertyChanged("IsVisibleExcept");
            RaisePropertyChanged("ExceptList");
            RaisePropertyChanged("ExampleText");
        }
 
        private void OnAddExcept(object param)
        {
            SelectedWord.Word.Excepts.Add(new ExcpetWord());
            RaisePropertyChanged("ExceptList");
            RaisePropertyChanged("ExampleText");
        }

        private void OnDeleteExcept(object param)
        {
            SelectedWord.Word.Excepts.Remove(param as ExcpetWord);
            RaisePropertyChanged("ExceptList");
            RaisePropertyChanged("ExampleText");
        }

        #endregion

        #region CanCommand

        private bool CanDeleteWord(object param)
        {
            CustomWordViewModel now = param as CustomWordViewModel;

            return now != null && _wordList.Any(w=>w==now);
        }

        private bool CanAddOption(object param)
        {
            return (SelectedWord.EncodedExample != null && SelectedWord.OriginalExample != null);
        }

        private bool IsOtherCrawling(object param)
        {
            CrawlingViewModel now = param as CrawlingViewModel;
            return (now != null && now.Crawling != nowCrawling);
        }

        #endregion
    }
}
