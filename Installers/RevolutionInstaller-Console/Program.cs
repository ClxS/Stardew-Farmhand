using Revolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RevolutionInstaller_Console
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
            if(args.Any() && args[0] == "-pass1")
            {
                patcher = CreatePatcher(Pass.PassOne);
                patcher.PatchStardew(Constants.StardewExe, Constants.RevolutionDll);
            }
            else if(args.Any() && args[0] == "-pass2")
            {
                patcher = CreatePatcher(Pass.PassTwo);
                patcher.PatchStardew(Constants.PassOneRevolutionExe, Constants.RevolutionUIDll);
            }
            else
            {
                Console.WriteLine("Invalid build pass");
            }
        }

        static Patcher CreatePatcher(Pass pass)
        {
            Assembly patcherAssembly = Assembly.LoadFrom(pass == Pass.PassOne ? "RevolutionPatcherFirstPass.dll" : "RevolutionPatcherSecondPass.dll");
            Type patcherType = patcherAssembly.GetType(pass == Pass.PassOne ? "Revolution.PatcherFirstPass" : "Revolution.PatcherSecondPass");
            object patcher = Activator.CreateInstance(patcherType);
            return patcher as Patcher;
        }
    }
}
