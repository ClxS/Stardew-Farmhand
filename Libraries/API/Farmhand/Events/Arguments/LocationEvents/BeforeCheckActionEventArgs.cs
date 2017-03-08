namespace Farmhand.Events.Arguments.LocationEvents
{
    using Farmhand.Events.Arguments.Common;

    using StardewValley;

    using xTile.Dimensions;

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
        /// <param name="tileLocation">
        ///     The tile location.
        /// </param>
        /// <param name="viewport">
        ///     The viewport.
        /// </param>
        /// <param name="farmer">
        ///     The farmer.
        /// </param>
        public BeforeCheckActionEventArgs(
            GameLocation gameLocation,
            Location tileLocation,
            Rectangle viewport,
            Farmer farmer)
        {
            this.GameLocation = gameLocation;
            this.TileLocation = tileLocation;
            this.Viewport = viewport;
            this.Farmer = farmer;
            this.handled = false;
        }

        /// <summary>
        ///     Gets the location where this event is
        /// </summary>
        public GameLocation GameLocation { get; }

        /// <summary>
        ///     Gets the tile location of the event
        /// </summary>
        public Location TileLocation { get; }

        /// <summary>
        ///     Gets the current screen viewport
        /// </summary>
        public Rectangle Viewport { get; }

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