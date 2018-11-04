using System;
using System.Windows;
using System.Linq;
using System.IO;
using TabbedFileBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TabTests
    {
        const string TEMPLATE_PATH = "TestFilesTemplate";
        const string TEST_DIR_PATH = "TestFiles";

        private ITabbedFileBrowserViewModel browser;

        private void AssertVisibleFiles(params string[] expected)
        {
            var expectedSorted = expected.OrderBy(s => s);

            var actualSorted = browser.CurrentTab
                                        .VisibleFiles
                                        .Select(f => f.Name)
                                        .OrderBy(s => s);

            Assert.IsTrue(expectedSorted.SequenceEqual(actualSorted));
        }

        [ClassInitialize]
        public static void ResetTestDir(TestContext context)
        {
            // Remove the old directory, in case it's been changed
            if (Directory.Exists(TEST_DIR_PATH))
                Directory.Delete(TEST_DIR_PATH, true);

            // Copy it from the template
            var templateFolder = new DirectoryInfo(TEMPLATE_PATH);
            templateFolder.Copy(TEST_DIR_PATH);

            // cd to the test folder
            Directory.SetCurrentDirectory(TEST_DIR_PATH);
        }

        [TestInitialize]
        public void MakeBrowser()
        {
            browser = new TabbedFileBrowserViewModel();

            // TODO: Put this in a real control
            // TODO: Put said control in a real window
            // TODO: Close said window on test cleanup
        }

        [TestMethod]
        public void TestFolderRestored()
        {
            string currentFolder = Directory.GetCurrentDirectory();
            Assert.AreEqual(TEST_DIR_PATH, Path.GetFileName(currentFolder));
        }

        [TestMethod]
        public void StartsInWorkingDirectory()
        {
            string expected = Directory.GetCurrentDirectory();
            expected = Path.GetFullPath(expected);

            string actual = browser.CurrentTab.CurrentFolder;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void VisibleFilesCorrectOnStartup() => AssertVisibleFiles
        (
            ".gitkeep",
            "folders",
            "foo_file.txt"
        );
    }
}
