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
    public class TabViewModel : ITabViewModel
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

        [DependsOn("CurrentFolder")] public bool HasPrevFolder => history.Count > 0;
        [DependsOn("CurrentFolder")] public bool HasNextFolder => futureHistory.Count > 0;

        [DependsOn("CurrentFolder")]
        public bool HasParentFolder => Path.GetDirectoryName(CurrentFolder) != null;

        private Stack<string> history       = new Stack<string>();
        private Stack<string> futureHistory = new Stack<string>();


        public TabViewModel(string startFolder)
        {
            CurrentFolder = Path.GetFullPath(startFolder);
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
            NavigateTo(Path.GetDirectoryName(CurrentFolder));
        }

        public void Refresh()
        {
            // TODO: Apply filtering and sorting
            var currentFolder = new DirectoryInfo(CurrentFolder);
            VisibleFiles = currentFolder.EnumerateFileSystemInfos();
        }

        public override string ToString() => Title;
    }
}
