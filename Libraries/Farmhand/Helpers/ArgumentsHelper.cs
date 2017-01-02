namespace Farmhand.Helpers
{
    using System.Collections.Generic;

    using Farmhand.Logging;

    internal class ArgumentsHelper
    {
        // A dictionary containing all possible command line arguments, and the delegates they call
        private static Dictionary<string, ArgumentAction> ArgumentActions { get; } =
            new Dictionary<string, ArgumentAction>
                {
                    { "setModFolder", ArgumentSetModFolder },
                    { "addModFolder", ArgumentAddModFolder }
                };

        /// <summary>
        ///     Take the raw args passed to the program and perform relevant actions
        /// </summary>
        /// <param name="args">space split array of arguments</param>
        internal static void ParseArguments(string[] args)
        {
            // If there are no arguments, there's no need to continue
            if (args.Length == 0)
            {
                return;
            }

            // Iterate over all the arguments, performing actions where requested
            string currentAction = null;
            var subArgs = new List<string>();
            foreach (string arg in args)
            {
                // New actions are signaled with a -
                if (arg.StartsWith("-"))
                {
                    // If we're currently building an argument action, a new action signals that the old one is complete, and usable
                    if (currentAction != null)
                    {
                        PerformAction(currentAction, subArgs.ToArray());

                        // Reset for next action
                        subArgs = new List<string>();
                    }

                    // Now that any old actions have been handled, start building the new one
                    // Be sure to remove that beginning "-"
                    currentAction = arg.Remove(0, 1);
                }
                else
                {
                    // If there is no beginning "-", this is an argument to tack onto an action
                    subArgs.Add(arg);
                }
            }

            // If we've got anything left in currentAction, that's our final argument
            if (currentAction != null)
            {
                PerformAction(currentAction, subArgs.ToArray());
            }
        }

        // performs an action with specified parameters
        private static void PerformAction(string action, string[] args)
        {
            // Make sure this is an action we recognize
            if (ArgumentActions.ContainsKey(action))
            {
                // Call our delegate which is associated with this action
                ArgumentActions[action](args);
            }
            else
            {
                Log.Warning($"Command line argument \"{action}\" unrecognized. Ignoring.");
            }
        }

        // An argument action which will set the folder used for mods
        private static void ArgumentSetModFolder(string[] args)
        {
            if (args.Length != 1)
            {
                Log.Warning(
                    $"Command line argument \"setModFolder\" recieved {args.Length} arguments instead of the expected 1. Ignoring.");
                return;
            }

            // Give args[0], which is the file location, to ModLoader as the new exclusive mod path
            ModLoader.ModPaths = new List<string> { args[0] };
            Log.Success($"Set mod folder path to {args[0]}.");
        }

        // An argument action which will add a folder used for mods
        private static void ArgumentAddModFolder(string[] args)
        {
            if (args.Length != 1)
            {
                Log.Warning(
                    $"Command line argument \"addModFolder\" recieved {args.Length} arguments instead of the expected 1. Ignoring.");
                return;
            }

            // Give args[0], which is the file location, to ModLoader as a new mods path
            ModLoader.ModPaths.Add(args[0]);
            Log.Success($"Added mod folder path {args[0]}.");
        }

        #region Nested type: ArgumentAction

        // The type of delegate called to perform argument actions
        private delegate void ArgumentAction(string[] args);

        #endregion
    }
}