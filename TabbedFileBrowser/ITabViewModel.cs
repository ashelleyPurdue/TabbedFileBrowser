using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedFileBrowser
{
    public interface ITabViewModel : INotifyPropertyChanged
    {
        string Title { get; }
        string CurrentFolder { get; }
        string FilterString { get; set; }

        bool HasPrevFolder { get; }
        bool HasNextFolder { get; }
        bool HasParentFolder { get; }

        IEnumerable<FileSystemInfo> VisibleFiles { get; }

        void NavigateTo(string path);
        void MoveUp();
        void MoveForward();
        void MoveBack();
        void Refresh();
    }
}
