using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabbedFileBrowser
{
    public class TabbedFileBrowserViewModel : ITabbedFileBrowserViewModel
    {
        public IReadOnlyList<ITabViewModel> Tabs => throw new NotImplementedException();

        public int SelectedTabIndex
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public ITabViewModel CurrentTab => Tabs[SelectedTabIndex];

        public void NewTab(string folderPath) => throw new NotImplementedException();
        public void CloseTab(int index) => throw new NotImplementedException();
    }
}
