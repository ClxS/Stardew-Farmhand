using Farmhand;
using System;
using System.Linq;
using System.Reflection;

namespace FarmhandInstaller_Console
{
    class Program
    {
        public enum Pass
        {
            PassOne,
            PassTwo
        }
        
        static void Main(string[] args)
        {
            Patcher patcher;
            bool grmDisabled = args.Any(a => a.Equals("-disablegrm"));

            if (args.Any(a => a.Equals("-pass1")))
            {
                patcher = CreatePatcher(Pass.PassOne);
                patcher.Options.DisableGrm = grmDisabled;
                patcher.PatchStardew();
            }
            else if (args.Any(a => a.Equals("-pass2")))
            {
                patcher = CreatePatcher(Pass.PassTwo);
                patcher.Options.DisableGrm = grmDisabled;
                patcher.PatchStardew();
            }
            else
            {
                Console.WriteLine("Invalid build pass");
            }

            
        }

        static Patcher CreatePatcher(Pass pass)
        {
            Assembly patcherAssembly = Assembly.LoadFrom(pass == Pass.PassOne ? "FarmhandPatcherFirstPass.dll" : "FarmhandPatcherSecondPass.dll");
            Type patcherType = patcherAssembly.GetType(pass == Pass.PassOne ? "Farmhand.PatcherFirstPass" : "Farmhand.PatcherSecondPass");
            object patcher = Activator.CreateInstance(patcherType);
            return patcher as Patcher;
        }
    }
}
