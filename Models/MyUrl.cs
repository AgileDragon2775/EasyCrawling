namespace EasyCrawling.Models
{
    [System.Serializable]
    public class MyUrl
    {
        [System.NonSerialized]
        public string Option;
        public string LeftUrl { get; set; }
        public string RightUrl { get; set; }

        public MyUrl() { }
        public MyUrl(MyUrl url)
        {
            Option = url.Option;
            LeftUrl = url.LeftUrl;
            RightUrl = url.RightUrl;
        }

        public override string ToString()
        {
            return LeftUrl + Option + RightUrl;
        }

        public override bool Equals(object obj)
        {
            var item = obj as MyUrl;
            if(ReferenceEquals(item, null))
            {
                return false;
            }

            if(ReferenceEquals(item, this))
            {
                return true;
            }

            return item.Option == Option &&
                item.LeftUrl == LeftUrl &&
                item.RightUrl == RightUrl;
        }

        public override int GetHashCode()
        {
            return 2 * (Option == null ? 0 : Option.GetHashCode()) +
                3 * (LeftUrl == null ? 0 : LeftUrl.GetHashCode()) +
                5 * (RightUrl == null ? 0 : RightUrl.GetHashCode());
        }
    }
}
