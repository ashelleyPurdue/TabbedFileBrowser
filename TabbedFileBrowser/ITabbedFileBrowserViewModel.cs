using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TabbedFileBrowser
{
    public interface ITabbedFileBrowserViewModel : INotifyPropertyChanged
    {
        IReadOnlyList<ITabViewModel> Tabs { get; }
        int SelectedTabIndex { get; set; }

        ITabViewModel CurrentTab { get; }

        void NewTab(string folderPath);
        void CloseTab(int index);
    }
}
