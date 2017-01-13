namespace Farmhand.Events.Arguments.LocationEvents
{
    using System;

    /// <summary>
    ///     Arguments for GetMonsterForThisLevel.
    /// </summary>
    public class GetMonsterForThisLevelEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GetMonsterForThisLevelEventArgs" /> class.
        /// </summary>
        /// <param name="level">
        ///     The level.
        /// </param>
        /// <param name="xTile">
        ///     The x tile.
        /// </param>
        /// <param name="yTile">
        ///     The y tile.
        /// </param>
        public GetMonsterForThisLevelEventArgs(int level, int xTile, int yTile)
        {
            this.Level = level;
            this.XTile = xTile;
            this.YTile = yTile;
        }

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     Gets or sets the x tile.
        /// </summary>
        public int XTile { get; set; }

        /// <summary>
        ///     Gets or sets the y tile.
        /// </summary>
        public int YTile { get; set; }
    }
}