using System;
using System.Collections.Generic;
using StardewModdingAPI.Events;

namespace StardewModdingAPI
{
    public class Command
    {
        internal static List<Command> RegisteredCommands = new List<Command>();
        public string[] CalledArgs;
        public string[] CommandArgs;
        public string CommandDesc;

        public string CommandName;

        /// <summary>
        ///     Creates a Command from a Name, Description, and Arguments
        /// </summary>
        /// <param name="cname">Name</param>
        /// <param name="cdesc">Description</param>
        /// <param name="args">Arguments</param>
        public Command(string cname, string cdesc, string[] args = null)
        {
            CommandName = cname;
            CommandDesc = cdesc;
            if (args == null)
                args = new string[0];
            CommandArgs = args;
        }

        public event EventHandler<EventArgsCommand> CommandFired;

        /// <summary>
        ///     Calls the specified command. (It runs the command)
        /// </summary>
        /// <param name="input">The command to run</param>
        public static void CallCommand(string input)
        {
            input = input.TrimEnd(' ');
            var args = new string[0];
            Command fnd;
            if (input.Contains(" "))
            {
                args = input.Split(new[] {" "}, 2, StringSplitOptions.RemoveEmptyEntries);
                fnd = FindCommand(args[0]);
                args = args[1].Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                fnd = FindCommand(input);
            }

            if (fnd != null)
            {
                fnd.CalledArgs = args;
                fnd.Fire();
            }
            else
            {
                Log.AsyncR("Unknown Command");
            }
        }

        /// <summary>
        ///     Registers a command to the list of commands properly
        /// </summary>
        /// <param name="command">Name of the command to register</param>
        /// <param name="cdesc">Description</param>
        /// <param name="args">Arguments (these are purely for viewing so that a user can see what an argument needs to be)</param>
        /// <returns></returns>
        public static Command RegisterCommand(string command, string cdesc, string[] args = null)
        {
            var c = new Command(command, cdesc, args);
            if (RegisteredCommands.Contains(c))
            {
                Log.AsyncR($"Command already registered! [{c.CommandName}]");
                return RegisteredCommands.Find(x => x.Equals(c));
            }

            RegisteredCommands.Add(c);
            Log.Async("Registered command: " + command);

            return c;
        }

        /// <summary>
        ///     Looks up a command in the list of registered commands. Returns null if it doesn't exist (I think)
        /// </summary>
        /// <param name="name">Name of command to find</param>
        /// <returns></returns>
        public static Command FindCommand(string name)
        {
            return RegisteredCommands.Find(x => x.CommandName.Equals(name));
        }

        /// <summary>
        ///     Runs a command. Fires it. Calls it. Any of those.
        /// </summary>
        public void Fire()
        {
            if (CommandFired == null)
            {
                Log.AsyncR("Command failed to fire because it's fire event is null: " + CommandName);
                return;
            }
            CommandFired.Invoke(this, new EventArgsCommand(this));
        }
    }
}