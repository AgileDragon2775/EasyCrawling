using EasyCrawling.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrawling.ViewModels
{
    public class OriginalWordListViewModel : Base.ViewModelBase
    {
        #region Fields

        private string _searchWord;
        private string _tempWord;
        private string _searchLabel = " Search ";
        private int _searchIndex = 0;
        private TreeData _selected;
        private List<TreeData> _searchedNodes;
        private List<TreeData> _wordList;
        private RelayCommand _addOriginalWordCommand;
        private RelayCommand _searchCommand;

        #endregion

        #region event

        public event EventHandler WordAdded;

        #endregion

        #region Constructors

        public OriginalWordListViewModel()
        {
            _wordList = new List<TreeData>();
        }

        #endregion

        #region Properties

        public ObservableCollection<TreeData> WordList
        {
            get => new ObservableCollection<TreeData>(_wordList);
        }

        public TreeData Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                RaisePropertyChanged();
            }
        }

        public string SearchWord
        {
            get => _searchWord;
            set
            {
                if (_searchWord != value)
                {
                    _searchWord = value;
                    InitSearch();
                    RaisePropertyChanged();
                }
            }
        }

        public string SearchLabel
        {
            get => _searchLabel;
            set => Set(ref _searchLabel, value);
        }

        #endregion

        #region Command

        public RelayCommand AddOriginalWordCommand
        {
            get => _addOriginalWordCommand ??
                    (_addOriginalWordCommand = new RelayCommand(OnAddOriginalWord));
            set => Set(ref _addOriginalWordCommand, value);
        }

        public RelayCommand SearchCommand
        {
            get => _searchCommand ??
                    (_searchCommand = new RelayCommand(OnSearch, CanSearch));
            set => Set(ref _searchCommand, value);
        }

        #endregion

        #region method

        public void InitList(List<TreeData> treeList)
        {
            _wordList = treeList;
            RaisePropertyChanged("WordList");
        }

        private void InitSearch()
        {
            _tempWord = "";
            _searchIndex = 0;
            SearchLabel = " Search ";
        }

        private void SearchFromList(List<TreeData> nodes)
        {
            foreach (TreeData nowNode in nodes)
            {            
                if (nowNode.Text.IndexOf(_searchWord) != -1)
                    _searchedNodes.Add(nowNode);
            }
        }

        #endregion

        #region ExcuteCommand

        private void OnAddOriginalWord(object param)
        {
            WordAdded(param, null);
        }

        public void OnSearch(object param)
        {
            if (_tempWord != _searchWord)
            {
                _searchedNodes = new List<TreeData>();
                SearchFromList(_wordList);
            }

            if (_searchedNodes.Count > 0)
            {
                TreeData SelectedItem = _searchedNodes[_searchIndex % _searchedNodes.Count];
                SearchLabel = " " + (++_searchIndex) + " / " + _searchedNodes.Count + " ";
                _searchIndex = _searchIndex % _searchedNodes.Count;
                                
                Selected = SelectedItem;
            }

            _tempWord = _searchWord;
        }

        #endregion

        #region CanCommand

        public bool CanSearch(object param)
        {
            return string.IsNullOrEmpty(SearchWord) ?
                 false :
                 true;
        }

        #endregion
    }
}
