using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Framework;
using StardewValley;
using Monitor = StardewModdingAPI.Framework.Monitor;

namespace SmapiCompatibilityLayer
{
    class CompatibilityLayer : Farmhand.Helpers.CompatibilityLayer
    {
        public override string ModSubdirectory => "SMAPI";

        private static LogFileManager _logFile;
        private static LogFileManager LogFile
        {
            get
            {
                if (_logFile != null)
                {
                    return _logFile;
                }

                var projectType = typeof(StardewModdingAPI.Program);
                var fieldInfo = projectType.GetField("LogFile", BindingFlags.Static | BindingFlags.NonPublic);
                _logFile = (LogFileManager)fieldInfo?.GetValue(null);
                return _logFile;
            }
        }

        private static bool? _developerMode;
        private static bool DeveloperMode
        {
            get
            {
                if (_developerMode != null)
                {
                    return _developerMode.Value;
                }

                var projectType = typeof(StardewModdingAPI.Program);
                var fieldInfo = projectType.GetField("DeveloperMode", BindingFlags.Static | BindingFlags.NonPublic);
                _developerMode = (bool)fieldInfo.GetValue(null);
                return _developerMode.Value;
            }
        }

        public override void Initialise()
        {
            Farmhand.Events.GameEvents.OnBeforeLoadContent += GameEvents_OnBeforeLoadContent;

            // set thread culture for consistent log formatting
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

            // initialise legacy log
            Log.Monitor = new Monitor("legacy mod", LogFile) { ShowTraceInConsole = DeveloperMode };
            Log.ModRegistry = Program.ModRegistry;

            // add info header
            Log.Monitor.Log($"Farmhand-SMAPI {Constants.Version} with Stardew Valley {Game1.version} on {Environment.OSVersion}", LogLevel.Info);

            EventForwarder.ForwardEvents();
        }

        public override void LoadMods(string modsDirectory)
        {
            var smapiModsDirectory = Path.Combine(modsDirectory, ModSubdirectory);
            if (!Directory.Exists(smapiModsDirectory))
            {
                Directory.CreateDirectory(smapiModsDirectory);
            }

            var projectType = typeof(StardewModdingAPI.Program);
            FieldInfo modPathField = projectType.GetField("ModPath", BindingFlags.Static | BindingFlags.NonPublic);
            modPathField?.SetValue(null, smapiModsDirectory);

            MethodInfo loadModsMethod = projectType.GetMethod("LoadMods", BindingFlags.Static | BindingFlags.NonPublic);
            loadModsMethod.Invoke(null, new object[] {});
        }

        private void GameEvents_OnBeforeLoadContent(object sender, EventArgs e)
        {
            Game1.graphics.GraphicsProfile = GraphicsProfile.HiDef;
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
