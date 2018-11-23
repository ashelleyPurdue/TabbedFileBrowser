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
        [TestClass]
        public class StartsWithFilterTests : FilterTestsBase
        {
            protected override FilterCondition ParseFilterString(string filterString) =>
                f => f.Name.StartsWith(filterString);

            [TestMethod]
            public void StartsWithFi() => AssertFiltered
            (
                folder: "fizz_buzz",
                filter: "fi",
                expectedFiles: new[] { "fizz.txt" }
            );
        }
    }

    public abstract class FilterTestsBase
    {
        const string BASE_PATH = "../../TestFilesTemplate/folders";

        protected abstract FilterCondition ParseFilterString(string filterString);

        protected void AssertFiltered(string folder, string filter, params string[] expectedFiles)
        {
            folder = Path.Combine(BASE_PATH, folder);

            ITabbedFileBrowserViewModel browser = new TabbedFileBrowserControl().ViewModel;

            browser.ParseFilterString = this.ParseFilterString;
            browser.CurrentTab.FilterString = filter;
            browser.CurrentTab.NavigateTo(folder);

            var actualFiles = browser.CurrentTab.VisibleFiles
                                                .Select(f => f.Name);

            // Sort them alphabetically before comparing them
            var actualSorted   = actualFiles.OrderBy(f => f);
            var expectedSorted = expectedFiles.OrderBy(f => f);

            Assert.IsTrue(actualSorted.SequenceEqual(expectedSorted));
        }
    }
}
