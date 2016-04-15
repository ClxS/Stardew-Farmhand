using System;
using System.IO;
using System.Reflection;
using StardewValley;

namespace StardewModdingAPI
{
    /// <summary>
    ///     Static class containing readonly values.
    /// </summary>
    public static class Constants
    {
        public static readonly StardewModdingAPI.Version Version = new Version(1, 0, 0, "Farmhand-Smapi");

        /// <summary>
        ///     Not quite "constant", but it makes more sense for it to be here, at least for now
        /// </summary>
        public static int ModsLoaded = 0;

        /// <summary>
        ///     Stardew Valley's roaming app data location.
        ///     %AppData%//StardewValley
        /// </summary>
        public static string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley");

        public static string SavesPath => Path.Combine(DataPath, "Saves");

        private static string saveFolderName => PlayerNull ? string.Empty : Game1.player.name.RemoveNumerics() + "_" + Game1.uniqueIDForThisGame;
        public static string SaveFolderName => CurrentSavePathExists ? saveFolderName : "";

        private static string currentSavePath => PlayerNull ? string.Empty : Path.Combine(SavesPath, saveFolderName);
        public static string CurrentSavePath => CurrentSavePathExists ? currentSavePath : "";

        public static bool CurrentSavePathExists => Directory.Exists(currentSavePath);

        public static bool PlayerNull => !Game1.hasLoadedGame || Game1.player == null || string.IsNullOrEmpty(Game1.player.name);

        /// <summary>
        ///     Execution path to execute the code.
        /// </summary>
        public static string ExecutionPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        ///     Title for the API console
        /// </summary>
        public static string ConsoleTitle => $"Stardew Modding API Console - Version {Version} - Mods Loaded: {ModsLoaded}";

        /// <summary>
        ///     Path for log files to be output to.
        ///     %LocalAppData%//StardewValley//ErrorLogs
        /// </summary>
        public static string LogDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley", "ErrorLogs");

        public static string LogPath => Path.Combine(LogDir, "MODDED_ProgramLog.Log_LATEST.txt");

        /// <summary>
        ///     Whether or not to enable the Render Target drawing code offered by ClxS
        ///     Do not mark as 'const' or else 'if' checks will complain that the expression is always true in ReSharper
        /// </summary>
        public static bool EnableDrawingIntoRenderTarget => true;

        /// <summary>
        /// Completely overrides the base game's draw call to the one is SGame
        /// </summary>
        public static bool EnableCompletelyOverridingBaseCalls => true;
    }
}