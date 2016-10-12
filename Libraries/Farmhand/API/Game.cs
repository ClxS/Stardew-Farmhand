using System;
using StardewValley;

namespace Farmhand.API
{
    internal static class Game
    {
        public static Type OverrideType { get; private set; }

        public static void RegisterGameOverride(Type t) 
        {
            if (OverrideType != null)
            {
                throw new Exception("ERROR, Attempting to override Game1 multiple times!");
            }
            OverrideType = t;
        }

        public static Game1 CreateGameInstance()
        {
            if (OverrideType != null)
            {
                Logging.Log.Success("Constructing from override");
                return (Game1)Activator.CreateInstance(OverrideType);
            }
            return (Game1)Activator.CreateInstance(Constants.Assembly.GetType("Farmhand.Overrides.Game1"));
        }


    }
}
