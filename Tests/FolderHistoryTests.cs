﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TabbedFileBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWindow;

namespace Tests
{
    public partial class TabTests
    {
        private readonly string[] LONG_HISTORY_PATH = new[]
        {
            "folders",
            "folders\\just_foo",
            "folders\\just_bar",
            "folders\\just_baz",
            "folders\\just_fizz",
            "folders\\just_buzz"
        };

        private void AssertForwardBackButtons(bool back, bool forward)
        {
            Assert.AreEqual(back, Browser.CurrentTab.HasPrevFolder);
            Assert.AreEqual(forward, Browser.CurrentTab.HasNextFolder);
        }

        [TestMethod]
        public void NoForwardOrBackOnTabOpen()
            => AssertForwardBackButtons(false, false);

        [TestMethod]
        public void FollowPathBackButtonAlwaysEnabled()
        {
            var tab = Browser.CurrentTab;

            // Browse through some folders
            foreach (string f in LONG_HISTORY_PATH)
            {
                tab.NavigateTo(f);
                AssertCurrentFolder(f);
                AssertForwardBackButtons(true, false);
            }
        }

        [TestMethod]
        public void MoveBackForwardAlwaysEnabled()
        {
            var tab = Browser.CurrentTab;

            // Browse through some folders
            foreach (string f in LONG_HISTORY_PATH)
                tab.NavigateTo(f);

            // Spam the back button
            for (int i = LONG_HISTORY_PATH.Length - 1; i >= 0; i--)
            {
                AssertCurrentFolder(LONG_HISTORY_PATH[i]);
                tab.MoveBack();

                // The forward button should always be enabled.
                // The back button should always be enabled, except for when
                // we've reached the start of the history.
                AssertForwardBackButtons(i != 0, true);
            }

            AssertCurrentFolder(".");
        }

        [TestMethod]
        public void MoveBackThenNavigateShouldClearFutureHistory()
        {
            var tab = Browser.CurrentTab;

            // Browse through some folders
            foreach (string f in LONG_HISTORY_PATH)
                tab.NavigateTo(f);

            // Move backwards a little
            int numMoveBack = LONG_HISTORY_PATH.Length / 2;

            for (int i = 0; i < numMoveBack; i++)
                tab.MoveBack();
            AssertForwardBackButtons(true, true);

            // Navigate somewhere.  The future history should be disappear.
            tab.NavigateTo("folders\\just_bar");
            AssertForwardBackButtons(true, false);
        }

        [TestMethod]
        public void CanMoveUpAllTheWayToRoot()
        {
            var tab = Browser.CurrentTab;
            string root = Path.GetPathRoot(tab.CurrentFolder);

            while (tab.HasParentFolder)
                tab.MoveUp();

            AssertCurrentFolder(root);
        }
    }
}
