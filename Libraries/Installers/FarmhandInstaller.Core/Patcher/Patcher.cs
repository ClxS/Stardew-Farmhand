namespace FarmhandInstaller.Core.Patcher
{
    using System;
    using System.IO;
    using System.Reflection;

    internal static class Patcher
    {
        public static void Patch(string outputPath, string executablePath, string toolsDirectory)
        {
            var localDomain = AppDomain.CreateDomain("LocalDomain");

            var patcher = CreatePatcher(
                Path.Combine(toolsDirectory, "FarmhandPatcherFirstPass.dll"),
                "Farmhand.Installers.Patcher.PatcherFirstPass",
                localDomain);
            patcher.LoadCommon(toolsDirectory);
            patcher.SetOptions(toolsDirectory, true);
            patcher.Patch(executablePath);

            patcher = CreatePatcher(
                Path.Combine(toolsDirectory, "FarmhandPatcherSecondPass.dll"),
                "Farmhand.Installers.Patcher.PatcherSecondPass",
                localDomain);
            patcher.LoadCommon(toolsDirectory);
            patcher.SetOptions(toolsDirectory, true);
            patcher.Patch(outputPath);

            AppDomain.Unload(localDomain);
        }

        private static PatcherProxy CreatePatcher(string assembly, string type, AppDomain domain)
        {
            var proxyType = typeof(PatcherProxy);
            var assemblyFile = Assembly.GetExecutingAssembly().Location;
            if (assemblyFile == null)
            {
                throw new Exception("Could not find currently executing assembly location");
            }

            var proxy = (PatcherProxy)domain.CreateInstanceFromAndUnwrap(assemblyFile, proxyType.FullName);
            proxy.Initialize(assembly, type);
            return proxy;
        }
    }
}