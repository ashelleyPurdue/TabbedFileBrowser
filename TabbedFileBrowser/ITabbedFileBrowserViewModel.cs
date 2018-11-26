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

        /// <summary>
        /// Copies the specified file to the clipboard
        /// </summary>
        /// <param name="file"></param>
        void CopyFile(FileSystemInfo file);

        /// <summary>
        /// Cuts the specified file to the clipboard
        /// </summary>
        /// <param name="file"></param>
        void CutFile(FileSystemInfo file);

        /// <summary>
        /// Pastes the file currently on the clipboard.
        /// The file will be pasted to the current tab's
        /// current folder.
        /// </summary>
        void PasteFile();

        void NewTab(string folderPath);
        void CloseTab(int index);
    }
}
