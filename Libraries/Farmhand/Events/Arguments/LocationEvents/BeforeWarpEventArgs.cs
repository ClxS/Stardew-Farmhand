namespace Farmhand.Events.Arguments.LocationEvents
{
    using System.ComponentModel;

    using StardewValley;

    /// <summary>
    ///     Arguments for BeforeWarp.
    /// </summary>
    public class BeforeWarpEventArgs : CancelEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BeforeWarpEventArgs" /> class.
        /// </summary>
        /// <param name="locationAfterWarp">
        ///     The location after warp.
        /// </param>
        /// <param name="tileX">
        ///     The tile x.
        /// </param>
        /// <param name="tileY">
        ///     The tile y.
        /// </param>
        /// <param name="facingDirectionAfterWarp">
        ///     The facing direction after warp.
        /// </param>
        public BeforeWarpEventArgs(GameLocation locationAfterWarp, int tileX, int tileY, int facingDirectionAfterWarp)
        {
            this.LocationAfterWarp = locationAfterWarp;
            this.TileX = tileX;
            this.TileY = tileY;
            this.FacingDirectionAfterWarp = facingDirectionAfterWarp;
        }

        /// <summary>
        ///     Gets the location after warp.
        /// </summary>
        public GameLocation LocationAfterWarp { get; }

        /// <summary>
        ///     Gets the tile x position.
        /// </summary>
        public int TileX { get; }

        /// <summary>
        ///     Gets the tile y position.
        /// </summary>
        public int TileY { get; }

        /// <summary>
        ///     Gets the facing direction after warp.
        /// </summary>
        public int FacingDirectionAfterWarp { get; }
    }
}