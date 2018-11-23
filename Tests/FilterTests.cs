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
        private void AssertFiltered(string folder, FilterStringParser parser, string filter, params string[] expectedFiles)
        {
            folder = Path.Combine(BASE_PATH, folder);

            ITabbedFileBrowserViewModel browser = new TabbedFileBrowserControl().ViewModel;

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

        private FilterCondition Default(string s) => null;

        private FilterCondition StartsWith(string s)
        {
            return f => f.Name.StartsWith(s);
        }


        [TestMethod]
        public void ContainsIzz() => AssertFiltered
        (
            folder: "fizz_buzz",
            parser: Default,
            filter: "izz",
            expectedFiles: new[] { "fizz.txt" }
        );

        [TestMethod]
        public void ContainsUzz() => AssertFiltered
        (
            folder: "fizz_buzz",
            parser: Default,
            filter: "uzz",
            expectedFiles: new[] { "buzz.txt" }
        );

        [TestMethod]
        public void ContainsEmptyStr() => AssertFiltered
        (
            folder: "fizz_buzz",
            parser: Default,
            filter: "",
            expectedFiles: new[] {""}
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

        [TestMethod]
        public void StartsWithEmptyStr() => AssertFiltered
        (
            folder: "fizz_buzz",
            parser: StartsWith,
            filter: "",
            expectedFiles: new[] { "buzz.txt", "fizz.txt" }
        );
    }
}
