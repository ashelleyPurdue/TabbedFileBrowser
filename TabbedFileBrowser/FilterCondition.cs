using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TabbedFileBrowser
{
    /// <summary>
    /// Returns true if file matches some condition.
    /// Otherwise, returns false.
    /// </summary>
    public delegate bool FilterCondition(FileSystemInfo file);

    /// <summary>
    /// Parses the given filterString and creates a
    /// FilterCondition based on it.
    /// </summary>
    public delegate FilterCondition FilterStringParser(string filterString);
}
