namespace FarmhandInstaller.UI.Utilities.Package
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class DeveloperFullPackage : IPackage
    {
        public const string PackageFile = "FarmhandInstaller.UI.Payload.DeveloperFull.package";

        private const string ProjectBlockMatch = @"(?=Project\()(.|\n)+?(?=EndProject\s)EndProject\s*\n";

        private const string ModsFolderProjectMatch = ".*\"Mods\"(.|\\n)*";

        private const string ProjectIdMatch = "(?<=, \"{)(.*?)(?=})";

        #region IPackage Members

        public void Install()
        {
            DirectoryUtility.EnsureDirectoryExists(InstallationContext.OutputPath);
            DirectoryUtility.CleanDirectory(InstallationContext.OutputPath);
            PackageManager.ExtractPackageFile(PackageFile, InstallationContext.OutputPath);

            var output = Path.Combine(InstallationContext.OutputPath, "Staging");
            DirectoryUtility.CopyAll(InstallationContext.StardewPath, output);

            this.EditSolution();
        }

        #endregion
        
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
    }
}