using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TabbedFileBrowser
{
    /// <summary>
    /// Fuck you, Microsoft.
    /// </summary>
    public static class FileSystemInfoExtensions
    {
        public static void Move(this FileSystemInfo file, string dest)
        {
            switch (file)
            {
                case FileInfo f:        f.MoveTo(dest); break;
                case DirectoryInfo f:   f.MoveTo(dest); break;  // Am I seeing double?
            }
        }

        public static void Copy(this FileSystemInfo file, string dest, bool overwrite = false)
        {
            switch (file)
            {
                case FileInfo f:        f.CopyTo(dest, overwrite); break;
                case DirectoryInfo f:   DirectoryCopy(f.FullName, dest); break;
            }
        }

        public static string ParentFolderPath(this FileSystemInfo file)
        {
            switch (file)
            {
                case FileInfo f:        return f.DirectoryName;
                case DirectoryInfo f:   return f.Parent.FullName;

                default: throw new Exception("REALLY?!  Why would you add another FileSystemInfo?!");   
            }
        }


        // Helper methods

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Copied from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
            // Though I shouldn't *have* to copy it.  This shit should be built in!
            // Fuck you, Microsoft.

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // Copy the subdirectories
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }
    }
}
