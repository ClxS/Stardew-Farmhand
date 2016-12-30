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

        public static void Patch(string outputPath, bool disableGrm = true)
        {
            var sdvPath = Path.Combine(InstallationContext.StardewPath, "Stardew Valley.exe");
            var binPath = Path.Combine(InstallationContext.OutputPath, "Bin");
            
            var patcher = CreatePatcher(StardewPatcherPass.PassOne, binPath);
            patcher.Options.AssemblyDirectory = binPath;
            patcher.Options.DisableGrm = disableGrm;
            patcher.PatchStardew(sdvPath);

            patcher = CreatePatcher(StardewPatcherPass.PassTwo, binPath);
            patcher.Options.AssemblyDirectory = binPath;
            patcher.Options.DisableGrm = disableGrm;
            patcher.PatchStardew(outputPath);
        }
    }
}
