namespace InstallerPackager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    internal class Program
    {
        private const bool UseParallelCopy = true;

        private static Settings settings;

        internal static void Main(string[] args)
        {
            var root = args[0];
            var payloadPath = args[1];

            if (!Path.IsPathRooted(payloadPath))
            {
                payloadPath = Path.Combine(root, args[1]);
            }

            root = SanitizePath(root);
            payloadPath = SanitizePath(payloadPath);

            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (location == null)
            {
                throw new Exception("Assembly.GetExecutingAssembly().Location returned null!");
            }

            var json = File.ReadAllText(Path.Combine(location, "settings.json"));
            settings = JsonConvert.DeserializeObject<Settings>(json);

            foreach (var package in settings.Packages)
            {
                PreparePackage(root, payloadPath, package);
            }
        }

        private static string SanitizePath(string root)
        {
            while (root.EndsWith("\\"))
            {
                root = root.Remove(root.Length - 1, 1);
            }

            return root;
        }

        private static void PreparePackage(string root, string destinationRoot, Package package)
        {
            var tempDir = GetTemporaryDirectory();
            var destination = Path.Combine(destinationRoot, package.Name + ".package");
            if (destination == null)
            {
                throw new Exception("Invalid Destination Path");
            }

            var source = new DirectoryInfo(root);
            CopyAll(source, tempDir, package.InclusionFilters.ToArray(), package.ExclusionFilters.ToArray());
            
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }

            ZipFile.CreateFromDirectory(tempDir, destination, CompressionLevel.Optimal, false);
            
            DeleteTempDirectory(tempDir);
        }

        private static void DeleteTempDirectory(string tempDir)
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }

        private static void CopyAll(DirectoryInfo root, string targetRoot, string[] inclusions, string[] exclusions)
        {
            if (UseParallelCopy)
            {
                CopyAllParallel(root, targetRoot, inclusions, exclusions);
                return;
            }

            var searchDirectories = new Stack<DirectoryInfo>();
            searchDirectories.Push(root);

            do
            {
                var source = searchDirectories.Pop();
                var target = source == root
                                         ? targetRoot
                                         : targetRoot + source.FullName.Remove(0, root.FullName.Length);

                // Copy each file into the new directory.
                foreach (var fi in source.GetFiles().Where(f => inclusions.Any(i => Regex.IsMatch(f.FullName, i))))
                {
                    if (exclusions.Any(e => Regex.IsMatch(fi.FullName, e)))
                    {
                        continue;
                    }

                    if (!Directory.Exists(target))
                    {
                        Directory.CreateDirectory(target);
                    }

                    fi.CopyTo(Path.Combine(target, fi.Name), true);
                }

                // Copy each subdirectory using recursion.
                foreach (var sourceSubDir in source.GetDirectories())
                {
                    searchDirectories.Push(sourceSubDir);
                }
            }
            while (searchDirectories.Count > 0);
        }

        private static void CopyAllParallel(DirectoryInfo root, string targetRoot, string[] inclusions, string[] exclusions)
        {
            var fileDirectories = new List<DirectoryInfo>();
            var searchDirectories = new Stack<DirectoryInfo>();
            searchDirectories.Push(root);

            do
            {
                var source = searchDirectories.Pop();
                fileDirectories.Add(source);

                // Copy each subdirectory using recursion.
                foreach (var sourceSubDir in source.GetDirectories())
                {
                    searchDirectories.Push(sourceSubDir);
                }
            }
            while (searchDirectories.Count > 0);
            
            Parallel.ForEach(
                fileDirectories,
                source =>
                {
                    var target = source == root
                                        ? targetRoot
                                        : targetRoot + source.FullName.Remove(0, root.FullName.Length);

                    // Copy each file into the new directory.
                    foreach (
                        var fi in source.GetFiles().Where(f => inclusions.Any(i => Regex.IsMatch(f.FullName, i))))
                    {
                        if (exclusions.Any(e => Regex.IsMatch(fi.FullName, e)))
                        {
                            continue;
                        }

                        if (!Directory.Exists(target))
                        {
                            Directory.CreateDirectory(target);
                        }

                        fi.CopyTo(Path.Combine(target, fi.Name), true);
                    }
                });
        }

        public static string GetTemporaryDirectory()
        {
            string tempDirectory;
            do
            {
                tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                tempDirectory = tempDirectory.Replace('.', '_');
            }
            while (Directory.Exists(tempDirectory));

            tempDirectory += "\\FH";

            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
