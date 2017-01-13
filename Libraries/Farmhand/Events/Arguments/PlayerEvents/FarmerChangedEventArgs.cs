namespace Farmhand.Events.Arguments.PlayerEvents
{
    using System;

    using StardewValley;

    /// <summary>
    ///     Arguments for FarmerChanged.
    /// </summary>
    public class FarmerChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FarmerChangedEventArgs" /> class.
        /// </summary>
        /// <param name="priorFarmer">
        ///     The previous farmer.
        /// </param>
        /// <param name="newFarmer">
        ///     The new farmer.
        /// </param>
        public FarmerChangedEventArgs(Farmer priorFarmer, Farmer newFarmer)
        {
            this.PreviousFarmer = priorFarmer;
            this.NewFarmer = newFarmer;
        }

        /// <summary>
        ///     Gets the previous farmer.
        /// </summary>
        public Farmer PreviousFarmer { get; }

        /// <summary>
        ///     Gets the new farmer.
        /// </summary>
        public Farmer NewFarmer { get; }
    }
}