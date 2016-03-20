using System;

namespace RevolutionDebugger
{
    class Program
    {
        static int Main(string[] args)
        {
            StardewRevolutionLauncher launcher = new StardewRevolutionLauncher();
            try
            {
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
