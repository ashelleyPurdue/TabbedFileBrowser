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

            string[] folders = new[]
            {
                "folders",
                "folders\\just_foo",
                "folders\\just_bar"
            };

            // Browse through some folders
            foreach (string f in folders)
            {
                tab.NavigateTo(f);
                AssertForwardBackButtons(true, false);
            }
        }

        [TestMethod]
        public void MoveBackForwardAlwaysEnabled()
        {
            var tab = Browser.CurrentTab;

            string[] folders = new[]
            {
                "folders",
                "folders\\just_foo",
                "folders\\just_bar"
            };

            // Browse through some folders
            foreach (string f in folders)
                tab.NavigateTo(f);

            // Spam the back button
            for (int i = folders.Length - 1; i >= 0; i--)
            {
                tab.MoveBack();

                // The forward button should always be enabled.
                // The back button should always be enabled, except for when
                // we've reached the start of the history.
                AssertForwardBackButtons(i != 0, true);
            }
        }
    }
}
