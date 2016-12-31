namespace FarmhandInstaller.UI.Utilities
{
    using System;
    using System.IO;
    using System.Reflection;

    using Farmhand;

    internal static class StardewPatcher
    {
        private class PatcherProxy : MarshalByRefObject
        {
            private Patcher Patcher { get; set; }

            public void Initialize(string assemblyPath, string type)
            {
                var test = Assembly.UnsafeLoadFrom(assemblyPath);
                var patcherType = test.GetType(type);
                this.Patcher = (Patcher)Activator.CreateInstance(patcherType);
            }

            public void LoadCommon(string assemblyDirectory)
            {
                Assembly.UnsafeLoadFrom(Path.Combine(assemblyDirectory, "FarmhandPatcherCommon.dll"));
                Assembly.UnsafeLoadFrom(Path.Combine(assemblyDirectory, "Farmhand.dll"));
            }

            public void SetOptions(string assemblyDirectory, bool disableGrm = true)
            {
                this.Patcher.Options.DisableGrm = disableGrm;
                this.Patcher.Options.AssemblyDirectory = assemblyDirectory;
            }

            public void Patch(string path)
            {
                this.Patcher.PatchStardew(path);
            }
        }

        private static PatcherProxy CreatePatcher(StardewPatcherPass pass, string assemblyLocation, AppDomain domain)
        {
            Type type = typeof(PatcherProxy);
            var proxy = (PatcherProxy)domain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName);
                
            proxy.Initialize(
                pass == StardewPatcherPass.PassOne
                    ? Path.Combine(assemblyLocation, "FarmhandPatcherFirstPass.dll")
                    : Path.Combine(assemblyLocation, "FarmhandPatcherSecondPass.dll"),
                pass == StardewPatcherPass.PassOne ? "Farmhand.PatcherFirstPass" : "Farmhand.PatcherSecondPass");
            return proxy;
        }

        public static void Patch(string outputPath, string assemblyDirectory, bool disableGrm = true)
        {
            var domaininfo = new AppDomainSetup { ApplicationBase = Environment.CurrentDirectory };
            var adevidence = AppDomain.CurrentDomain.Evidence;
            var localDomain = AppDomain.CreateDomain("LocalDomain", adevidence, domaininfo);

            var sdvPath = Path.Combine(InstallationContext.StardewPath, "Stardew Valley.exe");
            
            var patcher = CreatePatcher(StardewPatcherPass.PassOne, assemblyDirectory, localDomain);
            patcher.LoadCommon(assemblyDirectory);
            patcher.SetOptions(assemblyDirectory, disableGrm);
            patcher.Patch(sdvPath);

            patcher = CreatePatcher(StardewPatcherPass.PassTwo, assemblyDirectory, localDomain);
            patcher.LoadCommon(assemblyDirectory);
            patcher.SetOptions(assemblyDirectory, disableGrm);
            patcher.Patch(outputPath);

            AppDomain.Unload(localDomain);
        }
    }
}
