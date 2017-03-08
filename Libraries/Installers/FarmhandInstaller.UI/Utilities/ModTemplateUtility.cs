namespace Farmhand.Installers.Utilities
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    internal static class ModTemplateUtility
    {
        public static void PatchTemplateMod()
        {
            var templateModRoot = Path.Combine(InstallationContext.OutputPath, "Mods", "ModTemplate");
            var newModRoot = Path.Combine(InstallationContext.OutputPath, "Mods", InstallationContext.ModSettings.NameNoSpace);

            Directory.Move(templateModRoot, newModRoot);

            var filesToPatch = new[] { "manifest.json", "Mod.cs", "Settings.cs", "ModTemplate.csproj", "Properties\\AssemblyInfo.cs" };
            
            foreach (var file in filesToPatch)
            {
                var filePath = Path.Combine(newModRoot, file);
                if (!File.Exists(filePath))
                {
                    throw new Exception("Missing file from ModTemplate: " + file);
                }

                var fileContents = File.ReadAllText(filePath);
                fileContents = fileContents.Replace("ModTemplateWS", InstallationContext.ModSettings.Name);
                fileContents = fileContents.Replace("ModTemplate", InstallationContext.ModSettings.NameNoSpace);
                fileContents = fileContents.Replace("ModAuthor", InstallationContext.ModSettings.Author);
                fileContents = fileContents.Replace("ModDescription", InstallationContext.ModSettings.Description);
                File.WriteAllText(filePath, fileContents);
            }

            if (!InstallationContext.ModSettings.UsesSettingsFile)
            {
                File.Delete(Path.Combine(newModRoot, "Settings.cs"));
                var projFile = Path.Combine(newModRoot, "ModTemplate.csproj");
                var text = File.ReadAllText(projFile);
                text = Regex.Replace(text, ".*Settings\\.cs.*", string.Empty);
                File.WriteAllText(projFile, text);

                var mainFile = Path.Combine(newModRoot, "Mod.cs");
                text = File.ReadAllText(mainFile);
                text = Regex.Replace(text, @".*#region(.|\n)*#endregion\s\n\s\s", string.Empty);
                File.WriteAllText(mainFile, text);
            }
            
            File.Move(
                Path.Combine(newModRoot, "ModTemplate.csproj"),
                Path.Combine(newModRoot, InstallationContext.ModSettings.NameNoSpace + ".csproj"));
        }
    }
}
