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
