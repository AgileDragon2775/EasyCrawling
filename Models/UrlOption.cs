namespace EasyCrawling.Models
{
    [System.Serializable]
    public class UrlOption
    {
        public string Option { get; set; }
        public System.DateTime LastCrawling { get; set; }

        public UrlOption()
        {
            Option = "";
        }
        public UrlOption(UrlOption option)
        {
            Option = option.Option;
            LastCrawling = option.LastCrawling;
        }
    }
}
