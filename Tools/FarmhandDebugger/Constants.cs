namespace FarmhandDebugger
{
    internal static class Constants
    {
        public static string FarmhandExeName
            => $"../../../WorkingDirectory/{PlatformFolder}/{ConfigurationFolder}/Stardew Farmhand.exe";

#if DEBUG
        public static string ConfigurationFolder => "Debug";
#else
        public static string ConfigurationFolder => "Release";
#endif

#if WINDOWS
        public static string PlatformFolder => "Windows";
#elif LINUX
        public static string PlatformFolder => "Linux";
#elif MACOSX
        public static string PlatformFolder => "MacOSX";
#endif
    }
}