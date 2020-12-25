using EasyCrawling.Models;
using EasyCrawling.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyCrawling
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(System.Collections.Generic.List<CrawlingAndIndexs> crawlings)
        {
            InitializeComponent();

            this.DataContext = new MainViewModel(crawlings);
        }

        private void DataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    if (!dgr.IsMouseOver)
                    {
                        dgr.IsSelected = false;
                    }
                }
            }            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            Focus();
        }
    }
}
