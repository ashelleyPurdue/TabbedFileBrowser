using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using PropertyChanged;

namespace TabbedFileBrowser
{
    internal class TabbedFileBrowserViewModel : ITabbedFileBrowserViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate IComparable SortMethod(FileSystemInfo file);

        // Public properties
        [AlsoNotifyFor("TabsWithNull")]
        public IReadOnlyList<ITabViewModel> Tabs => tabs;
        public ITabViewModel CurrentTab => Tabs[SelectedTabIndex];

        public int SelectedTabIndex { get; set; }

        public int SelectedFileIndex { get; set; }
        public FileSystemInfo SelectedFile { get; set; }

        public Dictionary<string, SortMethod> SortMethods { get; set; } = new Dictionary<string, SortMethod>()
        {
            {"Name", f => f.Name },
            {"Date", f => f.LastWriteTime }
        };

        public FilterStringParser ParseFilterString { get; set; } = DefaultFilterStringParser;

        [DependsOn("SelectedFileIndex")] public bool OpenNewTabContextMenuEnabled => SelectedFile is DirectoryInfo;

        // HACK: Expose a version of Tabs but with null appended to the end.
        // The null acts as a stand-in for the "+" button.  Yes, it's dirty.  Sue me.
        public IEnumerable<ITabViewModel> TabsWithNull => Tabs.Append(null);

        // Private fields
        private ObservableCollection<ITabViewModel> tabs = new ObservableCollection<ITabViewModel>();
        

        // Constructor
        public TabbedFileBrowserViewModel()
        {
            // Start out with one tab pointed at the working directory
            var workingDir = Directory.GetCurrentDirectory();
            var initialTab = new TabViewModel(this, workingDir);

            tabs.Add(initialTab);

            // Raise PropertyChanged when the contents of tabs changes.
            tabs.CollectionChanged += (s, a) =>
            {
                PropertyChanged?.Invoke
                (
                    this,
                    new PropertyChangedEventArgs("Tabs")
                );

                // TODO: Remove this when we remove the TabsWithNull hack.
                // That is, IF it ever gets removed.
                PropertyChanged?.Invoke
                (
                    this,
                    new PropertyChangedEventArgs("TabsWithNull")
                );
            };
        }


        // Public interface methods

        public void NewTab(string folderPath)
        {
            var tab = new TabViewModel(this, folderPath);
            tabs.Insert(SelectedTabIndex + 1, tab);
        }

        public void CloseTab(int index)
        {
            // Don't close the tab if it's the last one.
            if (tabs.Count == 1)
                return;

            // Shift the selected tab to the left by one, if needed.
            int newTabCount = tabs.Count - 1;
            int newSelectedIndex = SelectedTabIndex;

            if (index < SelectedTabIndex)
                newSelectedIndex--;

            // Make sure the selected index stays in bounds.
            if (newSelectedIndex < 0)
                newSelectedIndex = 0;

            if (newSelectedIndex >= newTabCount)
                newSelectedIndex = newTabCount - 1;

            // Apply the change to the selected index
            tabs.RemoveAt(index);
            SelectedTabIndex = newSelectedIndex;
        }


        // Misc methods

        private static FilterCondition DefaultFilterStringParser(string filterString)
        {
            return f => f.Name.Contains(filterString);
        }
    }
}
