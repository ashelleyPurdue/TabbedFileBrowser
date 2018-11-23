using System;
using System.Windows;
using System.Linq;
using System.IO;
using TabbedFileBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWindow;

namespace Tests
{
    public partial class TabTests
    {
        [TestMethod]
        public void SelectedIndexAndSelectedFileMatch()
        {
            Browser.CurrentTab.NavigateTo("folders/fizz_buzz");
            AssertVisibleFiles("buzz.txt", "fizz.txt");

            Browser.SelectedFileIndex = 1;
            Assert.AreEqual(Browser.SelectedFile.Name, "fizz.txt");
        }
    }
}
