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
            Revolution.Patcher.PatchStardew(Constants.StardewExe, Constants.RevolutionDll)
        }
    }
}
