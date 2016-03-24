using System;
using System.Reflection;
using Revolution;

namespace RevolutionDebugger
{
    public class StardewRevolutionLauncher
    {
        private Assembly revolutionAssembly = null;
        public Assembly RevolutionAssembly
        {
            get
            {
                if(revolutionAssembly == null)
                {
                    revolutionAssembly = Assembly.LoadFrom(Constants.RevolutionExeName);
                }
                return revolutionAssembly;
            }
        }
        
        public bool Launch()
        {
            if (RevolutionAssembly == null)
                return false;
            
            Console.WriteLine("Starting Stardew Valley...");
            RevolutionAssembly.EntryPoint.Invoke(null, new object[] { new string[0] });
            return true;
        }
    }
}
