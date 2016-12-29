namespace FarmhandInstaller.UI.Utilities.Package
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using ICSharpCode.SharpZipLib.Core;
    using ICSharpCode.SharpZipLib.Zip;

    internal class DeveloperFullPackage : IPackage
    {
        public const string PackageFile = "FarmhandInstaller.UI.Payload.DeveloperFull.package";

        private const string ProjectBlockMatch = @"(?=Project\()(.|\n)+?(?=EndProject\s)EndProject\s*\n";

        private const string ModsFolderProjectMatch = ".*\"Mods\"(.|\\n)*";

        private const string ProjectIdMatch = "(?<=, \"{)(.*?)(?=})";

        #region IPackage Members

        public void Install()
        {
            this.EnsureOutputExists();
            this.ClearOutputFolder();
            this.ExtractPackageFile();
            this.CopyStagingFiles();
            this.EditSolution();
        }

        #endregion

        private void EnsureOutputExists()
        {
            if (!Directory.Exists(InstallationContext.OutputPath))
            {
                Directory.CreateDirectory(InstallationContext.OutputPath);
            }
        }

        private void ClearOutputFolder()
        {
            var di = new DirectoryInfo(InstallationContext.OutputPath);

            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private void EditSolution()
        {
            var solution = Path.Combine(InstallationContext.OutputPath, "Farmhand.sln");
            var text = File.ReadAllText(solution);
            var projects = Regex.Matches(text, ProjectBlockMatch).Cast<Match>().ToList();

            Match modTemplateProject = null;
            var linesToCull = new List<string>();
            foreach (var project in projects)
            {
                if (project.Value.Contains("Mods\\"))
                {
                    if (InstallationContext.AddNewModFromTemplate)
                    {
                        if (project.Value.Contains("ModTemplate"))
                        {
                            modTemplateProject = project;
                            continue;
                        }
                    }

                    text = text.Replace(project.Value, string.Empty);
                    linesToCull.Add(Regex.Match(project.Value, ProjectIdMatch).Value);
                }
            }

            foreach (var lineToCull in linesToCull)
            {
                text = Regex.Replace(text, ".*{" + lineToCull + "}.*\n", string.Empty);
            }

            if (InstallationContext.AddNewModFromTemplate)
            {
                if (modTemplateProject == null)
                {
                    throw new Exception(
                        "ModTemplate was not included in the solution! Development mod will not be generated");
                }

                var nextModEntry = modTemplateProject.Value.Replace(
                    "ModTemplate",
                    InstallationContext.ModSettings.NameNoSpace);
                text = text.Replace(modTemplateProject.Value, nextModEntry);

                ModTemplateUtility.PatchTemplateMod();
            }

            File.WriteAllText(solution + "test.sln", text);
        }

        private void CopyStagingFiles()
        {
            var output = Path.Combine(InstallationContext.OutputPath, "Staging");
            DirectoryUtility.CopyAll(InstallationContext.StardewPath, output);
        }

        private void ExtractPackageFile()
        {
            var output = InstallationContext.OutputPath;
            var zipFile = Path.Combine(output, "package.zip");
            EmbeddedResourceUtility.ExtractResource(PackageFile, zipFile);

            ZipFile zf = null;
            try
            {
                var fs = File.OpenRead(zipFile);
                zf = new ZipFile(fs);

                foreach (ZipEntry zipEntry in zf)
                {
                    var entryFileName = zipEntry.Name;
                    var buffer = new byte[4096];
                    var zipStream = zf.GetInputStream(zipEntry);

                    var fullZipToPath = Path.Combine(output, entryFileName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }

                File.Delete(zipFile);
            }
        }
    }
}