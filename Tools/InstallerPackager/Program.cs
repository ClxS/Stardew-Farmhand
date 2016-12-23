namespace InstallerPackager
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Newtonsoft.Json;

    internal class Program
    {
        private static Settings settings;

        internal static void Main(string[] args)
        {
            var root = args[0];
            var payloadPath = Path.Combine(root, args[1]);

            var json = File.ReadAllText("settings.json");
            settings = JsonConvert.DeserializeObject<Settings>(json);

            var tempDir = GetTemporaryDirectory();
            PrepareDevFull(root, tempDir);
        }

        private static void PrepareDevFull(string root, string tempDir)
        {
            DirectoryInfo source = new DirectoryInfo(root);
            DirectoryInfo target = new DirectoryInfo(tempDir);
            
            CopyAll(source, target, settings.DeveloperFullExclusions.ToArray());
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target, string[] exclusions)
        {
            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                if (!exclusions.Any(e => Regex.IsMatch(fi.FullName, e)))
                {
                    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                }
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo sourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(sourceSubDir.Name);
                CopyAll(sourceSubDir, nextTargetSubDir, exclusions);
            }
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

            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
