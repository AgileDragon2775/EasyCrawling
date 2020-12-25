using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyCrawling.Views
{
    /// <summary>
    /// HtmlVIew.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HtmlView : UserControl
    {
 
        private string tempWord;
        private List<int> searchedList;
        private int searchIndex;

        public HtmlView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            htmlSearchPanel.Visibility = Visibility.Hidden;
        }

        private void htmlControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                htmlSearchPanel.Visibility = (htmlSearchPanel.Visibility == Visibility.Visible) ?
                    Visibility.Hidden :
                    Visibility.Visible;
                searchTextBox.Focus();
            }
            else if (e.Key == Key.Escape)
            {
                htmlSearchPanel.Visibility = Visibility.Hidden;
            }
        }

        private void InitSearch()
        {
            tempWord = "";
            searchIndex = 0;
            searchButton.Content = " Search ";
        }

        private List<int> AllIndexesOf(string str, string value)
        {
            List<int> indexes = new List<int>();
            if (string.IsNullOrEmpty(value))
                return indexes;
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
 
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchWord = searchTextBox.Text;

            if (string.IsNullOrEmpty(searchWord)) return;

            if (tempWord != searchWord)
            {
                searchedList = AllIndexesOf(htmlTextBox.Text, searchWord);
            }

            if (searchedList.Count > 0)
            {                
                htmlTextBox.SelectionStart = searchedList[searchIndex % searchedList.Count];
                htmlTextBox.SelectionLength = searchWord.Length;
                htmlTextBox.ScrollToLine(htmlTextBox.GetLineIndexFromCharacterIndex(htmlTextBox.SelectionStart));

                searchButton.Content = " " + (searchIndex++) + " / " + searchedList.Count + " ";
                searchIndex = searchIndex % searchedList.Count;
            }

            tempWord = searchWord;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InitSearch();
        }

        private void htmlTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                searchButton_Click(sender, e);
            }
        }
    }
}
