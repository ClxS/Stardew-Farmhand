using System;

namespace RevolutionDebugger
{
    class Program
    {
        static int Main()
        {
            StardewRevolutionLauncher launcher = new StardewRevolutionLauncher();
            try
            {
                if (!launcher.Launch())
                {
                    throw new Exception("Could not launch Stardew Revolution");
                }
            }
            catch(Exception)
            {
                return -1;
            }
            return 0;
        }
    }
}
