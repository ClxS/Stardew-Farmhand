using System.IO;
using System.Reflection;

namespace Revolution
{
    public class PatcherConstants
    {
        public static string StardewExe => "Stardew Valley.exe";

        public static string RevolutionExe => $@"{CurrentAssemblyDirectory}..\..\WorkingDirectory\Stardew Revolution.exe";
        
        public static string RevolutionDll => "Revolution.dll";
        public static string RevolutionUiDll => "RevolutionUI.dll";

        public static string PassOnePackageResult => "Stardew Revolution.int1-package.dll";
        public static string PassTwoPackageResult => "Stardew Revolution.dll";
        
        public static string PassOneRevolutionExe => "Stardew Revolution.int1.dll";
        public static string PassTwoRevolutionExe => RevolutionExe;

        public static string JsonLibrary => "Newtonsoft.Json.dll";
        public static string CurrentAssemblyPath => Assembly.GetExecutingAssembly().Location;
        public static string CurrentAssemblyDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
