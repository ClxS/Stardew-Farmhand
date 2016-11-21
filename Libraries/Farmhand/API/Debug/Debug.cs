using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.Attributes;
using StardewValley;

namespace Farmhand.API.Debug
{
    public class Debug
    {
        private static readonly Dictionary<string, DebugInformation> DebugCommands = new Dictionary<string, DebugInformation>();

        public static void RegisterDebugCommand(string command, DebugInformation debugInformation)
        {
            if (DebugCommands.ContainsKey(command)) {
                Logging.Log.Warning($"Potential conflict registering new debug command. Command {command} is already registered by {(DebugCommands[command].Owner?.ModSettings?.Name ?? "another mod.")} only the last registered will be used.");
            }
            DebugCommands[command] = debugInformation;
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "parseDebugInput")]
        internal static bool parseDebugInput([InputBind(typeof(string), "debugInput")] string debugInput)
        {
            var useOutput = false;
            debugInput = debugInput?.Trim();
            var command = debugInput?.Split(' ')[0];
            var parameters = debugInput?.Split(' ').Skip(1).ToArray();

            if (command == null)
                return useOutput;

            if (DebugCommands.ContainsKey(command)) {
                Game1.exitActiveMenu();
                Game1.lastDebugInput = debugInput;

                useOutput = DebugCommands[command].Callback.Invoke(command, parameters);
            }

            return useOutput;
        }
    }
}
