namespace Farmhand.Installers.Utilities
{
    using System;
    
    internal static class InstallationContext
    {
        public static string StardewPath { get; set; }

        public static string OutputPath { get; set; }

        public static PackageType PackageType { get; set; }

        public static bool IncludeStardewModdingApi { get; set; }

        public static bool AddNewModFromTemplate { get; set; } = true;

        public static DevelopmentModSettings ModSettings { get; set; } = new DevelopmentModSettings();

        public static Exception Exception { get; set; }
    }
}
