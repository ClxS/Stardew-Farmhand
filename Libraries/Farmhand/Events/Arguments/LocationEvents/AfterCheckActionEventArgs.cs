namespace Farmhand.Events.Arguments.LocationEvents
{
    using Farmhand.Events.Arguments.Common;

    using StardewValley;

    using xTile.Dimensions;

    /// <summary>
    ///     Arguments for BeforeCheckAction.
    /// </summary>
    public class AfterCheckActionEventArgs : ReturnableEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AfterCheckActionEventArgs" /> class.
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
        /// <param name="wasHandled">
        ///     Whether the check action was handled.
        /// </param>
        public AfterCheckActionEventArgs(
            GameLocation gameLocation,
            Location tileLocation,
            Rectangle viewport,
            Farmer farmer,
            bool wasHandled)
        {
            this.GameLocation = gameLocation;
            this.TileLocation = tileLocation;
            this.Viewport = viewport;
            this.Farmer = farmer;
            this.WasHandled = wasHandled;
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
        ///     Gets a value indicating whether was the check action was handled.
        /// </summary>
        public bool WasHandled { get; }
    }
}