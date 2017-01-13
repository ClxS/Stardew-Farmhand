namespace FarmhandDebugger
{
    using System;
    using System.Reflection;

    internal class StardewFarmhandLauncher
    {
        private Assembly farmhandAssembly;

        public Assembly FarmhandAssembly
            => this.farmhandAssembly ?? (this.farmhandAssembly = Assembly.LoadFrom(Constants.FarmhandExeName));

        public bool Launch()
        {
            if (this.FarmhandAssembly == null)
            {
                return false;
            }

            Console.WriteLine("Starting Stardew Valley...");
            try
            {
                this.FarmhandAssembly.EntryPoint.Invoke(null, new object[] { new string[0] });
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}