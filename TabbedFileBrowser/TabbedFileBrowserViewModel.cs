using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

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
        private List<ITabViewModel> tabs = new List<ITabViewModel>();
        
        // Constructor
        public TabbedFileBrowserViewModel()
        {
            // Start out with one tab pointed at the working directory
            var workingDir = Directory.GetCurrentDirectory();
            var initialTab = new TabViewModel(workingDir);

            tabs.Add(initialTab);
        }

        // Public interface methods

        public void NewTab(string folderPath) => throw new NotImplementedException();
        public void CloseTab(int index) => throw new NotImplementedException();
    }
}
