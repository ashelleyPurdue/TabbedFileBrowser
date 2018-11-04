using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TabbedFileBrowser
{
    public interface ITabbedFileBrowserViewModel
    {
        IReadOnlyList<ITabViewModel> Tabs { get; }
        int SelectedTabIndex { get; set; }

        ITabViewModel CurrentTab { get; }

        void NewTab(string folderPath);
        void CloseTab(int index);
    }

    public interface ITabViewModel
    {
        string CurrentFolder { get; }
        string Title { get; }

        IEnumerable<FileSystemInfo> VisibleFiles { get; }

        void NavigateTo(string path);
        void MoveUp();
        void MoveForward();
        void MoveBack();
        void Refresh();
    }
}
