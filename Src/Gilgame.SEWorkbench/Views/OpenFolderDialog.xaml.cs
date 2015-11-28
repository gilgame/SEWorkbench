using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.Views
{
    public partial class OpenFolderDialog : Window
    {
        public string FolderName { get; set; }

        public OpenFolderDialog()
        {
            InitializeComponent();
            OpenDrives();
        }

        private void OpenDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                tvFileSystem.Items.Add(CreateTreeItem(drive));
            }
        }

        public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            if (item.Items.Count > 0 && item.Items[0] is string)
            {
                item.Items.Clear();

                DirectoryInfo expanded = null;
                if (item.Tag is DriveInfo)
                {
                    expanded = (item.Tag as DriveInfo).RootDirectory;
                }
                if (item.Tag is DirectoryInfo)
                {
                    expanded = (item.Tag as DirectoryInfo);
                }

                try
                {
                    foreach (DirectoryInfo directory in expanded.GetDirectories())
                    {
                        item.Items.Add(CreateTreeItem(directory));
                    }
                }
                catch (Exception)
                {
                    // do nothing
                }
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            PerformOpenFolder();
        }

        private void PerformOpenFolder()
        {
            string path = String.Empty;
            if (tvFileSystem.SelectedItem != null)
            {
                TreeViewItem item = (TreeViewItem)tvFileSystem.SelectedItem;
                path = GetFullPath(item);
            }
            if (!String.IsNullOrEmpty(path))
            {
                FolderName = path;
                DialogResult = true;
                Close();
            }
            else
            {
                Services.MessageBox.ShowMessage("Please choose a folder.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            PerformCancel();
        }

        private void PerformCancel()
        {
            DialogResult = false;
            Close();
        }

        private TreeViewItem CreateTreeItem(object o)
        {
            TreeViewItem item = new TreeViewItem()
            {
                Header = o.ToString(),
                Tag = o,
            };
            item.Items.Add("Loading...");

            return item;
        }

        private string GetFullPath(TreeViewItem node)
        {
            if (node == null)
            {
                return null;
            }

            var result = Convert.ToString(node.Header);

            for (var i = GetParent(node); i != null; i = GetParent(i))
            {
                result = Path.Combine(Convert.ToString(i.Header), result);
            }

            return result;
        }

        private TreeViewItem GetParent(TreeViewItem item)
        {
            if (item != null)
            {
                if (item.Parent != null)
                {
                    if (item.Parent is TreeViewItem)
                    {
                        return (TreeViewItem)item.Parent;
                    }
                }
            }
            return null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformOpenFolder();
            }
            if (e.Key == Key.Escape)
            {
                PerformCancel();
            }
        }
    }
}
