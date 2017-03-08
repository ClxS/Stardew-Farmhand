namespace Farmhand.Installers.Utilities.Package
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Farmhand.Installers.Utilities;

    internal class DeveloperLitePackage : IPackage
    {
        public const string PackageFile = "Farmhand.Installers.Payload.DeveloperLite.package";

        private const string ProjectBlockMatch = @"(?=Project\()(.|\n)+?(?=EndProject\s)EndProject\s*\n";
        
        private const string ProjectIdMatch = "(?<=, \"{)(.*?)(?=})";

        #region IPackage Members

        public void Install(PackageStatusContext context)
        {
            var workingDir = Path.Combine(InstallationContext.OutputPath, "WorkingDirectory");
            var farmhandBinary = Path.Combine(workingDir, "Stardew Farmhand.exe");
            var assemblyDirectory = Path.Combine(InstallationContext.OutputPath, "Bin");

            context.SetState(10, "Cleaning Output Directory");
            DirectoryUtility.EnsureDirectoryExists(InstallationContext.OutputPath);
            DirectoryUtility.CleanDirectory(InstallationContext.OutputPath);
            PackageManager.ExtractPackageFile(PackageFile, InstallationContext.OutputPath);

            this.EditSolution(context);

            context.SetState(50, "Copying SMAPI Files");
            DirectoryUtility.CopyAll(InstallationContext.StardewPath, workingDir, ".*\\.exe");

            StardewPatcher.Patch(farmhandBinary, assemblyDirectory, true, context);
        }

        #endregion

        private void EditSolution(PackageStatusContext context)
        {
            var solution = Path.Combine(InstallationContext.OutputPath, "Farmhand.sln");
            var text = File.ReadAllText(solution);
            var projects = Regex.Matches(text, ProjectBlockMatch).Cast<Match>().ToList();

            context.SetState(20, "Removing Projects From Solution");

            Match modTemplateProject = null;
            var linesToCull = new List<string>();
            foreach (var project in projects)
            {
                if (!project.Value.Contains("\"Mods\"") && !project.Value.Contains("FarmhandDebugger"))
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
                context.SetState(30, "Adding new blank mod");

                if (modTemplateProject == null)
                {
                    throw new Exception(
                        "ModTemplate was not included in the solution! Development mod will not be generated");
                }

                var modTemplateLine = modTemplateProject.Value;
                modTemplateLine = modTemplateLine.Substring(0, modTemplateLine.IndexOf("ProjectSection", StringComparison.Ordinal));

                var nextModEntry = modTemplateLine.Replace(
                    "ModTemplate",
                    InstallationContext.ModSettings.NameNoSpace);
                text = text.Replace(modTemplateLine, nextModEntry);

                ModTemplateUtility.PatchTemplateMod();
            }

            context.SetState(40, "Writing Modified Solution");
            File.WriteAllText(solution, text);
        }
    }
}