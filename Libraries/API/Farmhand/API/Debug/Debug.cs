namespace Farmhand.API.Debug
{
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.Attributes;
    using Farmhand.Logging;

    using StardewValley;

    /// <summary>
    ///     Debug-related API functionality. This utility allows you to handle commands passed to the
    ///     game's "parseDebugInput" method and add your own callback.
    /// </summary>
    public static class Debug
    {
        private static readonly Dictionary<string, DebugInformation> DebugCommands =
            new Dictionary<string, DebugInformation>();

        /// <summary>
        ///     Registers a debug command handler.
        /// </summary>
        /// <param name="command">
        ///     The name of the command to respond to.
        /// </param>
        /// <param name="debugInformation">
        ///     The information on this handler.
        /// </param>
        public static void RegisterDebugCommand(string command, DebugInformation debugInformation)
        {
            if (DebugCommands.ContainsKey(command))
            {
                Log.Warning(
                    $"Potential conflict registering new debug command. Command {command} is already registered by {DebugCommands[command].Owner?.ModSettings?.Name ?? "another mod."} only the last registered will be used.");
            }

            DebugCommands[command] = debugInformation;
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "parseDebugInput")]
        internal static bool ParseDebugInput([InputBind(typeof(string), "debugInput")] string debugInput)
        {
            if (debugInput == null)
            {
                return false;
            }

            var useOutput = false;
            debugInput = debugInput.Trim();
            var command = debugInput.Split(' ')[0];
            var parameters = debugInput.Split(' ').Skip(1).ToArray();

            if (command == null)
            {
                return false;
            }

            if (DebugCommands.ContainsKey(command))
            {
                Game1.exitActiveMenu();
                Game1.lastDebugInput = debugInput;

                useOutput = DebugCommands[command].Callback.Invoke(command, parameters);
            }

            return useOutput;
        }
    }
}