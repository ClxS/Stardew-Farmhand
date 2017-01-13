namespace FarmhandDebugger
{
    using System;

    internal class Program
    {
        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }

        private static int Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            var launcher = new StardewFarmhandLauncher();
            try
            {
                if (!launcher.Launch())
                {
                    throw new Exception("Could not launch Stardew Farmhand");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FATAL EXCEPTION: " + ex.InnerException?.Message);

                return -1;
            }

            return 0;
        }
    }
}