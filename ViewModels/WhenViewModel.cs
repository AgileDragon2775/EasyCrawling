namespace EasyCrawling.ViewModels
{
    public class WhenViewModel :Base.ViewModelBase
    {
        #region Fields

        private Models.Crawling crawling;
        public Models.WhenCrawling WhenCrawling;

        #endregion

        #region Constructors

        public WhenViewModel()
        {
            WhenCrawling = new Models.WhenCrawling();
        }

        public WhenViewModel(Models.WhenCrawling when, Models.Crawling crawling)
        {
            this.WhenCrawling = when;
            this.crawling = crawling;
        }

        #endregion

        #region Properties

        public Enums.TimeSpanType When
        {
            get => WhenCrawling.When;
            set
            {
                if (WhenCrawling.When != value)
                {
                    WhenCrawling.When = value;
                    Dirty();
                    RaisePropertyChanged();
                }
            }
        }

        public Enums.Week Week
        {
            get => WhenCrawling.Week;
            set
            {
                if (WhenCrawling.Week != value)
                {
                    WhenCrawling.Week = value;
                    Dirty();
                    RaisePropertyChanged();
                }
            }
        }

        public int Hour
        {
            get => WhenCrawling.Hour;
            set
            {
                if (WhenCrawling.Hour != value)
                {
                    WhenCrawling.Hour = value;
                    Dirty();
                    RaisePropertyChanged();
                }
            }
        }

        public int Min
        {
            get => WhenCrawling.Min;
            set
            {
                if (WhenCrawling.Min != value)
                {
                    WhenCrawling.Min = value;
                    Dirty();
                    RaisePropertyChanged();
                }
            }
        }

        public int Sec
        {
            get => WhenCrawling.Sec;
            set
            {
                if (WhenCrawling.Sec != value)
                {
                    WhenCrawling.Sec = value;
                    Dirty();
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Method

        private void Dirty()
        {
            Helpers.FileHelper.SaveFile(crawling);
        }

        #endregion
    }
}
