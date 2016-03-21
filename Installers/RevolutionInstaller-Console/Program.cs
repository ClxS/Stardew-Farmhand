using Revolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevolutionInstaller_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Patcher patcher;
            if(args.Any() && args[0] == "-pass1")
            {
                patcher = new PatcherFirstPass();
                patcher.PatchStardew(Constants.StardewExe, Constants.RevolutionDll);
            }
            else if(args.Any() && args[0] == "-pass2")
            {
                patcher = new PatcherSecondPass();
                patcher.PatchStardew(Constants.PassOneRevolutionExe, Constants.RevolutionUIDll);
            }
            else
            {
                Console.WriteLine("Invalid build pass");
            }
        }
    }
}
