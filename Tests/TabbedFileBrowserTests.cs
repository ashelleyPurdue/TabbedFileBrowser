using System;
using System.Windows;
using System.Linq;
using System.IO;
using TabbedFileBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWindow;

namespace Tests
{
    [TestClass]
    public class TabTests
    {
        const string TEMPLATE_PATH = "../../TestFilesTemplate";
        const string TEST_DIR_PATH = "TestFiles";

        readonly string[] ROOT_FOLDER_CONTENTS = new[]
        {
            ".gitkeep",
            "folders",
            "foo_file.txt"
        };

        private MainWindow window;
        private ITabbedFileBrowserViewModel Browser => window.Browser;

        private void AssertVisibleFiles(params string[] expected)
        {
            var expectedSorted = expected.OrderBy(s => s);

            var actualSorted = Browser.CurrentTab
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
            window = new MainWindow();
            window.Show();
        }

        [TestCleanup]
        public void CloseBrowser()
        {
            window.Close();
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

            string actual = Browser.CurrentTab.CurrentFolder;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void VisibleFilesCorrectOnStartup() => AssertVisibleFiles
        (ROOT_FOLDER_CONTENTS);

        [TestMethod]
        public void VisibleFilesUpdateAfterNavigate()
        {
            string path = Path.Combine("folders", "just_bar");
            Browser.CurrentTab.NavigateTo(path);

            AssertVisibleFiles("bar.txt");
        }

        [TestMethod]
        public void VisibleFilesUpdateAfterSwitchingTabs()
        {
            Browser.NewTab("folders\\just_bar");
            AssertVisibleFiles(ROOT_FOLDER_CONTENTS);

            Browser.SelectedTabIndex = 1;
            AssertVisibleFiles("bar.txt");

            Browser.SelectedTabIndex = 0;
            AssertVisibleFiles(ROOT_FOLDER_CONTENTS);
        }
    }
}
