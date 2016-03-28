using System;
using System.Collections.Generic;
using StardewValley;

namespace StardewModdingAPI.Entities
{
    /// <summary>
    ///     Static class for intergrating with the player
    /// </summary>
    public class SPlayer
    {
        /// <summary>
        /// Calls 'getAllFarmers' in Game1
        /// </summary>
        public static List<Farmer> AllFarmers => Game1.getAllFarmers();

        /// <summary>
        /// Do not use.
        /// </summary>
        [Obsolete("Use 'Player' instead.")]
        public static Farmer CurrentFarmer => Game1.player;

        /// <summary>
        /// Gets the current player from Game1
        /// </summary>
        public static Farmer Player => Game1.player;

        /// <summary>
        /// Gets the player's current location from Game1
        /// </summary>
        public static GameLocation CurrentFarmerLocation => Player.currentLocation;
    }
}