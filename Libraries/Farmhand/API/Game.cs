using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand.Attributes;
using StardewValley;

namespace Farmhand.API
{
    internal static class Game
    {
        public static Game1 CreateGameInstance()
        {
            var overridingExtension = ModLoader.CompatibilityLayers.FirstOrDefault(e => e.GameOverrideClass != null);
            if (overridingExtension != null)
            {
                return (Game1)Activator.CreateInstance(overridingExtension.GameOverrideClass);
            }

            return new Game1();
        }


    }
}
