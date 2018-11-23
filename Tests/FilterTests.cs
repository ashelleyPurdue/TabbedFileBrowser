using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TabbedFileBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class FilterTests
    {
        const string BASE_PATH = "../../TestFilesTemplate/folders";
        private void AssertFiltered(string folder, string filter, string[] expectedFiles, FilterStringParser parser = null)
        {
            folder = Path.Combine(BASE_PATH, folder);

            ITabbedFileBrowserViewModel browser = new TabbedFileBrowserControl().ViewModel;

            // If no parser is specified, use the default parser.
            if (parser != null)
                browser.ParseFilterString = parser;

            browser.CurrentTab.FilterString = filter;
            browser.CurrentTab.NavigateTo(folder);

            var actualFiles = browser.CurrentTab.VisibleFiles
                                                .Select(f => f.Name);

            // Sort them alphabetically before comparing them
            var actualSorted = actualFiles.OrderBy(f => f);
            var expectedSorted = expectedFiles.OrderBy(f => f);

            Assert.IsTrue(actualSorted.SequenceEqual(expectedSorted));
        }

        private FilterCondition StartsWith(string s)
        {
            return f => f.Name.StartsWith(s);
        }


        [TestMethod]
        public void EmptyStringDisablesFiltering() => AssertFiltered
        (
            folder: "fizz_buzz",
            parser: s => (f => false),      // Generates a FilterCondition that never matches.  Should still show all files.
            filter: "",
            expectedFiles: new[] { "buzz.txt", "fizz.txt" }
        );

        [TestMethod]
        public void ContainsIzz() => AssertFiltered
        (
            folder: "fizz_buzz",
            filter: "izz",
            expectedFiles: new[] { "fizz.txt" }
        );

        [TestMethod]
        public void ContainsUzz() => AssertFiltered
        (
            folder: "fizz_buzz",
            filter: "uzz",
            expectedFiles: new[] { "buzz.txt" }
        );

        [TestMethod]
        public void StartsWithFi() => AssertFiltered
        (
            folder: "fizz_buzz",
            parser: StartsWith,
            filter: "fi",
            expectedFiles: new[] { "fizz.txt" }
        );

        [TestMethod]
        public void StartsWithBu() => AssertFiltered
        (
            folder: "fizz_buzz",
            parser: StartsWith,
            filter: "bu",
            expectedFiles: new[] { "buzz.txt" }
        );
    }
}
