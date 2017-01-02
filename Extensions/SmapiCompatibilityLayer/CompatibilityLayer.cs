namespace SmapiCompatibilityLayer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using StardewModdingAPI;
    using StardewModdingAPI.Framework;
    using StardewModdingAPI.Inheritance;

    using StardewValley;

    using Monitor = StardewModdingAPI.Framework.Monitor;

    internal class CompatibilityLayer : Farmhand.Extensibility.FarmhandExtension
    {
        public static IMonitor Monitor
        {
            get
            {
                if (monitor != null)
                {
                    return monitor;
                }

                var projectType = typeof(Program);
                var fieldInfo = projectType.GetField("Monitor", BindingFlags.Static | BindingFlags.NonPublic);
                if (fieldInfo != null)
                {
                    monitor = (IMonitor)fieldInfo.GetValue(null);
                }

                return monitor;
            }
        }

        public override string ModSubdirectory => this.Manifest.ModsFolder;

        public override Type GameOverrideClass => typeof(SmapiGameOverride);

        private static LogFileManager logFile;

        private static LogFileManager LogFile
        {
            get
            {
                if (logFile != null)
                {
                    return logFile;
                }

                var projectType = typeof(Program);
                var fieldInfo = projectType.GetField("LogFile", BindingFlags.Static | BindingFlags.NonPublic);
                logFile = (LogFileManager)fieldInfo?.GetValue(null);
                return logFile;
            }
        }

        private static bool? developerMode;

        private static bool DeveloperMode
        {
            get
            {
                if (developerMode != null)
                {
                    return developerMode.Value;
                }

                var projectType = typeof(Program);
                var fieldInfo = projectType.GetField("DeveloperMode", BindingFlags.Static | BindingFlags.NonPublic);
                developerMode = fieldInfo != null && (bool)fieldInfo.GetValue(null);
                return developerMode.Value;
            }
        }

        private static IMonitor monitor;
        
        public override Game1 GameInstance
        {
            get { return Program.gamePtr; }
            set { Program.gamePtr = (SGame)value; }
        }

        public override void Initialise()
        {
            // set thread culture for consistent log formatting
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

            // initialise legacy log
            Log.Monitor = new Monitor("legacy mod", LogFile) { ShowTraceInConsole = DeveloperMode };
            Log.ModRegistry = Program.ModRegistry;

            // add info header
            Log.Monitor.Log($"Farmhand-SMAPI {Constants.Version} with Stardew Valley {Game1.version} on {Environment.OSVersion}", LogLevel.Info);
        }

        public override void LoadMods(string modsDirectory)
        {
            var smapiModsDirectory = Path.Combine(modsDirectory, this.ModSubdirectory);
            if (!Directory.Exists(smapiModsDirectory))
            {
                Directory.CreateDirectory(smapiModsDirectory);
            }

            var projectType = typeof(Program);
            FieldInfo modPathField = projectType.GetField("ModPath", BindingFlags.Static | BindingFlags.NonPublic);
            modPathField?.SetValue(null, smapiModsDirectory);

            MethodInfo loadModsMethod = projectType.GetMethod("LoadMods", BindingFlags.Static | BindingFlags.NonPublic);
            loadModsMethod.Invoke(null, new object[] { });
        }

        public override IEnumerable<Type> GetEventClasses()
        {
            return Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => string.Equals(t.Namespace, "StardewModdingAPI.Events", StringComparison.Ordinal))
                    .ToList();
        }
    }
}
