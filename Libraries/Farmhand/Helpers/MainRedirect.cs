namespace Farmhand.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;

    using Farmhand.API;
    using Farmhand.Attributes;
    using Farmhand.Extensibility;
    using Farmhand.Logging;

    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    internal static class MainRedirect
    {
        private static Type programType;

        private static FieldInfo gameTesterModeFieldField;

        private static FieldInfo releaseBuildField;

        private static FieldInfo buildTypeField;

        private static FieldInfo buildSteamField;

        private static FieldInfo buildGogField;

        private static FieldInfo gamePtrField;

        private static FieldInfo handlingExceptionField;

        private static FieldInfo hasTriedToPrintLogField;

        private static FieldInfo successfullyPrintedLogField;

        private static Type ProgramType
            => programType ?? (programType = Constants.Assembly.GetType("StardewValley.Program"));

        private static FieldInfo GameTesterModeField
            => gameTesterModeFieldField ?? (gameTesterModeFieldField = ProgramType.GetField("GameTesterMode"));

        private static FieldInfo ReleaseBuildField
            => releaseBuildField ?? (releaseBuildField = ProgramType.GetField("releaseBuild"));

        private static FieldInfo BuildTypeField
            => buildTypeField ?? (buildTypeField = ProgramType.GetField("buildType"));

        private static FieldInfo BuildSteamField
            => buildSteamField ?? (buildSteamField = ProgramType.GetField("build_steam"));

        private static FieldInfo BuildGogField => buildGogField ?? (buildGogField = ProgramType.GetField("build_gog"))
        ;

        private static FieldInfo GamePtrField => gamePtrField ?? (gamePtrField = ProgramType.GetField("gamePtr"));

        private static FieldInfo HandlingExceptionField
            => handlingExceptionField ?? (handlingExceptionField = ProgramType.GetField("handlingException"));

        private static FieldInfo HasTriedToPrintLogField
            => hasTriedToPrintLogField ?? (hasTriedToPrintLogField = ProgramType.GetField("hasTriedToPrintLog"));

        private static FieldInfo SuccessfullyPrintedLogField
            =>
                successfullyPrintedLogField
                ?? (successfullyPrintedLogField = ProgramType.GetField("successfullyPrintedLog"));

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

            ExtensibilityManager.TryLoadExtensions();

            if (Program.Config.DebugMode)
            {
                ReleaseBuild = false;
            }

            GameTesterMode = true;
            AppDomain.CurrentDomain.UnhandledException += HandleException;
            using (var game1 = Game.CreateGameInstance())
            {
                try
                {
                    ExtensibilityManager.SetGameInstance(game1);
                    GamePtr = game1;
                    game1.Run();
                }
                catch (Exception ex)
                {
                    Log.Exception("Encountered an error", ex);
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
            {
                return;
            }

            Game1.gameMode = 11;
            HandlingException = true;
            var exception = (Exception)args.ExceptionObject;
            Game1.errorMessage = "Message: " + (object)exception.Message + Environment.NewLine + "InnerException: "
                                 + (string)(object)exception.InnerException + Environment.NewLine + "Stack Trace: "
                                 + exception.StackTrace;
            var num = DateTime.Now.Ticks / 10000L;
            if (!HasTriedToPrintLog)
            {
                HasTriedToPrintLog = true;
                var path =
                    Path.Combine(
                        Path.Combine(
                            Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "StardewValley"),
                            "ErrorLogs"),
                        (string)(Game1.player != null ? (object)Game1.player.name : (object)"NullPlayer") + (object)"_"
                        + (string)(object)Game1.uniqueIDForThisGame + "_"
                        + (string)
                        (object)
                        (Game1.player != null ? (int)Game1.player.millisecondsPlayed : Game1.random.Next(999999))
                        + ".txt");
                var fileInfo =
                    new FileInfo(
                        Path.Combine(
                            Path.Combine(
                                Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    "StardewValley"),
                                "ErrorLogs"),
                            "asdfasdf"));
                if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                try
                {
                    File.WriteAllText(path, Game1.errorMessage);
                    SuccessfullyPrintedLog = true;
                    Game1.errorMessage = "(Error Report created at " + path + ")" + Environment.NewLine
                                         + Game1.errorMessage;
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