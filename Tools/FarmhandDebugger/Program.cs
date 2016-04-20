using System;

namespace FarmhandDebugger
{
    class Program
    {
        static int Main()
        {
            StardewFarmhandLauncher launcher = new StardewFarmhandLauncher();
            try
            {
                if (!launcher.Launch())
                {
                    throw new Exception("Could not launch Stardew Farmhand");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("FATAL EXCEPTION: " + ex.InnerException.Message);

                return -1;
            }
            return 0;
        }
    }
}
