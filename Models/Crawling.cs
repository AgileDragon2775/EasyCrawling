using System.Collections.Generic;

namespace EasyCrawling.Models
{
    [System.Serializable]
    public class Crawling
    {
        public string Name { get; set; }
        public bool IsStarted { get; set; }
        public MyUrl Url { get; set; }
        public string BaseXPath { get; set; }
        public List<Word> WordList { get; set; }
        public List<CrawlingInfo> OtherCrawlingList { get; set; }
        public List<BaseAction> ActionList { get; set; }
        public List<UrlOption> UrlOptionLIst { get; set; }
        public List<WhenCrawling> WhenList { get; set; }

        [System.NonSerialized]
        public bool IsDoing;

        public Crawling()
        {
            Url = new MyUrl();
            WordList = new List<Word>();
            OtherCrawlingList = new List<CrawlingInfo>();
            ActionList = new List<BaseAction>();
            UrlOptionLIst = new List<UrlOption>();
            WhenList = new List<WhenCrawling>();            
        }
        public Crawling(Crawling crawling) :this()
        {
            Url = new MyUrl(crawling.Url);
            BaseXPath = crawling.BaseXPath;
           
            foreach (Word word in crawling.WordList)
            {
                WordList.Add(new Word(word));
            }

            foreach (BaseAction action in crawling.ActionList)
            {
                ActionList.Add(new BaseAction(action));
            }
            foreach (CrawlingInfo info in crawling.OtherCrawlingList)
            {
                OtherCrawlingList.Add(new CrawlingInfo(info));
            }
            foreach (UrlOption option in crawling.UrlOptionLIst)
            {
                UrlOptionLIst.Add(new UrlOption(option));
            }
            foreach (WhenCrawling when in crawling.WhenList)
            {
                WhenList.Add(new WhenCrawling(when));
            }

        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            var item = obj as Crawling;
            if (item == null)
            {
                return false;

            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!Url.Equals(item.Url) ||
                BaseXPath != item.BaseXPath)
            {
                return false;
            }

            if(WordList.Count != item.WordList.Count||
               ActionList.Count != item.ActionList.Count)
            {
                return false;
            }

            for (int i = 0; i < WordList.Count; i++)
            {
                if (!WordList[i].Equals(item.WordList[i]))
                {
                    return false;
                }
            }

            for (int i = 0; i < ActionList.Count; i++)
            {
                if (!ActionList[i].Equals(item.ActionList[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return 2 * (Url == null ? 0 : Url.GetHashCode()) +
                3 * (BaseXPath == null ? 0 : BaseXPath.GetHashCode()) +
                5 * (WordList == null ? 0 : WordList.Count) +
                7 * (ActionList == null ? 0 : ActionList.Count);
        }
    }
}
