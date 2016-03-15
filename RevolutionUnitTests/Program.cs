using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Revolution
{
    class Program
    {
        static int Main(string[] args)
        {
            StardewRevolutionLauncher launcher = new StardewRevolutionLauncher();
            try
            {
                TestMod testMod = new TestMod();
                testMod.Entry();  


                if (!launcher.Launch())
                {
                    throw new Exception("Could not launch Stardew Revolution");
                }
            }
            catch(Exception e)
            {
                return -1;
            }
            return 0;
        }
    }
}
