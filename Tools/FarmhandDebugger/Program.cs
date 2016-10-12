using System;

namespace FarmhandDebugger
{
    class Program
    {
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }

        static int Main()
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

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
