using EasyCrawling.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;

namespace EasyCrawling.ViewModels
{
    public class OriginalWordTreeViewModel : Base.ViewModelBase
    {
        #region Fields

        private ReadOnlyCollection<TreeDataViewModel> _tree;
        private List<TreeDataViewModel> _searchedNodes;
        private string _searchWord;
        private string _tempWord;
        private string _searchLabel = " Search ";
        private int _searchIndex = 0;
        private RelayCommand _addNodeCommand;
        private RelayCommand _searchCommand;

        #endregion

        #region event

        public event EventHandler NodeChanged;

        #endregion

        #region Constructors

        #endregion

        #region Properties
        public ReadOnlyCollection<TreeDataViewModel> Tree
        {
            get => _tree;
            set
            {
                if(_tree != value)
                {
                    _tree = value;
                    InitSearch();
                    RaisePropertyChanged();
                }
            }
        }

        public string SearchWord
        {
            get => _searchWord;
            set
            {
                if( _searchWord != value)
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

        public RelayCommand AddNodeCommand
        {
            get => _addNodeCommand ??
                    (_addNodeCommand = new RelayCommand(OnAddNode));
            set => Set(ref _addNodeCommand, value);
        }

        public RelayCommand SearchCommand
        {
            get => _searchCommand ??
                    (_searchCommand = new RelayCommand(OnSearch, CanSearch));
            set => Set(ref _searchCommand, value);
        }

        #endregion

        #region method

        private void InitSearch()
        {
            _tempWord = "";
            _searchIndex = 0;
            SearchLabel = " Search ";
        }

        private void SearchFromTree(ReadOnlyCollection<TreeDataViewModel> nodes)
        {
            foreach (TreeDataViewModel nowNode in nodes)
            {
                string nowWord = nowNode.Text as string;
                if (nowWord.IndexOf(_searchWord) != -1)
                    _searchedNodes.Add(nowNode);

                SearchFromTree(nowNode.Children);
            }
        }

        #endregion

        #region ExcuteCommand
        public void OnAddNode(object param)
        {
            NodeChanged(param, null);        
        }

        public void OnSearch(object param)
        {
            if (_tempWord != _searchWord)
            {
                _searchedNodes = new List<TreeDataViewModel>();
                SearchFromTree(Tree[0].Children);
            }
         
            if (_searchedNodes.Count > 0)
            {
                TreeDataViewModel SelectedItem = _searchedNodes[_searchIndex % _searchedNodes.Count];
                SearchLabel = " " + (++_searchIndex) + " / " + _searchedNodes.Count + " ";
                _searchIndex = _searchIndex % _searchedNodes.Count;

                SelectedItem.IsExpanded = false;
                SelectedItem.IsSelected = true;                
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
