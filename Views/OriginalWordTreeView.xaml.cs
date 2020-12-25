using EasyCrawling.Helpers;
using EasyCrawling.ViewModels.Base;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace EasyCrawling.Views
{
    /// <summary>
    /// OriginalWordTreeView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OriginalWordTreeView : UserControl
    {
        public OriginalWordTreeView()
        {
            InitializeComponent();
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
        }
        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
        private void originalWordTreeControl_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void originalWordTreeControl_Loaded(object sender, RoutedEventArgs e)
        {
            Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            searchPanel.Visibility = Visibility.Hidden;
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            tvItem.BringIntoView();
            e.Handled = true;
        }

    }
}
