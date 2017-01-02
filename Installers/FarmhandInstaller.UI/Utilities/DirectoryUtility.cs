namespace Farmhand.Installers.Utilities
{
    using System.Collections.Generic;
    using System.IO;
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

        public static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static void CleanDirectory(string directory)
        {
            var di = new DirectoryInfo(directory);

            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public static void CopyAll(string sourcePath, string targetPath, string exceptFilter = null)
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
                    if (exceptFilter != null && Regex.IsMatch(fi.FullName, exceptFilter))
                    {
                        continue;
                    }

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
