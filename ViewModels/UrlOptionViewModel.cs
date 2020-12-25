using EasyCrawling.Helpers;

namespace EasyCrawling.ViewModels
{   
    public class UrlOptionViewModel : Base.ViewModelBase
    {
        #region Fields

        private Models.Crawling crawling;
        public Models.UrlOption UrlOption;

        #endregion

        #region Constructors

        public UrlOptionViewModel()
        {
            UrlOption = new Models.UrlOption();
        }

        public UrlOptionViewModel(Models.UrlOption option, Models.Crawling crawling)
        {
            UrlOption = option;
            this.crawling = crawling;
        }

        #endregion


        #region Properties

        public string Option
        {
            get => UrlOption.Option;
            set
            {
                if(UrlOption.Option != value)
                {
                    UrlOption.Option = value;
                    Dirty();
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Method

        private void Dirty()
        {
            FileHelper.SaveFile(crawling);
        }

        #endregion
    }
}
