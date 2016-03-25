using StardewValley;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Entities
{
    public static class SPlayer
    {
        public static List<Farmer> AllFarmers => Game1.getAllFarmers();

        public static Farmer CurrentFarmer => Game1.player;

        public static GameLocation CurrentFarmerLocation => Game1.player.currentLocation;
    }
}
