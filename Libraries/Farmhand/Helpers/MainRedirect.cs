using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Farmhand.Attributes;
using StardewValley;
using System.Collections.Generic;
using Farmhand.Extensibility;
using Microsoft.Xna.Framework.Graphics;

namespace Farmhand.Helpers
{
    internal static class MainRedirect
    {
        private static Type _programType;
        private static Type ProgramType => _programType ?? (_programType = Constants.Assembly.GetType("StardewValley.Program"));

        private static FieldInfo _gameTesterModeFieldField;
        private static FieldInfo GameTesterModeField => _gameTesterModeFieldField ?? (_gameTesterModeFieldField = ProgramType.GetField("GameTesterMode"));

        private static FieldInfo _releaseBuildField;
        private static FieldInfo ReleaseBuildField => _releaseBuildField ?? (_releaseBuildField = ProgramType.GetField("releaseBuild"));

        private static FieldInfo _buildTypeField;
        private static FieldInfo BuildTypeField => _buildTypeField ?? (_buildTypeField = ProgramType.GetField("buildType"));

        private static FieldInfo _buildSteamField;
        private static FieldInfo BuildSteamField => _buildSteamField ?? (_buildSteamField = ProgramType.GetField("build_steam"));
        
        private static FieldInfo _buildGogField;
        private static FieldInfo BuildGogField => _buildGogField ?? (_buildGogField = ProgramType.GetField("build_gog"));

        private static FieldInfo _gamePtrField;
        private static FieldInfo GamePtrField => _gamePtrField ?? (_gamePtrField = ProgramType.GetField("gamePtr"));

        private static FieldInfo _handlingExceptionField;
        private static FieldInfo HandlingExceptionField => _handlingExceptionField ?? (_handlingExceptionField = ProgramType.GetField("handlingException"));

        private static FieldInfo _hasTriedToPrintLogField;
        private static FieldInfo HasTriedToPrintLogField => _hasTriedToPrintLogField ?? (_hasTriedToPrintLogField = ProgramType.GetField("hasTriedToPrintLog"));

        private static FieldInfo _successfullyPrintedLogField;
        private static FieldInfo SuccessfullyPrintedLogField => _successfullyPrintedLogField ?? (_successfullyPrintedLogField = ProgramType.GetField("successfullyPrintedLog"));
        
        private static bool GameTesterMode
        {
            get
            {
                return (bool)GameTesterModeField.GetValue(null);
            }
            set
            {
                GameTesterModeField.SetValue(null, value);
            }
        }
        private static bool ReleaseBuild
        {
            get
            {
                return (bool)ReleaseBuildField.GetValue(null);
            }
            set
            {
                ReleaseBuildField.SetValue(null, value);
            }
        }
        private static bool BuildType
        {
            get
            {
                return (bool)BuildTypeField.GetValue(null);
            }
            set
            {
                BuildTypeField.SetValue(null, value);
            }
        }
        private static Game1 GamePtr
        {
            get
            {
                return (Game1)GamePtrField.GetValue(null);
            }
            set
            {
                GamePtrField.SetValue(null, value);
            }
        }
        private static bool HandlingException
        {
            get
            {
                return (bool)HandlingExceptionField.GetValue(null);
            }
            set
            {
                HandlingExceptionField.SetValue(null, value);
            }
        }
        private static bool HasTriedToPrintLog
        {
            get
            {
                return (bool)HasTriedToPrintLogField.GetValue(null);
            }
            set
            {
                HasTriedToPrintLogField.SetValue(null, value);
            }
        }
        private static bool SuccessfullyPrintedLog
        {
            get
            {
                return (bool)SuccessfullyPrintedLogField.GetValue(null);
            }
            set
            {
                SuccessfullyPrintedLogField.SetValue(null, value);
            }
        }
        
        [Hook(HookType.Entry, "StardewValley.Program", "Main")]
        internal static bool MainRedirectFunction([InputBind(typeof(string[]), "args")] string[] args)
        {
            ArgumentsHelper.ParseArguments(args);

            Program.ReadConfig();

            ExtensibilityManager.TryLoadModCompatiblityLayers();

            if (Program.Config.DebugMode)
                ReleaseBuild = false;

            GameTesterMode = true;
            AppDomain.CurrentDomain.UnhandledException += HandleException;
            using (var game1 = API.Game.CreateGameInstance())
            {
                try
                {
                    ExtensibilityManager.SetGameInstance(game1);
                    GamePtr = game1;
                    game1.Run();
                }
                catch (System.Exception ex)
                {
                    Logging.Log.Exception("Encountered an error", ex);
                }
                
            }
            return false;
        }

        [Hook(HookType.Exit, "StardewValley.Game1", ".ctor")]
        internal static void CorrectGraphicsProfile()
        {
            Game1.graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        private static void HandleException(object sender, UnhandledExceptionEventArgs args)
        {
            if (HandlingException || !GameTesterMode)
                return;
            Game1.gameMode = 11;
            HandlingException = true;
            var exception = (Exception)args.ExceptionObject;
            Game1.errorMessage = "Message: " + (object)exception.Message + Environment.NewLine + "InnerException: " + (string)(object)exception.InnerException + Environment.NewLine + "Stack Trace: " + exception.StackTrace;
            var num = DateTime.Now.Ticks / 10000L;
            if (!HasTriedToPrintLog)
            {
                HasTriedToPrintLog = true;
                var path = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "ErrorLogs"), (string)(Game1.player != null ? (object)Game1.player.name : (object)"NullPlayer") + (object)"_" + (string)(object)Game1.uniqueIDForThisGame + "_" + (string)(object)(Game1.player != null ? (int)Game1.player.millisecondsPlayed : Game1.random.Next(999999)) + ".txt");
                FileInfo fileInfo = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "ErrorLogs"), "asdfasdf"));
                if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                    fileInfo.Directory.Create();
                if (File.Exists(path))
                    File.Delete(path);
                try
                {
                    File.WriteAllText(path, Game1.errorMessage);
                    SuccessfullyPrintedLog = true;
                    Game1.errorMessage = "(Error Report created at " + path + ")" + Environment.NewLine + Game1.errorMessage;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            Game1.gameMode = 3;
        }
    }
}
