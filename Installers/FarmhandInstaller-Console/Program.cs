namespace Farmhand.Installers
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Installers.Patcher;

    internal class Program
    {
        public enum Pass
        {
            /// <summary>
            /// First Pass
            /// </summary>
            PassOne,

            /// <summary>
            /// Second Pass
            /// </summary>
            PassTwo
        }

        internal static void Main(string[] args)
        {
            Patcher.Patcher patcher;
            bool grmDisabled = args.Any(a => a.Equals("-disablegrm"));

            if (args.Any(a => a.Equals("-pass1")))
            {
                patcher = CreatePatcher(Pass.PassOne);
                PatcherOptions.DisableGrm = grmDisabled;
                patcher.PatchStardew();
            }
            else if (args.Any(a => a.Equals("-pass2")))
            {
                patcher = CreatePatcher(Pass.PassTwo);
                PatcherOptions.DisableGrm = grmDisabled;
                patcher.PatchStardew();
            }
            else
            {
                Console.WriteLine("Invalid build pass");
            }            
        }

        internal static Patcher.Patcher CreatePatcher(Pass pass)
        {
            Assembly patcherAssembly = Assembly.LoadFrom(pass == Pass.PassOne ? "FarmhandPatcherFirstPass.dll" : "FarmhandPatcherSecondPass.dll");
            Type patcherType = patcherAssembly.GetType(pass == Pass.PassOne ? "Farmhand.Installers.Patcher.PatcherFirstPass" : "Farmhand.Installers.Patcher.PatcherSecondPass");
            object patcher = Activator.CreateInstance(patcherType);
            return patcher as Patcher.Patcher;
        }
    }
}
