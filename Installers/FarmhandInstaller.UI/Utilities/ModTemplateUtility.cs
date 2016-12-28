namespace FarmhandInstaller.UI.Utilities
{
    using System;
    using System.IO;

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
                fileContents = fileContents.Replace("ModTemplate", InstallationContext.ModSettings.Name);
                fileContents = fileContents.Replace("ModAuthor", InstallationContext.ModSettings.Author);
                fileContents = fileContents.Replace("ModDescription", InstallationContext.ModSettings.Description);
                File.WriteAllText(filePath, fileContents);
            }

            File.Move(
                Path.Combine(newModRoot, "ModTemplate.csproj"),
                Path.Combine(newModRoot, InstallationContext.ModSettings.NameNoSpace + ".csproj"));

        }
    }
}
