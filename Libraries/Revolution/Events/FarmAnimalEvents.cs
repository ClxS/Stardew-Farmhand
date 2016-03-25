using Revolution.Attributes;
using Revolution.Events.Arguments;
using System;

namespace Revolution.Events
{
    public static class FarmAnimalEvents
    {
        public static EventHandler<EventArgsOnAnimalEatGrass> OnBeforeEatGrass = delegate { };
        public static EventHandler OnMakeSound = delegate { };
        public static EventHandler OnFarmerPushing = delegate { };

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "eatGrass")]
        internal static void InvokeOnBeforeEatGrass()
        {
            EventCommon.SafeInvoke(OnBeforeEatGrass, null, new EventArgsOnAnimalEatGrass());
        }

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "makeSound")]
        internal static void InvokeOnMakeSound()
        {
            EventCommon.SafeInvoke(OnMakeSound, null);
        }

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "farmerPushing")]
        internal static void InvokeOnFarmerPushing()
        {
            EventCommon.SafeInvoke(OnFarmerPushing, null);
        }
    }
}
