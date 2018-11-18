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

namespace TabbedFileBrowser
{
    /// <summary>
    /// Interaction logic for TabbedFileBrowser.xaml
    /// </summary>
    public partial class TabbedFileBrowserControl : UserControl
    {
        public ITabbedFileBrowserViewModel ViewModel { get; private set; }

        public TabbedFileBrowserControl()
        {
            InitializeComponent();

            ViewModel = new TabbedFileBrowserViewModel();
            DataContext = ViewModel;
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
            if (e.Key == Key.Enter)
                ViewModel.CurrentTab.NavigateTo(currentPathBox.Text);
                // TODO: Validate the input first
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
    }
}
