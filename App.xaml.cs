using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using EasyCrawling.Helpers;
using EasyCrawling.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;

namespace EasyCrawling
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private System.Timers.Timer _timer;
        private List<CrawlingAndIndexs> crawlings;

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Contains("-ToastActivated"))
            {
                StartTray();
            }
            else
            {
                StartTray();
            }
        }

        private void StartMainWindow()
        {
            bool bNew;
            Mutex mutex = new Mutex(true, "EasyCrawling", out bNew);
            if (bNew)
            {
                if (Current.MainWindow == null)
                {
                    new MainWindow(crawlings).Show();
                }
                else
                {
                    Current.MainWindow.Show();
                }
                mutex.ReleaseMutex();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void StartTray()
        {
            bool bNew;
            Mutex mutex = new Mutex(true, "EasyCrawlingTray", out bNew);
            if (bNew)
            {
                Init();
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Already running EasyCrawling");
                Environment.Exit(0);
            }
        }

        private void Init()
        {
            NotificationHelper.RegisterNotificationActivator();

            crawlings = new List<CrawlingAndIndexs>(
                FileHelper.LoadFiles()
                .Select(x => new CrawlingAndIndexs(x)));

            //RegisterStatupIfNeed();
            InitUstingName();
            Start(true);

            new MainWindow(crawlings);
            new NotifyIconForm().Show();

            _timer = new System.Timers.Timer(250);
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Enabled = true;
        }

        private void RegisterStatupIfNeed()
        {
            try
            {
                using(RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    key.SetValue("EasyCrawling", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
                    key.Close();
                }
            }
            catch
            {
                MessageBox.Show("Add StartUp Fail");
            }

            System.Security.Principal.WindowsIdentity user = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(user);
            var isAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        public void InitUstingName()
        {
            foreach (CrawlingAndIndexs crawling in crawlings)
            {
                foreach (var other in crawling.Crawling.OtherCrawlingList)
                {
                    try
                    {
                        other.CrawlingPointer = crawlings.First(x => x.Crawling.Name == other.CrawlingName).Crawling;
                    }
                    catch
                    {
                        other.CrawlingPointer = new Crawling();
                    }                  
                    
                    other.UrlOption.Pointer = EncodingWordHelper.SearchSameWord(
                        crawling.Crawling.WordList,
                        other.UrlOption.Name);
                }

                foreach (var action in crawling.Crawling.ActionList)
                {
                    switch (action.ActionType)
                    {
                        case Enums.BaseActionType.NOTIFITY:
                            InitToastUsingName(
                                action as MyToast,
                                crawling.Crawling.WordList
                                .Concat(crawling.Crawling.OtherCrawlingList
                                .SelectMany(x => x.CrawlingPointer.WordList)
                                .Select(c => c)).ToList());
                            break;
                    }
                }
            }
        }

        public void InitToastUsingName(MyToast toast, List<Word> words)
        {
            foreach (var action in toast.ActionList)
            {
                action.Pointer = EncodingWordHelper.SearchSameWord(words, action.Name);
            }

            foreach (var visual in toast.VisualList)
            {
                visual.Pointer = EncodingWordHelper.SearchSameWord(words, visual.Name);
            }
        }

        private void Start(bool isBoot)
        {
            foreach (var now in crawlings)
            {
                foreach (var when in now.Crawling.WhenList)
                {
                    if (!now.Crawling.IsDoing && now.Crawling.IsStarted)
                    {
                        now.Crawling.IsDoing = true;
                        CrawlingHelper.StartCrawlings(when, now.Crawling, now.indexs, isBoot);
                        now.Crawling.IsDoing = false;
                    }
                }
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Start(false);
        }
    }
}
