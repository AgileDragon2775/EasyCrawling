using System.Collections.Generic;
using HtmlAgilityPack;
using EasyCrawling.Helpers;
using EasyCrawling.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading;

namespace EasyCrawling.ViewModels
{
    public class CrawlingWindowViewModel : Base.ViewModelBase
    {
        #region Fields

        private readonly CrawlingViewModel _nowCrawling;
       
        private HtmlDocument _htmlDocument;
        private string _statusText;

        private HtmlViewModel _html;
        private OriginalWordTreeViewModel _originalWordTree;
        private OriginalWordListViewModel _originalWordList;
        private CustomWordCollectionViewModel _customWord;
        private TestViewModel _test;
        private BaseActionListViewModel _myAction;
        private ToastViewModel _toast;

        private RelayCommand _startCrawlingCommand;
        private RelayCommand _saveCrawlingCommand;

        #endregion

        #region Constructors

        public CrawlingWindowViewModel(CrawlingViewModel crawling, ObservableCollection<CrawlingViewModel> crawlingList)
        {
            _nowCrawling = crawling;
            InitViewModel(crawlingList);            
            InitCrawling(NowCrawling, crawlingList);            
        }
        #endregion

        #region ViewModels Properties
        public HtmlViewModel Html
        {
            get => Get(ref _html);
            set => Set(ref _html, value);
        }

        public OriginalWordTreeViewModel OriginalWordTree
        {
            get => Get(ref _originalWordTree);
            set => Set(ref _originalWordTree, value);
        }

        public OriginalWordListViewModel OriginalWordList
        {
            get => Get(ref _originalWordList);
            set => Set(ref _originalWordList, value);
        }

        public CustomWordCollectionViewModel CustomWord
        {
            get => Get(ref _customWord);
            set => Set(ref _customWord, value);
        }

        public TestViewModel Test
        {
            get => Get(ref _test);
            set => Set(ref _test, value);
        }

        public BaseActionListViewModel MyAction
        {
            get => Get(ref _myAction);
            set => Set(ref _myAction, value);
        }

        public ToastViewModel Toast
        {
            get => Get(ref _toast);
            set => Set(ref _toast, value);
        }
        #endregion
       
        #region Properties
        public CrawlingViewModel NowCrawling
        {
            get => _nowCrawling;
        }

        public string Url
        {
            get => LeftUrl + OptionUrl + RightUrl;
        }

        public string LeftUrl
        {
            get => NowCrawling.LeftUrl;//"https://nyaa.si/?f=0&c=0_0&q=moozzi"; //NowCrawling.Url;
            set
            {
                if(NowCrawling.LeftUrl != value)
                {
                    NowCrawling.LeftUrl = value;
                    Toast.RefreshView();
                    RaisePropertyChanged();                   
                }
            }                
        }

        public string OptionUrl
        {
            get => NowCrawling.Crawling.Url.Option;
            set
            {
                if (NowCrawling.Crawling.Url.Option != value)
                {
                    NowCrawling.Crawling.Url.Option = value;
                    Toast.RefreshView();
                    RaisePropertyChanged();
                }
            }
        }

        public string RightUrl
        {
            get => NowCrawling.RightUrl;
            set
            {
                if (NowCrawling.RightUrl != value)
                {
                    NowCrawling.RightUrl = value;
                    Toast.RefreshView();
                    RaisePropertyChanged();
                }
            }
        }

        public HtmlDocument HtmlDocument
        {
            get => Get(ref _htmlDocument);
            set => Set(ref _htmlDocument, value);
        }

        public string StatusText
        {
            get => _statusText;
            set => Set(ref _statusText, value);
        }

        #endregion

        #region Command

        public RelayCommand StartCrawlingCommand
        {
            get
            {
                return _startCrawlingCommand ??
                    (_startCrawlingCommand = new RelayCommand(OnCrawling));
            }
            set => _startCrawlingCommand = value;
        }

        public RelayCommand SaveCrawlingCommand
        {
            get
            {
                return _saveCrawlingCommand ??
                    (_saveCrawlingCommand = new RelayCommand(OnSaveCrawling));
            }
            set => _saveCrawlingCommand = value;
        }        

        #endregion

        #region method

        private void InitViewModel(ObservableCollection<CrawlingViewModel> crawlingList)
        {
            Html = new HtmlViewModel();
            OriginalWordTree = new OriginalWordTreeViewModel();
            OriginalWordList = new OriginalWordListViewModel();
            CustomWord = new CustomWordCollectionViewModel(NowCrawling.Crawling, crawlingList);
            Test = new TestViewModel();
            MyAction = new BaseActionListViewModel();
            Toast = new ToastViewModel();

            OriginalWordTree.NodeChanged += (s, e) => SetXPath(s);
            OriginalWordTree.NodeChanged += (s, e) => RefreshViewModels();
            OriginalWordList.WordAdded += (s, e) => AddOriginalWord(s);
            CustomWord.WordChanged += (s, e) => DoTest(false);
            CustomWord.WordChanged += (s, e) => Toast.RefreshView();
            CustomWord.WordChanged += (s, e) => SaveWordList();
            CustomWord.WordAdded += (s, e) => Toast.RefreshWords();
            CustomWord.OtherCrawlingChanged += (s, e) => SaveCrawlingList();
            Test.DoTest += (s, e) => DoTest(true);
            Test.ClassificationChanged += (s, e) => Test.ChangeClassification(CustomWord.WordList.Select(x => x.Word).ToList());
            Test.TestStarted += (s, e) => StatusText = "Testing.....";
            Test.TestEnded += (s, e) => StatusText = "";
            MyAction.SetAction += (s, e) => InitToastAction(s);
            MyAction.ActionChanged += (s, e) => SaveActionLIst();
            Toast.TitleChanged += (s, e) => SearchWordUsingName(s);
            Toast.ToastChanged += (s, e) => SaveActionLIst();
            Toast.TestToast += (s, e) => TestToast(s);
        }

        private void InitCrawling(CrawlingViewModel crawling, ObservableCollection<CrawlingViewModel> crawlingList)
        {            
            CustomWord.WordList = new ObservableCollection<CustomWordViewModel>(
                crawling.Crawling.WordList.Select(
                      x => new CustomWordViewModel(x)));
            CustomWord.OtherCrawlingList = new ObservableCollection<CrawlingInfoViewModel>(
                crawling.Crawling.OtherCrawlingList.Select(
                    x => crawlingList.Any(c=>c.Name == x.CrawlingName) == true ?
                    new CrawlingInfoViewModel(crawlingList.First(cl=>cl.Name == x.CrawlingName)):
                    new CrawlingInfoViewModel()));
            MyAction.BaseList = new List<BaseAction>(crawling.Crawling.ActionList);
        }

        private void SaveWordList()
        {
            NowCrawling.Crawling.WordList = CustomWord.WordList.Select(x => x.Word).ToList();
        }

        private void SaveCrawlingList()
        {
            NowCrawling.Crawling.OtherCrawlingList = CustomWord.OtherCrawlingList.Select(x => x.CrawlingInfo).ToList();
        }

        private void SaveActionLIst()
        {
            NowCrawling.Crawling.ActionList = MyAction.ActionList.Select(x => x.BaseAction).ToList();
        }

        private void SaveCrawling()
        {
            SaveWordList();
            SaveCrawlingList();
            SaveActionLIst();
        }

        private void StartCrawling()
        {
            StatusText = "Downloading......";
            HtmlDocument = CrawlingHelper.InfiniteDownloadHtml(Url);
            StatusText = "";
            Html.Html = HtmlDocument.Text;
            OriginalWordTree.Tree =
                new ReadOnlyCollection<TreeDataViewModel>(
                    new TreeDataViewModel[]
                    {
                        new TreeDataViewModel(
                            TreeHelper.GetTree(HtmlDocument.DocumentNode))
                    });
            RefreshViewModels();
        }

        private void RefreshViewModels()
        {
            InitOriginalWordList();
            CustomWord.RefreshWords(HtmlDocument, NowCrawling.Crawling.BaseXPath);
            DoTest(false);
            Toast.RefreshWords();
            Toast.RefreshView();
        }
          
        private void SetXPath(object sender)
        {
            NowCrawling.Crawling.BaseXPath = TreeHelper.GetAllPath(sender as TreeDataViewModel);
        }

        private void InitOriginalWordList()
        {            
            HtmlNodeCollection nodes = CrawlingHelper.GetResults(HtmlDocument, NowCrawling.Crawling.BaseXPath);
            List<TreeData> newTreeDataList = new List<TreeData>();

            if (nodes == null) return;

            foreach(HtmlNode node in nodes)
            {                
                TreeHelper.SetListUsingNode(
                    newTreeDataList,
                    node,
                    ".");
            }

            OriginalWordList.InitList( newTreeDataList);
        }

        private void AddOriginalWord(object sender)
        {
            TreeData selected = sender as TreeData;
            CustomWord.AddOriginalWord(
                selected.Tag.cmpAttr,
                selected.Tag,
                selected.Text);       
        }
        
        private void DoTest(bool isDirect)
        {
            Thread thread = new Thread(()=> Test.DoTestIfNeeded(
                HtmlDocument,
                NowCrawling.Crawling.BaseXPath,
                isDirect,
                NowCrawling.Crawling));
            thread.Start();
        }

        private void InitToastAction(object param)
        {
            Toast.NowToast = (param as BaseActionViewModel).BaseAction as MyToast;         
        }

        private void SearchWordUsingName(object param)
        {
            var now = param as NameViewModel;

            now.Word = EncodingWordHelper.SearchSameWord(
                CustomWord.WordUnionList.Select(x => x.Word).ToList(),    
                now.Title);            
        }

        private void TestToast(object param)
        {
            NotificationHelper.CreateCollection(NowCrawling.Name, OptionUrl);
            NotificationHelper.SendToastAsync(Toast.NowToast, NowCrawling.Name, OptionUrl);
        }      

        #endregion

        #region ExcuteCommand
        private void OnCrawling(object param)
        {
            Thread thread = new Thread(() => StartCrawling());
            thread.Start();
        }

        private void OnSaveCrawling(object param)
        {
            SaveCrawling();
        }

        #endregion

        #region CanCommand

        #endregion
    }
}
