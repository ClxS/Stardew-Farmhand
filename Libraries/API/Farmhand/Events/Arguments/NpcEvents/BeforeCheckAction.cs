namespace Farmhand.Events.Arguments.NpcEvents
{
    using Farmhand.Events.Arguments.Common;

    using StardewValley;

    /// <summary>
    ///     Arguments for BeforeCheckAction.
    /// </summary>
    public class BeforeCheckActionEventArgs : ReturnableEventArgs
    {
        private bool handled;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BeforeCheckActionEventArgs" /> class.
        /// </summary>
        /// <param name="gameLocation">
        ///     The game location.
        /// </param>
        /// <param name="farmer">
        ///     The farmer.
        /// </param>
        public BeforeCheckActionEventArgs(GameLocation gameLocation, Farmer farmer)
        {
            this.GameLocation = gameLocation;
            this.Farmer = farmer;
            this.handled = false;
        }

        /// <summary>
        ///     Gets the location where this event is
        /// </summary>
        public GameLocation GameLocation { get; }

        /// <summary>
        ///     Gets the farmer who triggered this event
        /// </summary>
        public Farmer Farmer { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this event has been handled by a mod.
        /// </summary>
        /// <remarks>
        ///     Note that if another mod has handled this action,
        ///     setting this to false will not prevent that mod marking it as handled.
        /// </remarks>
        public bool Handled
        {
            get
            {
                return this.handled;
            }

            set
            {
                this.handled = value;
                this.IsHandled = true;
            }
        }
    }
}