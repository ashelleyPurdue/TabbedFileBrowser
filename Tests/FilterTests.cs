using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabbedFileBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class FilterTests
    {
    }

    public abstract class FilterTestsBase
    {
        protected abstract FilterCondition ParseFilterString(string filterString);

        protected void AssertFiltered(string folder, string filter, params string[] expectedFiles)
        {
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
