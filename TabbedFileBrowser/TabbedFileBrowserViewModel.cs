﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace TabbedFileBrowser
{
    public class TabbedFileBrowserViewModel : ITabbedFileBrowserViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Public properties
        public IReadOnlyList<ITabViewModel> Tabs => tabs;
        public ITabViewModel CurrentTab => Tabs[SelectedTabIndex];

        public int SelectedTabIndex { get; set; }

        // Private fields
        private ObservableCollection<ITabViewModel> tabs = new ObservableCollection<ITabViewModel>();
        
        // Constructor
        public TabbedFileBrowserViewModel()
        {
            // Start out with one tab pointed at the working directory
            var workingDir = Directory.GetCurrentDirectory();
            var initialTab = new TabViewModel(workingDir);

            tabs.Add(initialTab);
        }


        // Public interface methods

        public void NewTab(string folderPath)
        {
            var tab = new TabViewModel(folderPath);
            tabs.Insert(SelectedTabIndex + 1, tab);
        }

        public void CloseTab(int index)
        {
            // Don't close the tab if it's the last one.
            if (tabs.Count == 1)
                return;

            // Shift the selected tab to the left by one, if needed.
            int newTabCount = tabs.Count - 1;
            int newSelectedIndex = SelectedTabIndex;

            if (index < SelectedTabIndex)
                newSelectedIndex--;

            // Make sure the selected index stays in bounds.
            if (newSelectedIndex < 0)
                newSelectedIndex = 0;

            if (newSelectedIndex >= newTabCount)
                newSelectedIndex = newTabCount - 1;

            // Apply the change to the selected index
            tabs.RemoveAt(index);
            SelectedTabIndex = newSelectedIndex;
        }
    }
}