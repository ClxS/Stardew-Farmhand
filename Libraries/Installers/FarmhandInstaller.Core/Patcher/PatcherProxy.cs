namespace FarmhandInstaller.Core.Patcher
{
    using System;
    using System.IO;
    using System.Reflection;

    internal class PatcherProxy : MarshalByRefObject
    {
        private dynamic Patcher { get; set; }

        public void Initialize(string assemblyPath, string type)
        {
            var test = Assembly.UnsafeLoadFrom(assemblyPath);
            var patcherType = test.GetType(type);
            this.Patcher = Activator.CreateInstance(patcherType);
        }

        public void LoadCommon(string assemblyDirectory)
        {
            Assembly.UnsafeLoadFrom(Path.Combine(assemblyDirectory, "ILRepack.dll"));
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
}