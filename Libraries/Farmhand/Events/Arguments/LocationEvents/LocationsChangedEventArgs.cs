namespace Farmhand.Events.Arguments.LocationEvents
{
    using System;
    using System.Collections.Generic;

    using StardewValley;

    /// <summary>
    ///     Arguments for LocationsChanged.
    /// </summary>
    public class LocationsChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LocationsChangedEventArgs" /> class.
        /// </summary>
        /// <param name="locations">
        ///     The locations.
        /// </param>
        public LocationsChangedEventArgs(List<GameLocation> locations)
        {
            this.NewLocations = locations;
        }

        /// <summary>
        ///     Gets the new locations.
        /// </summary>
        public List<GameLocation> NewLocations { get; }
    }
}