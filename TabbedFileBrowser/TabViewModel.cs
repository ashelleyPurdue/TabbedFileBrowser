using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedFileBrowser
{
    public class TabViewModel : ITabViewModel
    {
        public IEnumerable<FileSystemInfo> VisibleFiles { get; set; }

        public string CurrentFolder { get; private set; }
        public string Title         => throw new NotImplementedException();

        public void MoveBack()      => throw new NotImplementedException();
        public void MoveForward()   => throw new NotImplementedException();
        public void MoveUp()        => throw new NotImplementedException();

        public TabViewModel(string startFolder)
        {
            NavigateTo(startFolder);
        }

        public void NavigateTo(string path)
        {
            // TODO: Push this to a navigation stack of some sort
            // instead of directly changing the folder
            CurrentFolder = Path.GetFullPath(path);
            Refresh();
        }

        public void Refresh()
        {
            // TODO: Apply filtering and sorting
            var currentFolder = new DirectoryInfo(CurrentFolder);
            VisibleFiles = currentFolder.EnumerateFileSystemInfos();
        }
    }
}
