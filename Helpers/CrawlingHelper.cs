using HtmlAgilityPack;
using System.Net;
using EasyCrawling.Models;
using System.Text;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace EasyCrawling.Helpers
{  
    public class CrawlingHelper
    {
        public static HtmlDocument InfiniteDownloadHtml(string url, int page = 0)
        {            
            return DownloadHtml(url, true, page);
        }

        public static HtmlDocument DownloadHtml(string url, bool isInfinity = false, int page = 0)
        {   
            HtmlDocument doc = new HtmlDocument();

            using (var webclient = new WebClient())
            {
                WebRequest webRequest = WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {

                    if (response.CharacterSet.Contains("ISO"))
                        webclient.Encoding = Encoding.UTF8;
                    else
                    {
                        try
                        {
                            webclient.Encoding = Encoding.GetEncoding(response.CharacterSet);
                        }
                        catch
                        {
                            webclient.Encoding = Encoding.UTF8;
                        }
                    }
                }
                try
                {
                    var html = webclient.DownloadString(url);
                    doc.LoadHtml(html);
                }
                catch (WebException e)
                {
                    HttpWebResponse error = e.Response as HttpWebResponse;
                    if (error == null) return doc;
                    switch (error.StatusCode) 
                    { 
                        case HttpStatusCode.NotFound:                            
                            break;
                        default:
                            return doc;
                    }
                }                
            }
            if (isInfinity && !IsSuccess(doc))
            {
                System.Threading.Thread.Sleep(100);
                return DownloadHtml(url, isInfinity, page);
            }

            return doc;
        }

        public static bool IsSuccess(HtmlDocument doc)
        {
            if (doc == null)
            {
                return false;
            }

            if (doc.DocumentNode.SelectSingleNode("//html ") == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string ConvertAttrToTag(HtmlNode htmlNode)
        {            
            int count = 0;
            foreach (HtmlAttribute attr in htmlNode.Attributes)
            {
                if (attr.Name.Contains(":")) count++;               
            }
            if (count == htmlNode.Attributes.Count) return "";

            string attrStr = "[";
            int index = 0;
            foreach (HtmlAttribute attr in htmlNode.Attributes)
            {
                if (attr.Name.Contains(":"))
                    continue;

                if (index == 0)
                    attrStr += "@" + attr.Name;
                else
                    attrStr += " and @" + attr.Name;

                index++;
            }
            attrStr += "]";

            return attrStr;
        }

        public static string ConvertAttrToText(HtmlNode htmlNode)
        {
            if (htmlNode.Attributes.Count == 0)
            {
                return "";
            }

            string attrStr = " ";
            foreach (HtmlAttribute attr in htmlNode.Attributes)
            {
                attrStr += attr.Name + " =\"" + attr.Value + "\" ";
            }

            return attrStr;
        }

        public static HtmlNodeCollection GetResults(HtmlDocument doc, string tag)
        {   
            try
            {
                return doc.DocumentNode.SelectNodes(tag);
            }
            catch
            {
                return null;
            }
        }

        public static string GetFitWord(HtmlNodeCollection nodes, NodeTag cmpTag)
        {
            foreach (HtmlNode thisNode in nodes)
            {
                string xPath = GetAllXPath(thisNode);

                NodeTag nowTag = new NodeTag(
                    xPath,
                    thisNode.Attributes.Count,
                    thisNode.ChildNodes.Count,
                    thisNode.Name);

                if (nowTag == cmpTag) 
                    return thisNode.InnerText;

                foreach (HtmlAttribute thisAttrNode in thisNode.Attributes)
                {
                    nowTag = new NodeTag(
                        xPath,
                        thisNode.Attributes.Count,
                        thisNode.ChildNodes.Count,
                        thisAttrNode.Name);
                    if (nowTag == cmpTag)
                        return thisNode.Attributes[cmpTag.cmpAttr].Value;
                }

                string search = GetFitWord(thisNode.ChildNodes, cmpTag);
                if (search.Trim() != "") return search;
            }
            return "";
        }

        public static string GetIfExist(HtmlNode nowNode, NodeTag cmpTag)
        {
            HtmlNodeCollection nodes;
            if (cmpTag == null || cmpTag.currentXPath == null)
                return "";

            try
            {
                nodes = nowNode.SelectNodes(cmpTag.currentXPath);
            }
            catch
            {
                return "";
            }

            if (nodes == null) return "";

            return GetFitWord(nodes, cmpTag);            
        }

        public static string GetOneXPath(HtmlNode node)
        {
            string xPath = "//";

            if (node.Name != "")
            {
                if (IsText(node.Name))
                {
                    return "";
                }
                else
                {
                    xPath += node.Name;
                }
            }

            return xPath + ConvertAttrToTag(node);
        }

        public static string GetAllXPath(HtmlNode node)
        {
            string xPath = "";
            while (node != null)
            {
                if (IsText(node.Name))
                {
                    node = node.ParentNode;
                    continue;
                }           

                xPath = GetOneXPath(node) + xPath;
                node = node.ParentNode;
            }

            return xPath;
        }
        public static bool IsText(string text)
        {
            return text == "#comment" || text == "#text" || text == "#document";
        }

        private static bool IsPassedTime(WhenCrawling when, DateTime old, bool isBoot)
        {
            DateTime next = new DateTime(old.Ticks);
            next = next.AddHours(when.Hour < 0 ? 0 : when.Hour);
            next = next.AddMinutes(when.Min < 0 ? 0 : when.Min);
            next = next.AddSeconds(when.Sec < 0 ? 0 : when.Sec);

            DateTime specificNext = new DateTime(
                old.Year,
                old.Month,
                old.Day,
                when.Hour < 0 ? 0 : when.Hour,
                when.Min < 0 ? 0 : when.Min,
                when.Sec < 0 ? 0 : when.Sec);
            

            if (isBoot)
            {
                switch (when.When)
                {
                    case Enums.TimeSpanType.BOOT_PROGRAM:
                        return true;
                    case Enums.TimeSpanType.SPECIFIC_TIME_FORCE:
                        specificNext = specificNext.AddDays((((int)when.Week - (int)old.DayOfWeek) % 7 + 7) % 7);
                        if (specificNext < old) return true;
                        else if (next.Day == DateTime.Now.Day && specificNext < DateTime.Now) return true;
                        break;
                }
            }
            else
            {     
                switch (when.When)
                {
                    case Enums.TimeSpanType.PER_FEW_MINUTES:
                        return (when.Week == Enums.Week.NONE || (int)when.Week == (int)DateTime.Now.DayOfWeek)
                            && DateTime.Now >= next;
                    case Enums.TimeSpanType.SPECIFIC_TIME:
                    case Enums.TimeSpanType.SPECIFIC_TIME_FORCE:
                        return (when.Week == Enums.Week.NONE || (int)when.Week == (int)DateTime.Now.DayOfWeek)
                            && (when.Hour < 0 || next.Hour == DateTime.Now.Hour)
                            && (when.Min < 0 || next.Minute == DateTime.Now.Minute)
                            && (when.Sec < 0 || next.Second == DateTime.Now.Second);                   
                }
            }
            return false;
        }

        public static List<Word> CrawlingOne(Crawling crawling, string urlOption)
        {          
            if (crawling == null) return new List<Word>();
            
            crawling.Url.Option = urlOption;
            var html = InfiniteDownloadHtml(crawling.Url.ToString());
            var nodes = GetResults(html, crawling.BaseXPath);          
            
            if(nodes ==null || nodes.Count < 1) return new List<Word>();

            EncodingWordHelper.SetOriginalWords(crawling.WordList, nodes[0]);
            EncodingWordHelper.SetEncodedWords(crawling.WordList);

            return crawling.WordList;
        }

        public static void StartCrawlings(WhenCrawling when ,Crawling crawling, List<string> indexs, bool isBoot)
        {
            if (crawling.UrlOptionLIst.Count == 0)
            {
                crawling.UrlOptionLIst.Add(new UrlOption());                
            }

            try
            {
                foreach (UrlOption urlOption in crawling.UrlOptionLIst)
                {
                    if (IsPassedTime(when, urlOption.LastCrawling, isBoot))
                    {
                        bool success = StasrtCrawling(urlOption.Option, crawling, indexs);
                        if (success)
                        {
                            urlOption.LastCrawling = DateTime.Now;
                            FileHelper.SaveFile(crawling);
                        }

                    }
                }
            }
            catch { }            
        }

        public static bool StasrtCrawling(string optionUrl, Crawling crawling, List<string> indexs)
        {
            crawling.Url.Option = optionUrl;
            HtmlDocument html = DownloadHtml(crawling.Url.ToString());
            if (!IsSuccess(html)) return false;

            List<List<string>> results = EncodingWordHelper.GetEncodedWords(
                html,
                crawling.WordList,
                crawling.OtherCrawlingList,
                crawling.BaseXPath);

            foreach (BaseAction action in crawling.ActionList)
            {
                List<Word> words = crawling.WordList
                    .Concat(crawling.OtherCrawlingList
                    .SelectMany(x => x.CrawlingPointer.WordList)).ToList();
                ExcuteAction(
                    action,
                    words,
                    words.Find(x=>x.IsIndex),
                    crawling,
                    optionUrl,
                    results,
                    indexs);
            }

            return true;
        }

        public static void ExcuteAction(
            BaseAction action, 
            List<Word> words,
            Word indexWord,
            Crawling crawling,
            string option,
            List<List<string>> results,
            List<string> indexs)
        {

            switch (action.ActionType)
            {
                case Enums.BaseActionType.NOTIFITY:
                    NotificationHelper.CreateCollection(crawling.Name, option);
                    break;
            }

            foreach (var result in results)
            {
                for (int i = 0; i < words.Count; i++)
                {
                    words[i].Encoded = result[i];
                }
              
                if (indexWord != null && !indexs.Contains(indexWord.Encoded))
                {
                    switch (action.ActionType)
                    {
                        case Enums.BaseActionType.NOTIFITY:
                            NotificationHelper.SendToastAsync(action as MyToast, crawling.Name, option);
                            break;
                    }
                    indexs.Add(indexWord.Encoded);
                    FileHelper.SaveIndexFile(crawling, indexWord.Encoded);
                }      
            }            
        }
    }
}
