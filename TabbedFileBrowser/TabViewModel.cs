using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace TabbedFileBrowser
{
    internal class TabViewModel : ITabViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<FileSystemInfo> VisibleFiles { get; set; }

        public string CurrentFolder { get; private set; }
        public string Title
        {
            get
            {
                string noTrailingSlash = CurrentFolder.TrimEnd(Path.DirectorySeparatorChar);
                return Path.GetFileName(noTrailingSlash);
            }
        }

        public string FilterString { get; set; }

        [DependsOn("CurrentFolder")] public bool HasPrevFolder => history.Count > 0;
        [DependsOn("CurrentFolder")] public bool HasNextFolder => futureHistory.Count > 0;
        [DependsOn("CurrentFolder")] public bool HasParentFolder => Path.GetDirectoryName(CurrentFolder) != null;

        private ITabbedFileBrowserViewModel parent;

        private Stack<string> history       = new Stack<string>();
        private Stack<string> futureHistory = new Stack<string>();


        public TabViewModel(ITabbedFileBrowserViewModel parent, string startFolder)
        {
            CurrentFolder = Path.GetFullPath(startFolder);
            this.parent = parent;

            Refresh();
        }

        public void NavigateTo(string path)
        {
            futureHistory.Clear();
            history.Push(CurrentFolder);

            CurrentFolder = Path.GetFullPath(path);
            Refresh();
        }

        public void MoveBack()
        {
            futureHistory.Push(CurrentFolder);
            CurrentFolder = history.Pop();

            Refresh();
        }

        public void MoveForward()
        {
            history.Push(CurrentFolder);
            CurrentFolder = futureHistory.Pop();

            Refresh();
        }

        public void MoveUp()
        {
            // Navigate up to the parent folder
            NavigateTo(Path.GetDirectoryName(CurrentFolder));
        }

        public void Refresh()
        {
            // Parse the filter string to create a filtering function.
            FilterCondition matchesFilter = parent.ParseFilterString(FilterString);
            bool skipFiltering = String.IsNullOrWhiteSpace(FilterString);

            // Query the current folder for all files that match the filter,
            // and display them in the listbox.
            // TODO: Apply sorting too.
            var currentFolder = new DirectoryInfo(CurrentFolder);
            VisibleFiles = currentFolder.EnumerateFileSystemInfos()
                                        .Where(f => skipFiltering || matchesFilter(f));
        }

        public override string ToString() => Title;
    }
}
