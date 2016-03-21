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

        public static string RevolutionExe => "Z:\\Projects\\C#\\Revolution\\WorkingDirectory\\Stardew Revolution.exe";
        
        public static string RevolutionDll => "Revolution.dll";
        public static string RevolutionUIDll => "RevolutionUI.dll";
        public static string IntermediateRevolutionExe => "Stardew Revolution.int1.dll";
        public static string PassOneRevolutionExe => "Stardew Revolution.int2.dll";
        public static string JsonLibrary => "Newtonsoft.Json.dll";
        public static string CurrentAssemblyPath => Assembly.GetExecutingAssembly().Location;
    }
}
