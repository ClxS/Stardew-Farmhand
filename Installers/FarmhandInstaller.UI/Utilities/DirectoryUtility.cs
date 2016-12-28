namespace FarmhandInstaller.UI.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal static class DirectoryUtility
    {
        public static string CreateTemporaryDirectory()
        {
            string tempDirectory;
            do
            {
                tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                tempDirectory = tempDirectory.Replace('.', '_');
            }
            while (Directory.Exists(tempDirectory));

            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public static void CopyAll(string sourcePath, string targetPath)
        {
            var root = new DirectoryInfo(sourcePath);
            var searchDirectories = new Stack<DirectoryInfo>();
            searchDirectories.Push(root);

            do
            {
                var source = searchDirectories.Pop();
                var target = source == root ? targetPath : Path.Combine(targetPath, source.FullName.Remove(0, root.FullName.Length + 1));

                // Copy each file into the new directory.
                foreach (var fi in source.GetFiles())
                {
                    if (!Directory.Exists(target))
                    {
                        Directory.CreateDirectory(target);
                    }

                    fi.CopyTo(Path.Combine(target, fi.Name), true);
                }
                
                foreach (var sourceSubDir in source.GetDirectories())
                {
                    searchDirectories.Push(sourceSubDir);
                }
            }
            while (searchDirectories.Count > 0);
        }
    }
}
