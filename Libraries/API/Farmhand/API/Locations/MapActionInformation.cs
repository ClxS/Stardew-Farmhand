namespace Farmhand.API.Locations
{
    using System;

    /// <summary>
    ///     Information about a map action.
    /// </summary>
    public class MapActionInformation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MapActionInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owning mod.
        /// </param>
        /// <param name="action">
        ///     The action to listen for.
        /// </param>
        /// <param name="callback">
        ///     The callback to call when the action occurs.
        /// </param>
        public MapActionInformation(Mod owner, string action, Func<string, bool> callback)
        {
            this.Owner = owner;
            this.Action = action;
            this.Callback = callback;
        }

        /// <summary>
        ///     Gets or sets the action.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        ///     Gets or sets the callback.
        /// </summary>
        public Func<string, bool> Callback { get; set; }

        /// <summary>
        ///     Gets or sets the owning mod.
        /// </summary>
        public Mod Owner { get; set; }
    }
}