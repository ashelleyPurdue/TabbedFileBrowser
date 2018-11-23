﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace TabbedFileBrowser
{
    public interface ITabbedFileBrowserViewModel : INotifyPropertyChanged
    {
        IReadOnlyList<ITabViewModel> Tabs { get; }

        int SelectedTabIndex { get; set; }

        int SelectedFileIndex { get; set; }
        FileSystemInfo SelectedFile { get; }

        ITabViewModel CurrentTab { get; }

        void NewTab(string folderPath);
        void CloseTab(int index);
    }
}
