using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Revolution
{
    public class Constants
    {
        public static string StardewExe => "Stardew Valley.exe";
        public static string RevolutionExe => "Stardew Revolution.exe";
        public static string RevolutionDll => "Revolution.dll";
        public static string IntermediateRevolutionExe => "Stardew Revolution.int";
        public static string CurrentAssemblyPath => Assembly.GetExecutingAssembly().Location;
    }
}
