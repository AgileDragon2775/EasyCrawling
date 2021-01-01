using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using Microsoft.QueryStringDotNET; // QueryString.NET
using EasyCrawling.Models;
using EasyCrawling.Enums;
using System.Net;
using System.Threading.Tasks;

namespace EasyCrawling.Helpers
{
    public class NotificationHelper
    {
        public static ToastNotification ConvertToast(MyToast myToast)
        {
            Dictionary<int, Models.ToastVisual> visualDict = new Dictionary<int, Models.ToastVisual>();
            Dictionary<int, QueryString> actionDict = new Dictionary<int, QueryString>();

            ToastActionsCustom actions = new ToastActionsCustom();
            QueryString launch = new QueryString();
            Microsoft.Toolkit.Uwp.Notifications.ToastVisual visual = new Microsoft.Toolkit.Uwp.Notifications.ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = ""
                        }
                    }
                }
            };
            
            foreach (Models.ToastVisual nowVisual in myToast.VisualList)
            {
                switch (nowVisual.Visual)
                {
                    case VisualType.TITLE:
                        visual.BindingGeneric.Children[0] = new AdaptiveText()
                        {
                            Text = nowVisual.ToString()
                        };
                        break;
                    case VisualType.DETAIL:                        
                        visual.BindingGeneric.Children.Add(new AdaptiveText()
                        {
                            Text = nowVisual.ToString()
                        });
                        break;
                    case VisualType.MAIN_IMG:
                        visual.BindingGeneric.Children.Add(new AdaptiveImage()
                            {
                                Source = nowVisual.ToString()
                            });
                        break;
                    case VisualType.LOGO_IMG:
                        visual.BindingGeneric.AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = nowVisual.ToString(),
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        };
                        break;
                    case VisualType.BUTTON1:
                    case VisualType.BUTTON2:
                    case VisualType.BUTTON3:
                        int index = (int)nowVisual.Visual - (int)VisualType.BUTTON1;
                        if (!visualDict.ContainsKey(index))
                            visualDict.Add(index, nowVisual);
                        else
                            visualDict[index] = nowVisual;
                        break;
                }
            }     
            
            
            foreach (ToastAction nowAction in myToast.ActionList)
            {
                switch (nowAction.When)
                {
                    case WhenType.BODY:
                        launch.Add(new QueryString()
                        {
                            { "action", ((int)nowAction.ActionType).ToString() },
                            { "id", nowAction.ToString() },
                            { "folder", nowAction.Folder}
                        }.ToString());
                        break;
                    case WhenType.BUTTON1:
                    case WhenType.BUTTON2:
                    case WhenType.BUTTON3:
                        int when = (int)nowAction.When - (int)WhenType.BUTTON1;
                        if (!actionDict.ContainsKey(when))
                        {
                            actionDict.Add(when, new QueryString()
                            {
                                { "action", ((int)nowAction.ActionType).ToString() },
                                { "id", nowAction.ToString() },
                                { "folder", nowAction.Folder}
                            });
                        }
                        else
                        {
                            actionDict[when].Add("action", ((int)nowAction.ActionType).ToString());
                            actionDict[when].Add("id", nowAction.ToString());
                            actionDict[when].Add("folder", nowAction.Folder);
                        }           
                        break;
                }
            }

            /*Add button*/
            foreach (var nowVisual in visualDict.OrderBy(x => x.Key))
            {
                actions.Buttons.Add(
                    new ToastButton(nowVisual.Value.ToString(),
                        actionDict.ContainsKey(nowVisual.Key) == true ?
                        actionDict[nowVisual.Key].ToString() : "")
                );
            }

            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,
                Launch = launch.ToString(),                
            };
            
            // Create the XML document (BE SURE TO REFERENCE WINDOWS.DATA.XML.DOM)
            var doc = new XmlDocument();
            doc.LoadXml(toastContent.GetContent());

            // And create the toast notification
            var toast = new ToastNotification(doc);
            toast.ExpirationTime = DateTime.Now.AddDays(3);
 
            return toast;
        }
        
        private static ToastCollection ConvertCollection(string toastCollectionId, string displayName, string launchArg)
        {
            Uri icon = new Uri("ms-appx:///Assets/workEmail.png");

            return new ToastCollection(
                toastCollectionId,
                displayName,
                launchArg,
                icon);
        }

        public static void SendToastAsync(MyToast myToast, string name, string option)
        {
            var toast = ConvertToast(myToast);
            string displayName = string.IsNullOrEmpty(option) ? name : name + "::" + option;

            Send(toast, displayName);
        }

        public static async void CreateCollection(string name, string option)
        {
            string launchArg = "Collection";
            string displayName = string.IsNullOrEmpty(option) ? name : name + "::" + option;

            /*Register Group*/
            var workEmailToastCollection = ConvertCollection(displayName, displayName, launchArg);
            await ToastNotificationManager.GetDefault().GetToastCollectionManager().SaveToastCollectionAsync(workEmailToastCollection);
        }

        public static async Task Send(ToastNotification toast, string id)
        {            
            try
            {
                var notifier = await ToastNotificationManager.GetDefault().GetToastNotifierForToastCollectionIdAsync(id);
                notifier?.Show(toast);
            }
            catch(Exception e)
            {
                MessageBox.Show("Notification Error: "+e.ToString());
            }            
        }

        public static void RegisterNotificationActivator()
        {
            /*Register at Com*/
            DesktopNotificationManagerCompat.RegisterAumidAndComServer<MyNotificationActivator>("EasyCrawling.EasyCrawling");
            DesktopNotificationManagerCompat.RegisterActivator<MyNotificationActivator>();
        }


        [ClassInterface(ClassInterfaceType.None)]
        [ComSourceInterfaces(typeof(INotificationActivationCallback))]
        [Guid("5beb9544-7ed3-4142-8adf-071260a39cca"), ComVisible(true)]
        public class MyNotificationActivator : NotificationActivator
        {
            public override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
            {
                // Tapping on the top-level header launches with empty args
                if (arguments.Length == 0)
                {
                    OpenWindowIfNeeded();
                    return;
                }

                // Parse the query string (using NuGet package QueryString.NET)
                QueryString args = QueryString.Parse(WebUtility.UrlDecode(arguments));

                string url = args["id"];
                // See what action is being requested 
                switch ((BaseActionType)(int.Parse(args["action"])))
                {
                    case BaseActionType.OPEN_APP:
                        OpenWindowIfNeeded();     
                        break;
                    case BaseActionType.OPEN_URL:
                        Open(url);
                        break;
                    case BaseActionType.OPEN_FILE:
                        FileHelper.CreateDirectoryIfNeed(FileHelper.TempDirectory);
                        string tempFilePath = FileHelper.TempDirectory + "\\" + "temp." + url.Split('/').Last().Split('.').Last();
                        using (var client = new WebClient())                        
                            client.DownloadFile(url, tempFilePath);
                        System.Diagnostics.Process.Start(tempFilePath);
                        break;
                    case BaseActionType.DOWNLOAD_FILE:                     
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(url, args["folder"]);
                        }
                        break;
                }
            }
            
            private void OpenWindowIfNeeded()
            {               
                Application.Current.Dispatcher.Invoke(delegate
                {
                    Window owner = Application.Current.MainWindow;

                    // Use owner here - it must be used on the UI thread as well..

                    owner.Show();
                    owner.Activate();
                });
            }
            private void Open(string url)
            {
                try
                {
                    System.Diagnostics.Process.Start(url);                   
                }
                catch (System.ComponentModel.Win32Exception noBrowser)
                {
                    if (noBrowser.ErrorCode == -2147467259)
                        MessageBox.Show(noBrowser.Message);
                }
                catch (Exception other)
                {
                    MessageBox.Show(other.Message);
                }
            }           
        }
    }
}
