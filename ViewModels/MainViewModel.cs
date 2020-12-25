using EasyCrawling.Helpers;
using EasyCrawling.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EasyCrawling.ViewModels
{
    public class MainViewModel : Base.ViewModelBase
    {
        #region Fields

        private List<CrawlingAndIndexs> mainCrawlings;
        private CrawlingViewModel _selectedCrawling;
        private bool _isVisibleUrlOption;

        private RelayCommand _addCrawlingCommand;
        private RelayCommand _addUrlOptionCommand;
        private RelayCommand _addTImeCommand;
        private RelayCommand _deleteCrawlingCommand;
        private RelayCommand _deleteUrlOptionCommand;
        private RelayCommand _deleteTImeCommand;
        private RelayCommand _copyCrawlingCommand;
        private RelayCommand _modifyCrawlingCommand;
        private RelayCommand _startCrawlingCommand;
        private RelayCommand _saveCrawlingsCommand;
        private RelayCommand _resetCrawlingsCommand;

        #endregion

        #region Constructors

        public MainViewModel(List<CrawlingAndIndexs> crawlings)
        {
            mainCrawlings = crawlings;
        }

        #endregion

        #region Properties

        public ObservableCollection<CrawlingViewModel> CrawlingList
        {
            get => new ObservableCollection<CrawlingViewModel>(
                mainCrawlings
               .Select(c => new CrawlingViewModel(c.Crawling.Name, c.Crawling)));
        }

        public CrawlingViewModel SelectedCrawling
        {
            get =>_selectedCrawling;
            set
             {
                if(_selectedCrawling != value)
                {
                    _selectedCrawling = value;                   
                    RaisePropertyChanged("IsVisibleUrlOption");
                    RaisePropertyChanged("UrlOptionList");
                    RaisePropertyChanged("WhenList");                    
                    RaisePropertyChanged();
                }
            }           
        }

        public ObservableCollection<UrlOptionViewModel> UrlOptionList
        {
            get => _selectedCrawling == null ?
                new ObservableCollection<UrlOptionViewModel>() :
                new ObservableCollection<UrlOptionViewModel>(
                    _selectedCrawling.Crawling.UrlOptionLIst
                    .Select(x=>new UrlOptionViewModel(x, _selectedCrawling.Crawling)));          
        }

        public ObservableCollection<WhenViewModel> WhenList
        {
            get => _selectedCrawling == null ?
                new ObservableCollection<WhenViewModel>() :
                new ObservableCollection<WhenViewModel>(
                    _selectedCrawling.Crawling.WhenList
                    .Select(x => new WhenViewModel(x, _selectedCrawling.Crawling)));
        }

        public bool IsCheckedUrlOption
        {
            get => _isVisibleUrlOption;
            set 
            {
                if(_isVisibleUrlOption != value)
                {
                    _isVisibleUrlOption = value;
                    RaisePropertyChanged("IsVisibleUrlOption");
                    RaisePropertyChanged();
                }
            }             
        }

        public Visibility IsVisibleUrlOption
        {
            get => _isVisibleUrlOption ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Command

        public RelayCommand AddCrawlingCommand
        {
            get => _addCrawlingCommand ??
                    (_addCrawlingCommand = new RelayCommand(OnAddCrawling));
            set => Set(ref _addCrawlingCommand, value);
        }

        public RelayCommand AddUrlOptionCommand
        {
            get => _addUrlOptionCommand ??
                    (_addUrlOptionCommand = new RelayCommand(OnAddUrlOption, IsSeletedUrl));
            set => Set(ref _addUrlOptionCommand, value);
        }

        public RelayCommand AddTImeCommand
        {
            get => _addTImeCommand ??
                    (_addTImeCommand = new RelayCommand(OnAddTIme, IsSeletedUrl));
            set => Set(ref _addTImeCommand, value);
        }

        public RelayCommand DeleteCrawlingCommand
        {
            get => _deleteCrawlingCommand ??
                    (_deleteCrawlingCommand = new RelayCommand(OnDeleteCrawling, IsSelected));
            set => Set(ref _deleteCrawlingCommand, value);
        }

        public RelayCommand DeleteUrlOptionCommand
        {
            get => _deleteUrlOptionCommand ??
                    (_deleteUrlOptionCommand = new RelayCommand(OnDeleteUrlOption, IsSelected));
            set => Set(ref _deleteUrlOptionCommand, value);
        }

        public RelayCommand DeleteTImeCommand
        {
            get => _deleteTImeCommand ??
                    (_deleteTImeCommand = new RelayCommand(OnDeleteTIme, IsSelected));
            set => Set(ref _deleteTImeCommand, value);
        }

        public RelayCommand CopyCrawlingCommand
        {
            get => _copyCrawlingCommand ??
                    (_copyCrawlingCommand = new RelayCommand(OnCopyCrawling, IsSelected));
            set => Set(ref _copyCrawlingCommand, value);
        }

        public RelayCommand ModifyCrawlingCommand
        {
            get => _modifyCrawlingCommand ??
                    (_modifyCrawlingCommand = new RelayCommand(OnOpenCrawlingWindow, IsNotActivated));
            set => Set(ref _modifyCrawlingCommand, value);
        }
        
        public RelayCommand StartCrawlingCommand
        {
            get => _startCrawlingCommand ??
                    (_startCrawlingCommand = new RelayCommand(OnStartCrawling));
            set => Set(ref _startCrawlingCommand, value);
        }

        public RelayCommand SaveCrawlingsCommand
        {
            get => _saveCrawlingsCommand ??
                    (_saveCrawlingsCommand = new RelayCommand(OnSaveCrawlings));
            set => Set(ref _saveCrawlingsCommand, value);
        }

        public RelayCommand ResetCrawlingsCommand
        {
            get => _resetCrawlingsCommand ??
                    (_resetCrawlingsCommand = new RelayCommand(OnResetCrawlings));
            set => Set(ref _resetCrawlingsCommand, value);
        }
        
        #endregion

        #region Method

        private string SearchUnusedName()
        {
            int name = 0;

            while (mainCrawlings.Any(c => c.Crawling.Name == name.ToString()))
            {
                name++;
            }

            return name.ToString();
        }

        private void SaveAllCrawling()
        {
            foreach(var crawling in mainCrawlings)
            {
                if (mainCrawlings.Count(c => c.Crawling.Name == crawling.Crawling.Name) != 1)
                {
                    MessageBox.Show("A duplicate name exist in list");
                    return;
                }
            }

            foreach (var crawling in mainCrawlings)
            {
                FileHelper.SaveFile(crawling.Crawling);
            }
        }

        #endregion

        #region ExcuteCommand

        private void OnAddCrawling(object param)
        {
            CrawlingAndIndexs newCrawling = new CrawlingAndIndexs(SearchUnusedName());
            mainCrawlings.Add(newCrawling);
            FileHelper.SaveFile(newCrawling.Crawling);
            RaisePropertyChanged("CrawlingList");
        }

        private void OnAddUrlOption(object param)
        {
            SelectedCrawling.Crawling.UrlOptionLIst.Add(new UrlOption());       
            RaisePropertyChanged("UrlOptionList");
        }

        private void OnAddTIme(object param)
        {
            SelectedCrawling.Crawling.WhenList.Add(new WhenCrawling());          
            RaisePropertyChanged("WhenList");
        }

        private void OnDeleteCrawling(object param)
        {
            var found = mainCrawlings.Find(x => x.Crawling.Name == (param as CrawlingViewModel).Name);
            if (found != null) mainCrawlings.Remove(found);
            FileHelper.DeleteFile((param as CrawlingViewModel).Name);
            FileHelper.DeleteIndexFile((param as CrawlingViewModel).Name);

            RaisePropertyChanged("CrawlingList");
        }

        private void OnDeleteUrlOption(object param)
        {
            SelectedCrawling.Crawling.UrlOptionLIst.Remove((param as UrlOptionViewModel).UrlOption);
            FileHelper.SaveFile(SelectedCrawling.Crawling);
            RaisePropertyChanged("UrlOptionList");
        }

        private void OnDeleteTIme(object param)
        {
            SelectedCrawling.Crawling.WhenList.Remove((param as WhenViewModel).WhenCrawling);
            FileHelper.SaveFile(SelectedCrawling.Crawling);
            RaisePropertyChanged("WhenList");
        }

        private void OnCopyCrawling(object param)
        {
            CrawlingAndIndexs newCrawling = new CrawlingAndIndexs((param as CrawlingViewModel).Crawling, SearchUnusedName());
   
            mainCrawlings.Add(newCrawling);
            FileHelper.SaveFile(newCrawling.Crawling);
            RaisePropertyChanged("CrawlingList");
        }

        private void OnOpenCrawlingWindow(object param)
        {
            CrawlingWindow crawlingWindow = new CrawlingWindow();
            crawlingWindow.DataContext = new CrawlingWindowViewModel(param as CrawlingViewModel, CrawlingList);
            (param as CrawlingViewModel).IsActivated = true;
            crawlingWindow.Closed += (s, e) =>
            {
                (param as CrawlingViewModel).IsActivated = false;
                FileHelper.SaveFile((param as CrawlingViewModel).Crawling);
                RaisePropertyChanged("CrawlingList");
            };
            crawlingWindow.Show();
            crawlingWindow.Focus();
        }

        private void OnResetCrawlings(object param)
        {
            if (MessageBox.Show("Do you reset this crawling?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                foreach (var option in (param as CrawlingViewModel).Crawling.UrlOptionLIst)
                {
                    option.LastCrawling = new System.DateTime();
                }

                CrawlingAndIndexs search = mainCrawlings.Find(x => x.Crawling.Name == (param as CrawlingViewModel).Name);
                if (search != null) search.indexs = new List<string>();
                search.Crawling.IsDoing = false;

                FileHelper.SaveFile((param as CrawlingViewModel).Crawling);
                FileHelper.DeleteIndexFile((param as CrawlingViewModel).Name);                
            }           
        }
        
        private void OnStartCrawling(object param)
        {
            CrawlingViewModel now = param as CrawlingViewModel;
            now.IsStarted = !now.IsStarted;
        }

        private void OnSaveCrawlings(object param)
        {
            SaveAllCrawling();
        }

        #endregion

        #region CanCommand

        private bool IsSelected(object param)
        {
             return param != null;
        }

        private bool IsSeletedUrl(object param)
        {
            return _selectedCrawling != null;
        }

        private bool IsNotActivated(object param)
        {
            return (param != null) && !(param as CrawlingViewModel).IsActivated;
        }

        #endregion
    }
}
