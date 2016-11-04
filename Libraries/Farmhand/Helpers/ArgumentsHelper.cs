using System.Collections.Generic;

namespace Farmhand.Helpers
{
    public class ArgumentsHelper
    {
        // The type of delegate called to perform argument actions
        private delegate void ArgumentAction(string[] args);

        // A dictionary containing all possible command line arguments, and the delegates they call
        private static Dictionary<string, ArgumentAction> argumentActions = new Dictionary<string, ArgumentAction>()
        {
            { "setModFolder", new ArgumentAction(ArgumentSetModFolder) },
            { "addModFolder", new ArgumentAction(ArgumentAddModFolder) }
        };

        /// <summary>
        /// Take the raw args passed to the program and perform relevant actions
        /// </summary>
        /// <param name="args">space split array of arguments</param>
        internal static void ParseArguments(string[] args)
        {
            // If there are no arguments, there's no need to continue
            if (args.Length == 0)
            {
                return;
            }

            // Iterrate over all the arguments, performing actions where requested
            string currentAction = null;
            List<string> subArgs = new List<string>();
            for (int i = 0; i < args.Length; i++)
            {
                // New actions are signalled with a -
                if(args[i].StartsWith("-"))
                {
                    // If we're currently building an argument action, a new action signals that the old one is complete, and usable
                    if(currentAction != null)
                    {
                        PerformAction(currentAction, subArgs.ToArray());

                        // Reset for next action
                        currentAction = null;
                        subArgs = new List<string>();
                    }

                    // Now that any old actions have been handled, start building the new one
                    // Be sure to remove that beginning "-"
                    currentAction = args[i].Remove(0, 1);
                }
                // If there is no beginning "-", this is an argument to tack onto an action
                else
                {
                    subArgs.Add(args[i]);
                }
            }

            // If we've got anything left in currentAction, that's our final argument
            if(currentAction != null)
            {
                PerformAction(currentAction, subArgs.ToArray());
            }
        }

        // performs an action with specified parameters
        private static void PerformAction(string action, string[] args)
        {
            // Make sure this is an action we recognize
            if (argumentActions.ContainsKey(action))
            {
                // Call our delegate which is associated with this action
                argumentActions[action](args);
            }
            else
            {
                Farmhand.Logging.Log.Warning($"Command line argument \"{action}\" unrecognized. Ignoring.");
            }
        }

        // An argument action which will set the folder used for mods
        private static void ArgumentSetModFolder(string[] args)
        {
            if (args.Length != 1)
            {
                Farmhand.Logging.Log.Warning($"Command line argument \"setModFolder\" recieved {args.Length} arguments instead of the expected 1. Ignoring.");
                return;
            }

            // Give args[0], which is the file location, to ModLoader as the new exclusive mod path
            ModLoader.ModPaths = new List<string>
            {
                args[0]
            };
            Farmhand.Logging.Log.Success($"Set mod folder path to {args[0]}.");
        }

        // An argument action which will add a folder used for mods
        private static void ArgumentAddModFolder(string[] args)
        {
            if (args.Length != 1)
            {
                Farmhand.Logging.Log.Warning($"Command line argument \"addModFolder\" recieved {args.Length} arguments instead of the expected 1. Ignoring.");
                return;
            }

            // Give args[0], which is the file location, to ModLoader as a new mods path
            ModLoader.ModPaths.Add(args[0]);
            Farmhand.Logging.Log.Success($"Added mod folder path {args[0]}.");
        }
    }
}
