using EasyCrawling.Helpers;
using EasyCrawling.Models;
using System.IO;

namespace EasyCrawling.ViewModels
{
    public class CrawlingViewModel :Base.ViewModelBase
    {
        #region Fields

        public Crawling Crawling { get; }

        #endregion

        #region Constructors

        public CrawlingViewModel()
        {
            Crawling = new Crawling();            
        }

        public CrawlingViewModel(string name) : this()
        {
            Crawling.Name = name;
        }

        public CrawlingViewModel(string name, Crawling crawling)
        {
            Crawling = crawling;
            Crawling.Name = name;
        }

        #endregion

        #region Properties

        public string LeftUrl
        {
            get => Crawling.Url.LeftUrl;
            set
            {
                if (Crawling.Url.LeftUrl != value)
                {
                    Crawling.Url.LeftUrl = value;
                    Dirty();
                    RaisePropertyChanged("Url");
                }
            }
        }

        public string RightUrl
        {
            get => Crawling.Url.RightUrl;
            set
            {
                if (Crawling.Url.RightUrl != value)
                {
                    Crawling.Url.RightUrl = value;
                    Dirty();
                    RaisePropertyChanged("Url");
                }
            }
        }

        public string Url
        {
            get => Crawling.Url.LeftUrl + "├OPTION┤" + Crawling.Url.RightUrl;
        }

        public bool IsActivated { get; set; }
        
        public bool IsStarted
        {
            get => Crawling.IsStarted;
            set
            {
                Crawling.IsStarted = value;
                Dirty();
                RaisePropertyChanged("StartString");
            }
        }

        public string StartString
        {
            get => Crawling.IsStarted ? "■" : "▶";            
        }

        public string Name
        {
            get => Crawling.Name;
            set
            {
                if(Crawling.Name != value)
                {
                    if (!FileHelper.IsExistFile(value))
                    {
                        FileHelper.ChangeFileName(Crawling.Name, value);
                        Crawling.Name = value;
                        Dirty();
                        RaisePropertyChanged();
                    }
                }              
            }
        }

        #endregion

        #region Method

        private void Dirty()
        {
            FileHelper.SaveFile(Crawling);
        }

        #endregion
    }
}
