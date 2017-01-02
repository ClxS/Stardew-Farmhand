namespace Farmhand.Installers.Utilities.Package
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Farmhand.Installers.Utilities;

    internal class DeveloperFullPackage : IPackage
    {
        public const string PackageFile = "Farmhand.Installers.Payload.DeveloperFull.package";

        private const string ProjectBlockMatch = @"(?=Project\()(.|\n)+?(?=EndProject\s)EndProject\s*\n";

        private const string ProjectIdMatch = "(?<=, \"{)(.*?)(?=})";

        #region IPackage Members

        public void Install(PackageStatusContext context)
        {
            context.SetState(10, "Cleaning Output Directory");
            DirectoryUtility.EnsureDirectoryExists(InstallationContext.OutputPath);
            DirectoryUtility.CleanDirectory(InstallationContext.OutputPath);

            context.SetState(25, "Extracting Package File");
            PackageManager.ExtractPackageFile(PackageFile, InstallationContext.OutputPath);

            context.SetState(40, "Copying Stardew Valley Files");
            var output = Path.Combine(InstallationContext.OutputPath, "Staging");
            DirectoryUtility.CopyAll(InstallationContext.StardewPath, output);

            this.EditSolution(context);
        }

        #endregion

        private void EditSolution(PackageStatusContext context)
        {
            var solution = Path.Combine(InstallationContext.OutputPath, "Farmhand.sln");
            var text = File.ReadAllText(solution);
            var projects = Regex.Matches(text, ProjectBlockMatch).Cast<Match>().ToList();

            context.SetState(50, "Removing Mod Projects From Solution");

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
                context.SetState(80, "Adding new blank mod");

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

            context.SetState(100, "Writing Modified Solution");
            File.WriteAllText(solution, text);
        }
    }
}