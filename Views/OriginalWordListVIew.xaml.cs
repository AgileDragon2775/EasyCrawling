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
    /// OriginalWordListVIew.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OriginalWordListVIew : UserControl
    {
        public OriginalWordListVIew()
        {
            InitializeComponent();
        }

        private void originalWordListControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                searchPanel.Visibility = (searchPanel.Visibility == Visibility.Visible) ?
                    Visibility.Hidden :
                    Visibility.Visible;
                searchTextBox.Focus();
            }
            else if (e.Key == Key.Escape)
            {
                searchPanel.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            searchPanel.Visibility = Visibility.Hidden;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
   
            listBox.ScrollIntoView(listBox.SelectedItem);
        }
    }
}
