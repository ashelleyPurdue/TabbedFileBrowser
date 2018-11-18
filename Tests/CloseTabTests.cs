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
        private void AssertSelectedIndexChangeOnTabClose
            (int numTabs, int startTab, int tabToClose, int expectedSelectedTab)
        {
            // Start by opening all the tabs
            while (Browser.Tabs.Count < numTabs)
                Browser.NewTab(".");

            Browser.SelectedTabIndex = startTab;
            Browser.CloseTab(tabToClose);

            Assert.AreEqual(expectedSelectedTab, Browser.SelectedTabIndex);
        }

        [TestMethod]
        public void DontCloseOnlyTab() => AssertSelectedIndexChangeOnTabClose
        (
            numTabs: 1,
            startTab: 0,
            tabToClose: 0,
            expectedSelectedTab: 0
        );

        [TestMethod]
        public void CloseTabAfterCurrent() => AssertSelectedIndexChangeOnTabClose
        (
            numTabs: 3,
            startTab: 1,
            tabToClose: 2,
            expectedSelectedTab: 1
        );

        [TestMethod]
        public void CloseTabBeforeCurrent() => AssertSelectedIndexChangeOnTabClose
        (
            numTabs: 3,
            startTab: 1,
            tabToClose: 0,
            expectedSelectedTab: 0
        );

        [TestMethod]
        public void CloseTabBeforeCurrentLast() => AssertSelectedIndexChangeOnTabClose
        (
            numTabs: 3,
            startTab: 2,
            tabToClose: 0,
            expectedSelectedTab: 1
        );

        [TestMethod]
        public void CloseCurrentTabFirst() => AssertSelectedIndexChangeOnTabClose
        (
            numTabs: 3,
            startTab: 0,
            tabToClose: 0,
            expectedSelectedTab: 0
        );

        [TestMethod]
        public void CloseCurrentTabMiddle() => AssertSelectedIndexChangeOnTabClose
        (
            numTabs: 3,
            startTab: 1,
            tabToClose: 1,
            expectedSelectedTab: 1
        );

        [TestMethod]
        public void CloseCurrentTabLast() => AssertSelectedIndexChangeOnTabClose
        (
            numTabs: 4,
            startTab: 3,
            tabToClose: 3,
            expectedSelectedTab: 2
        );
    }
}
