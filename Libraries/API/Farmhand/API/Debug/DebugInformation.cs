namespace Farmhand.API.Debug
{
    using System;

    /// <summary>
    ///     Information about a debug command handler.
    /// </summary>
    public class DebugInformation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DebugInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owning mod of this command.
        /// </param>
        /// <param name="callback">
        ///     The handler callback function with the signature: "bool Func(string command, string[] parameters)",
        ///     where the return value is if the command was handled.
        /// </param>
        public DebugInformation(Mod owner, Func<string, string[], bool> callback)
        {
            this.Owner = owner;
            this.Callback = callback;
        }

        /// <summary>
        ///     Gets or sets the owning mod of this handler.
        /// </summary>
        public Mod Owner { get; set; }

        /// <summary>
        ///     Gets or sets the callback with the signature: "bool Func(string command, string[] parameters)",
        ///     where the return value is if the command was handled.
        /// </summary>
        public Func<string, string[], bool> Callback { get; set; }
    }
}