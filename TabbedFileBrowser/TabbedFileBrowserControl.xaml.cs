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
using System.IO;

namespace TabbedFileBrowser
{
    /// <summary>
    /// Interaction logic for TabbedFileBrowser.xaml
    /// </summary>
    public partial class TabbedFileBrowserControl : UserControl
    {
        public ITabbedFileBrowserViewModel ViewModel { get; private set; }

        public event EventHandler<FileSystemInfo> FileDoubleClicked;

        public delegate void FileContextMenuOpeningHandler(FileSystemInfo file, ContextMenu menu);
        public event FileContextMenuOpeningHandler FileContextMenuOpening;

        public List<MenuItem> ExtraContextMenuItems { get; set; } = new List<MenuItem>();


        public TabbedFileBrowserControl()
        {
            InitializeComponent();

            ViewModel = new TabbedFileBrowserViewModel();
            DataContext = ViewModel;

            // Make sure the context menu is bound to the viewmodel
            ContextMenu menu = FindResource("fileContextMenu") as ContextMenu;
            menu.DataContext = ViewModel;
        }


        // Misc methods

        private int GetIndexOfCloseButton(Button closeButton)
        {
            IEnumerable<DependencyObject> GetChildren(DependencyObject parent)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    yield return VisualTreeHelper.GetChild(parent, i);
                }
            }

            bool HasCloseButtonAsChild(DependencyObject item)
            {
                // Base case: we *are* the close button
                if (item == closeButton)
                    return true;

                // Recursive case: search all the children
                foreach (var child in GetChildren(item))
                {
                    if (HasCloseButtonAsChild(child))
                        return true;
                }

                // We didn't find it, so false.
                return false;
            }

            var itemsInListbox = ViewModel.Tabs
                                            .Select(t => tabsList.ItemContainerGenerator.ContainerFromItem(t))
                                            .ToList();

            return itemsInListbox.FindIndex(HasCloseButtonAsChild);

            // We didn't find it, so throw
            throw new Exception("Couldn't find tab that goes with this close button.");
        }


        // Event handlers

        private void CloseTab_Click(object sender, RoutedEventArgs e)
        {
            int index = GetIndexOfCloseButton((Button)sender);
            ViewModel.CloseTab(index);
        }

        private void NewTab_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewTab(ViewModel.CurrentTab.CurrentFolder);
        }

        private void CurrentPathBox_KeyDown(object sender, KeyEventArgs e)
        {
            // TODO: Validate the input first
            if (e.Key == Key.Enter)
                ViewModel.CurrentTab.NavigateTo(currentPathBox.Text);
        }

        private void FilterTextbox_EnterPressed(object sender, KeyEventArgs e)
        {
            // TODO: Validate input
            if (e.Key != Key.Enter)
                return;

            ViewModel.CurrentTab.Refresh();
        }

        private void MoveBack_Click(object s, RoutedEventArgs a)    => ViewModel.CurrentTab.MoveBack();
        private void MoveUp_Click(object s, RoutedEventArgs a)      => ViewModel.CurrentTab.MoveUp();
        private void MoveForward_Click(object s, RoutedEventArgs a) => ViewModel.CurrentTab.MoveForward();

        private void tabsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // HACK: Don't let it get deselected!
            if (tabsList.SelectedIndex == -1)
            {
                tabsList.SelectedIndex = 0;
                return;
            }

            ViewModel.SelectedTabIndex = tabsList.SelectedIndex;
        }

        private void File_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var file = (sender as ListBoxItem).Content as FileSystemInfo;

            // Forward the event
            FileDoubleClicked?.Invoke(this, file);

            // If it's a folder, navigate there.
            if (file is DirectoryInfo dir)
                ViewModel.CurrentTab.NavigateTo(dir.FullName);
        }

        private bool alreadyAdded = false;
        private void File_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var item = (ListBoxItem)sender;
            var file = (FileSystemInfo)(item.Content);
            var contextMenu = item.ContextMenu;

            // Add the extra menu items, if they haven't been already
            if (!alreadyAdded && ExtraContextMenuItems.Count > 0)
            {
                alreadyAdded = true;

                contextMenu.Items.Add(new Separator());
                foreach (MenuItem i in ExtraContextMenuItems)
                    contextMenu.Items.Add(i);
            }

            // Give the application a chance to make their own changes to it
            FileContextMenuOpening?.Invoke(file, contextMenu);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileSystemInfo file = ViewModel.SelectedFile;

            // If it's a folder, navigate to it.
            if (file is DirectoryInfo)
            {
                ViewModel.CurrentTab.NavigateTo(file.FullName);
                return;
            }

            // TODO: If it's a shortcut, resolve it.

            // If it's a file, open it with the shell.
            System.Diagnostics.Process.Start(file.FullName);
        }

        private void OpenInNewTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string folder = ViewModel.SelectedFile.FullName;

            ViewModel.NewTab(folder);
            ViewModel.SelectedTabIndex++;
        }
    }
}
