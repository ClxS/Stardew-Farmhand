using System.IO;
using System.Reflection;

namespace Farmhand
{
    public static class PatcherConstants
    {
        public static string StardewExe => "Stardew Valley.exe";

        public static string FarmhandExe => $@"{CurrentAssemblyDirectory}..\..\WorkingDirectory\Stardew Farmhand.exe";
        
        public static string FarmhandDll => "Farmhand.dll";
        public static string FarmhandUiDll => "FarmhandUI.dll";
        public static string SmapiDll => "StardewModdingAPI.dll";

        public static string PassOnePackageResult => "Stardew Farmhand.int1-package.dll";
        public static string PassTwoPackageResult => "Stardew Farmhand.dll";
        
        public static string PassOneFarmhandExe => "Stardew Farmhand.int1.dll";
        public static string PassTwoFarmhandExe => FarmhandExe;

        public static string JsonLibrary => "Newtonsoft.Json.dll";
        public static string CurrentAssemblyPath => Assembly.GetExecutingAssembly().Location;
        public static string CurrentAssemblyDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
