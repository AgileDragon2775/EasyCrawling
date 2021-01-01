using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using EasyCrawling.Enums;
using EasyCrawling.Helpers;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EasyCrawling
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CrawlingWindow : Window
    {
        public CrawlingWindow()
        {
            InitializeComponent();           
        }       

        
        private void dockManager_DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            var nowButton = SearchToggleButton(e.Document.ContentId);
            nowButton.IsChecked = false;
        }

        private void OnDumpToConsole(object sender, RoutedEventArgs e)
        {
            // Uncomment when TRACE is activated on AvalonDock project
            // dockManager.Layout.ConsoleDump(0);
        }

        private void OnUnloadManager(object sender, RoutedEventArgs e)
        {
            if (layoutRoot.Children.Contains(dockManager))
                layoutRoot.Children.Remove(dockManager);
        }

        private void OnLoadManager(object sender, RoutedEventArgs e)
        {
            if (!layoutRoot.Children.Contains(dockManager))
                layoutRoot.Children.Add(dockManager);
        }

        private void OnLayoutRootPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var activeContent = ((LayoutRoot)sender).ActiveContent;
            if (e.PropertyName == "ActiveContent")
            {
                Debug.WriteLine(string.Format("ActiveContent-> {0}", activeContent));
            }
        }
        private void OnLoadLayout(object sender, RoutedEventArgs e)
        {           
            string fileName = (sender as MenuItem).Header.ToString();

            InitDocuments(fileName);
            LoadLayout(fileName);            
        }

        private void OnSaveLayout(object sender, RoutedEventArgs e)
        {
            string fileName = (sender as MenuItem).Header.ToString();
            var serializer = new XmlLayoutSerializer(dockManager);
            try
            {
                using (var stream = new StreamWriter(string.Format(FileHelper.BaseDirectory + @".\AvalonDock_{0}.config", fileName)))
                    serializer.Serialize(stream);
            }
            catch (Exception ioex)
            {
                MessageBox.Show("An error occurred while writing the file." + ioex);
            }
        }
        private void InitDocuments(string fileName)
        {
            try
            {
                using (var stream = new StreamReader(
                    string.Format(string.Format(FileHelper.BaseDirectory + @".\AvalonDock_{0}.config", fileName))))
                {
                    LoadDocument(stream);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while reading the file." + ex);
            }
        }
        private void InitDocuments(TextReader text)
        {
            LoadDocument(text);
        }

        private void LoadLayout(string fileName)
        {
            var serializer = new XmlLayoutSerializer(dockManager);

            try
            {
                using (var stream = new StreamReader(string.Format(FileHelper.BaseDirectory + @".\AvalonDock_{0}.config", fileName)))
                    serializer.Deserialize(stream);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while reading the file." + ex);
            }
        }

        private void LoadLayout(TextReader text)
        {
            var serializer = new XmlLayoutSerializer(dockManager);                
            serializer.Deserialize(text);
        }

        private void LoadDocument(TextReader reader)
        {
            try
            {
                var layoutSerializer = new XmlSerializer(typeof(LayoutRoot));
                var layout = layoutSerializer.Deserialize(reader) as LayoutRoot;

                foreach (var doc in layout.Descendents().OfType<LayoutDocument>().Where(lc => lc.Content == null).ToArray())
                {
                    MyDoc myDoc;

                    if (Enum.TryParse(doc.ContentId, out myDoc))
                    {
                        var toggleButton = SearchToggleButton(myDoc.ToString());
                        AddDocument(myDoc);
                    }
                }

                foreach (var btn in FindVisualChildren<ToggleButton>(this))
                {
                    if (btn.Tag == null) continue;

                    if (SearchDocument(btn.Tag.ToString(), layout) == null)
                    {
                        btn.IsChecked = false;                       
                    }
                }                
            }
            catch { }
        }

        private void AddDocument(MyDoc docType)
        {           
            if (SearchDocument(docType.ToString(), dockManager.Layout) == null)
            {
                /*Get Corresponding  View*/
                string userString = EnumHelper.StringValueOf(docType, ReturnType.CLASS);
                Type userClass = Type.GetType(userString);

                /*Init VIew*/
                UserControl user =
                        Activator.CreateInstance(userClass) as UserControl;

                /*Add View*/
                LayoutDocument doc = new LayoutDocument();
                doc.Content = user;
                doc.ContentId = docType.ToString();
                doc.Title = EnumHelper.StringValueOf(docType);               

                var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                firstDocumentPane.Children.Add(doc);
            }

            var nowButton = SearchToggleButton(docType.ToString());
            if (nowButton != null)
                nowButton.IsChecked = true;
        }

        private void CloseDocument(MyDoc docType)
        {
            var searched = SearchDocument(docType.ToString(), dockManager.Layout);

            if(searched != null)
            {
                searched.Close();
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton now = (sender as ToggleButton);         

            if (now.Tag == null) return;

            if (now.IsChecked == true)
            {
                AddDocument((MyDoc)now.Tag);
            }
            else
            {
                CloseDocument((MyDoc)now.Tag);
            }
        }

        private void ToggleButton_Loaded(object sender, RoutedEventArgs e)
        {
            ToggleButton now = (sender as ToggleButton);

            var nowButton = SearchToggleButton(now.Tag.ToString());
            if (nowButton != null)
                nowButton.IsChecked = true;
        }

        private void dockManager_Loaded(object sender, RoutedEventArgs e)
        {
            InitDocuments(new StringReader(Properties.Resources.AvalonDock_Layout_base));
            LoadLayout(new StringReader(Properties.Resources.AvalonDock_Layout_base));
        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        private ToggleButton SearchToggleButton(string searchName)
        {
            foreach (var btn in FindVisualChildren<ToggleButton>(this))
            {
                if (btn.Tag == null) continue;

                if (btn.Tag.ToString() == searchName)
                {
                    return btn;
                }
            }
            return null;
        }

        private LayoutDocument SearchDocument(string contentName, LayoutRoot layout)
        {
            var currentContentsList = layout.Descendents().OfType<LayoutContent>().Where(c => c.ContentId != null).ToArray();

            if (currentContentsList != null)
            {
                foreach (LayoutDocument content in currentContentsList)
                {
                    if (content.ContentId == contentName)
                    {
                        return content;
                    }
                }
            }
            return null;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Focus();
        }
    }
}
