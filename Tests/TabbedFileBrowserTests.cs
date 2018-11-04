using System;
using System.Windows;
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

        [TestMethod]
        public void TestFolderRestored()
        {
            string currentFolder = Directory.GetCurrentDirectory();
            Assert.AreEqual(TEST_DIR_PATH, Path.GetFileName(currentFolder));
        }
    }
}
