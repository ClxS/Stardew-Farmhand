using System;
using System.Collections.Generic;
using Farmhand.Attributes;
using StardewValley;

namespace Farmhand.API
{
    internal static class Game
    {
        public static Game1 CreateGameInstance()
        { 
            return new Game1();
        }


    }
}
