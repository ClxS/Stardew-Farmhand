using System;
using System.Reflection;

namespace RevolutionDebugger
{
    public class StardewRevolutionLauncher
    {
        private Assembly _revolutionAssembly;
        public Assembly RevolutionAssembly => _revolutionAssembly ?? (_revolutionAssembly = Assembly.LoadFrom(Constants.RevolutionExeName));

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
