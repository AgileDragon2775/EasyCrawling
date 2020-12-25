using EasyCrawling.Enums;
using EasyCrawling.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace EasyCrawling.Helpers
{
    public class EncodingWordHelper
    {        
        public static string GetEncodedWord(Word word, List<Word> wordList)
        {
            return GetEncodedWord(word, wordList, null);
        }
        public static string GetEncodedWord(Word word, List<Word> wordList, int[] usedList)
        {
            word.Encoded = word.Original;
            foreach (WordOption option in word.Options)
            {
                EncodeWord(
                    word,
                    option,
                    wordList,                    
                    usedList == null ? new int[wordList.Count] : usedList);
            }
            return word.Encoded;
        }

        private static void EncodeWord(Word word, WordOption option, List<Word> wordList, int[] usedList)
        {
            switch (option.Type)
            {
                case EncodingOptionType.ADD_WORD:
                    AddUsingTwoWord(word, option.LeftWord, option.RightWord);
                    break;
                case EncodingOptionType.ADD_NUMBERING:
                    AddUsingNumbering(word, option.LeftNum, option.RightNum, wordList, usedList);
                    break;
                case EncodingOptionType.ADD_NAME:
                    AddUsingName(word, option.LeftWord, option.RightWord, wordList, usedList);
                    break;
                case EncodingOptionType.CUT_WORD:
                    CutUsingTwoWord(word, option.LeftWord, option.RightWord);
                    break;
                case EncodingOptionType.CUT_UNTIL_WORD:
                    CutUntilTwoWord(word, option.LeftWord, option.RightWord);
                    break;
                case EncodingOptionType.CUT_POSITION:
                    CutUsingTwoPosition(word, option.LeftNum, option.RightNum);
                    break;               
                default:
                    AddUsingTwoWord(word, "", "");
                    break;
            }
        }

        private static void AddUsingName(Word word, string leftText, string rightText, List<Word> wordList, int[] usedList)
        {
            for (int index = 0; index < wordList.Count; index++)
            {
                if (wordList[index].Name == leftText &&
                    !IsRepeat(index, usedList))
                {
                    usedList[index]++;
                    word.Encoded = GetEncodedWord(wordList[index], wordList, usedList) + word.Encoded;
                }
                if (wordList[index].Name == rightText &&
                    !IsRepeat(index, usedList))
                {
                    usedList[index]++;
                    word.Encoded = word.Encoded + GetEncodedWord(wordList[index], wordList, usedList);
                }
            }           
        }

        private static void AddUsingNumbering(Word word, int leftNum, int rightNum, List<Word> wordList, int[] usedList)
        {
            for (int index = 0; index < wordList.Count; index++)
            {
                if (wordList[index].Numbering == leftNum &&
                    !IsRepeat(index, usedList))
                {
                    usedList[index]++;
                    word.Encoded = GetEncodedWord(wordList[index], wordList, usedList) + word.Encoded;                    
                }
                if (wordList[index].Numbering == rightNum &&
                    !IsRepeat(index, usedList))
                {
                    usedList[index]++;
                    word.Encoded = word.Encoded + GetEncodedWord(wordList[index], wordList, usedList);
                }
            }
        }

        private static void AddUsingTwoWord(Word word, string leftText, string rightText)
        {
            word.Encoded =
                leftText +
                word.Encoded +
                rightText;
        }

        private static void CutUsingTwoWord(Word word, string leftText, string rightText)
        {
            if (word.Encoded == null) return;

            if (!string.IsNullOrEmpty(leftText))
            {
                int startIndex = word.Encoded.IndexOf(leftText);
                if (leftText.Length != 0 && startIndex != -1 && startIndex + leftText.Length <= word.Encoded.Length)
                {
                    word.Encoded = word.Encoded.Substring(startIndex + leftText.Length);
                }
            }

            if (!string.IsNullOrEmpty(rightText))
            {
                int endIndex = word.Encoded.LastIndexOf(rightText);               
                if (rightText.Length != 0 && endIndex != -1)
                {
                    word.Encoded = word.Encoded.Substring(0, endIndex);
                }
            }
        }

        private static void CutUntilTwoWord(Word word, string leftText, string rightText)
        {
            if (word.Encoded == null) return;

            if (!string.IsNullOrEmpty(leftText))
            {
                int startIndex = word.Encoded.IndexOf(leftText);

                if(startIndex == -1)
                {
                    word.Encoded = "";
                    return;
                }
                if (leftText.Length != 0 && startIndex != -1 && startIndex + leftText.Length <= word.Encoded.Length)
                {
                    word.Encoded = word.Encoded.Substring(startIndex + leftText.Length);
                }
            }

            if (!string.IsNullOrEmpty(rightText))
            {
                int endIndex = word.Encoded.IndexOf(rightText);
                if (endIndex == -1)
                {
                    word.Encoded = "";
                    return;
                }
                if (rightText.Length != 0 && endIndex != -1)
                {
                    word.Encoded = word.Encoded.Substring(0, endIndex);
                }
            }
        }

        private static void CutUsingTwoPosition(Word word, int startNum, int endNum)
        {
            if (word.Encoded == null) return;

            if (0 < endNum && endNum < word.Encoded.Length)
            {
                word.Encoded = word.Encoded.Substring(0, endNum);
            }
            if (0 < startNum && startNum < word.Encoded.Length)
            {
                word.Encoded = word.Encoded.Substring(startNum);
            }           
        }

        public static void SetOriginalWords(List<Word> wordList, HtmlNode nowNode)
        {
            foreach(Word word in wordList)
            {
                word.Original = CrawlingHelper.GetIfExist(nowNode, word.Tag);
            }
        }

        public static void SetEncodedWords(List<Word> wordList)
        {
            for(int i = 0; i < wordList.Count; i++)
            {
                wordList[i].Encoded = GetEncodedWord(wordList[i], wordList);
            }       
        }

        private static bool IsExistExceptWord(List<Word> words)
        {
            foreach (var word in words)
            {
                if (word.Excepts.Any(x => x.Except == word.Original))
                    return true;
            }
            return false;
        }

        public static List<List<string>> GetEncodedWords(
            HtmlDocument html, 
            List<Word> wordList,
            List<CrawlingInfo> otherInfoList,
            string xPath)
        {
            HtmlNodeCollection nodes = CrawlingHelper.GetResults(html, xPath);
            List<List<string>> results = new List<List<string>>();
            
            if (nodes == null) return results;

            foreach (HtmlNode nowNode in nodes)
            {
                List<string> otherList = new List<string>();
                bool isExcept = false;
                SetOriginalWords(wordList, nowNode);               
                SetEncodedWords(wordList);

                if (IsExistExceptWord(wordList)) continue;
                
                foreach (CrawlingInfo crawlingInfo in otherInfoList)
                {
                    var other = CrawlingHelper.CrawlingOne(crawlingInfo.CrawlingPointer, crawlingInfo.UrlOption.ToString());
                    if (IsExistExceptWord(other))
                    {
                        isExcept = true;
                        break;
                    }
                    foreach (Word word in other)
                    {
                        otherList.Add(word.Encoded);
                    }
                }

                if (isExcept) continue;

                results.Add(wordList.Select(x => x.Encoded).Concat(otherList).ToList());
            }

            return results;
        }

        public static Word SearchSameWord(List<Word> encodedList, string searchWord)
        {
            int searcgIndex;
            bool success = int.TryParse(searchWord, out searcgIndex);
            foreach (Word encoded in encodedList)
            {
                if (success &&
                    searcgIndex == encoded.Numbering)
                {
                    return encoded;
                }
                if (searchWord == encoded.Name)
                {
                    return encoded;
                }
            }

            return null;
        }

        public static bool IsRepeat(int index, int[] usedList)
        {
            return usedList[index] > 5 ? true : false;
        }
    }
}
