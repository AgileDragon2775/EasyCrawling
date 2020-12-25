using System;
using System.Windows;

namespace EasyCrawling
{
    public class NotifyIconForm
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem closeMenuItem;
        private System.Windows.Forms.MenuItem openMenuItem;

        private System.ComponentModel.IContainer components;

        public NotifyIconForm()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.closeMenuItem = new System.Windows.Forms.MenuItem();
            this.openMenuItem = new System.Windows.Forms.MenuItem();


            // Initialize contextMenu
            this.contextMenu.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.openMenuItem, this.closeMenuItem });

            // Initialize menuItems
            this.openMenuItem.Index = 0;
            this.openMenuItem.Text = "&Show";
            this.openMenuItem.Click += new EventHandler(this.openMenuItem_Click);

            this.closeMenuItem.Index = 1;
            this.closeMenuItem.Text = "E&xit";
            this.closeMenuItem.Click += new EventHandler(this.closeMenuItem_Click);

            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);

            notifyIcon.Icon = Properties.Resources.Icon1;
            notifyIcon.Text = "EasyCrawling";
            notifyIcon.ContextMenu = this.contextMenu;
            notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_DoubleClick);
        }

        private void notifyIcon_DoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.Activate();
            }
        }

        public void Show()
        {
            notifyIcon.Visible = true;
        }

        private void closeMenuItem_Click(object Sender, EventArgs e)
        {
            notifyIcon.Dispose();
            Application.Current.Shutdown();
        }

        private void openMenuItem_Click(object Sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.Activate();
        }
    }
}
