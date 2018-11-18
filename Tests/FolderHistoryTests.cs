using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabbedFileBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWindow;

namespace Tests
{
    public partial class TabTests
    {
        [TestMethod]
        public void NoForwardOrBackOnTabOpen()
        {
            Assert.IsFalse(Browser.CurrentTab.HasNextFolder);
            Assert.IsFalse(Browser.CurrentTab.HasPrevFolder);
        }
    }
}
