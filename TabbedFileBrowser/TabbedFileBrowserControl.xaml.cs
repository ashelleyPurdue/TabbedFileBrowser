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

        private List<MenuItem> originalContextMenu = new List<MenuItem>();

        public TabbedFileBrowserControl()
        {
            InitializeComponent();

            ViewModel = new TabbedFileBrowserViewModel();
            DataContext = ViewModel;

            // Save the original context menu so it can be restored
            // every time it is opened
            ContextMenu menu = FindResource("fileContextMenu") as ContextMenu;
            foreach (MenuItem i in menu.Items)
                originalContextMenu.Add(i);
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

        private void File_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var item = (ListBoxItem)sender;
            var file = (FileSystemInfo)(item.Content);
            var contextMenu = item.ContextMenu;

            // Restore the original context menu
            contextMenu.Items.Clear();
            foreach (MenuItem i in originalContextMenu)
                contextMenu.Items.Add(i);

            // TODO: Make changes to it
            var openItem = new MenuItem()
            {
                Header = "Open"
            };
            contextMenu.Items.Insert(0, openItem);

            // Give the application a chance to make their own changes to it
            FileContextMenuOpening?.Invoke(file, contextMenu);
        }
    }
}
