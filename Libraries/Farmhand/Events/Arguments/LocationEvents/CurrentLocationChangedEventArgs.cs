namespace Farmhand.Events.Arguments.LocationEvents
{
    using System;

    using StardewValley;

    /// <summary>
    ///     Arguments for CurrentLocationChanged.
    /// </summary>
    public class CurrentLocationChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CurrentLocationChangedEventArgs" /> class.
        /// </summary>
        /// <param name="priorLocation">
        ///     The prior location.
        /// </param>
        /// <param name="newLocation">
        ///     The new location.
        /// </param>
        public CurrentLocationChangedEventArgs(GameLocation priorLocation, GameLocation newLocation)
        {
            this.PriorLocation = priorLocation;
            this.NewLocation = newLocation;
        }

        /// <summary>
        ///     Gets the prior location.
        /// </summary>
        public GameLocation PriorLocation { get; }

        /// <summary>
        ///     Gets the new location.
        /// </summary>
        public GameLocation NewLocation { get; }
    }
}