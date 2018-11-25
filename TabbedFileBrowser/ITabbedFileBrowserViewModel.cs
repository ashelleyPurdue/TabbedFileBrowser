using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace TabbedFileBrowser
{
    public interface ITabbedFileBrowserViewModel : INotifyPropertyChanged
    {
        IReadOnlyList<ITabViewModel> Tabs { get; }
        int SelectedTabIndex { get; set; }
        ITabViewModel CurrentTab { get; }

        int SelectedFileIndex { get; set; }
        FileSystemInfo SelectedFile { get; }
        
        FilterStringParser ParseFilterString { get; set; }

        bool OpenNewTabContextMenuEnabled { get; }
        bool PasteEnabled { get; }

        void NewTab(string folderPath);
        void CloseTab(int index);
    }
}
