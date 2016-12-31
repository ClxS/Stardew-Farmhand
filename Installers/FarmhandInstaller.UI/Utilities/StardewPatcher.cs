namespace FarmhandInstaller.UI.Utilities
{
    using System;
    using System.IO;
    using System.Reflection;

    using Farmhand;

    internal static class StardewPatcher
    {
        private static Patcher CreatePatcher(StardewPatcherPass pass, string assemblyLocation)
        {
            var patcherAssembly =
                Assembly.LoadFrom(
                    pass == StardewPatcherPass.PassOne
                        ? Path.Combine(assemblyLocation, "FarmhandPatcherFirstPass.dll")
                        : Path.Combine(assemblyLocation, "FarmhandPatcherSecondPass.dll"));
            var patcherType = patcherAssembly.GetType(
                    pass == StardewPatcherPass.PassOne 
                        ? "Farmhand.PatcherFirstPass" 
                        : "Farmhand.PatcherSecondPass");
            var patcher = Activator.CreateInstance(patcherType);
            return patcher as Patcher;
        }

        public static void Patch(string outputPath, string assemblyDirectory, bool disableGrm = true)
        {
            var sdvPath = Path.Combine(InstallationContext.StardewPath, "Stardew Valley.exe");
            
            var patcher = CreatePatcher(StardewPatcherPass.PassOne, assemblyDirectory);
            patcher.Options.AssemblyDirectory = assemblyDirectory;
            patcher.Options.DisableGrm = disableGrm;
            patcher.PatchStardew(sdvPath);

            patcher = CreatePatcher(StardewPatcherPass.PassTwo, assemblyDirectory);
            patcher.Options.AssemblyDirectory = assemblyDirectory;
            patcher.Options.DisableGrm = disableGrm;
            patcher.PatchStardew(outputPath);
        }
    }
}
