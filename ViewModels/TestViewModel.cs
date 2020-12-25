using EasyCrawling.Helpers;
using EasyCrawling.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace EasyCrawling.ViewModels
{
    public class TestViewModel : Base.ViewModelBase
    {

        #region Fields
        
        private DataTable _tests = new DataTable();
        private bool _isAuto;
        private bool _isName = true;
        private bool _isTesting;

        RelayCommand _doTestCommand;
        RelayCommand _changedClassification;

        #endregion

        #region event

        public event EventHandler DoTest;
        public event EventHandler ClassificationChanged;
        public event EventHandler TestStarted;
        public event EventHandler TestEnded;

        #endregion

        #region Constructors

        #endregion

        #region Properties
        public DataView Tests
        {
            get => _tests.DefaultView;           
        }

        public bool IsAuto
        {
            get => _isAuto;
            set => Set(ref _isAuto, value);
        }

        public bool IsName
        {
            get => _isName;
            set => Set(ref _isName, value);
        }

        #endregion

        #region Command

        public RelayCommand DoTestCommand
        {
            get => _doTestCommand ??
                    (_doTestCommand = new RelayCommand(OnTest));
            set => Set(ref _doTestCommand, value);
        }

        public RelayCommand ClassificationChangedCommand
        {
            get => _changedClassification ??
                    (_changedClassification = new RelayCommand(OnChangeClassification));
            set => Set(ref _changedClassification, value);
        }

        #endregion

        #region method
      
        public void DoTestIfNeeded(
            HtmlDocument html,
            string xPath,
            bool isDirect,
            Crawling crawling)
        {
            if (IsAuto || isDirect)
            {
                TestStarted(null, null);
                LetTest(html, crawling.WordList, crawling.OtherCrawlingList, xPath);

                TestEnded(null, null);
            }
        }

        private void LetTest(
            HtmlDocument html,
            List<Word> wordList, 
            List<CrawlingInfo> otherList,
            string xPath)
        {
            if (_isTesting) return;
            _tests = new DataTable();

            string[,] backup = BackupWords(wordList);

            string[][] array = EncodingWordHelper.GetEncodedWords(
                html,
                wordList,
                otherList,
                xPath)
                .Select(a => a.ToArray()).ToArray();

            if (array == null)
            {
                RaisePropertyChanged("Tests");
                return;
            };

            var rowLength = array.Length;
            if (rowLength == 0)
            {
                RaisePropertyChanged("Tests");
                return;
            } 

            var columnLength = array[0].Length;           
            if (columnLength == 0) 
            {
                RaisePropertyChanged("Tests");
                return;
            }

            foreach(Word info in wordList)
            {
                _tests.Columns.Add(new DataColumn(IsName ? info.Name : info.Numbering.ToString()));
            }

            foreach (CrawlingInfo info in otherList)
            {
                if (info.CrawlingPointer != null)
                {
                    foreach (Word wordInfo in info.CrawlingPointer.WordList)
                    {
                        _tests.Columns.Add(new DataColumn(IsName ? wordInfo.Name : wordInfo.Numbering.ToString()));
                    }
                }
            }
                      
            for (int i = 0; i < rowLength; i++)
            {
                var newRow = _tests.NewRow();
                for (int j = 0; j < columnLength; j++)
                    newRow[j] = array[i][j];
                _tests.Rows.Add(newRow);
            }

            RecoveryWords(wordList, backup);

            _isTesting = false;

            RaisePropertyChanged("Tests");
        } 

        public void ChangeClassification(List<Word> wordList)
        {
            var columnLength = _tests.Columns.Count;
            foreach (DataColumn column in _tests.Columns)
            {
                column.ColumnName = IsName ? 
                    wordList.Find(x=>int.Parse(column.ColumnName) == x.Numbering).Name : 
                    wordList.Find(x=> column.ColumnName == x.Name).Numbering.ToString();
            }
        }

        private string[,] BackupWords(List<Word> words)
        {
            string[,] results = new string[words.Count,2];
            for(int i = 0; i < words.Count; i++)
            {
                results[i, 0] = words[i].Original;
                results[i, 1] = words[i].Encoded;
            }
            return results;
        }

        private void RecoveryWords(List<Word> words, string[,] backup)
        {
            for (int i = 0; i < words.Count; i++)
            {
                words[i].Original = backup[i, 0];
                words[i].Encoded = backup[i, 1];
            }
        }


        #endregion

        #region ExcuteCommand

        private void OnTest(object param)
        {
            DoTest(null, null);
        }

        private void OnChangeClassification(object param)
        {
            ClassificationChanged(null, null);
        }

        #endregion

        #region CanCommand

        #endregion
    }
}
