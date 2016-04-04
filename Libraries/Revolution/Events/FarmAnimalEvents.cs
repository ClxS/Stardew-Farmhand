using Revolution.Attributes;
using Revolution.Events.Arguments;
using System;
using System.ComponentModel;

namespace Revolution.Events
{
    public static class FarmAnimalEvents
    {
        public static EventHandler<EventArgsOnAnimalEatGrass> OnBeforeEatGrass = delegate { };
        public static EventHandler<CancelEventArgs> OnMakeSound = delegate { };
        public static EventHandler<CancelEventArgs> OnFarmerPushing = delegate { };
        
        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "eatGrass")]
        internal static bool InvokeOnBeforeEatGrass([ThisBind] object @this)
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeEatGrass, @this, new EventArgsOnAnimalEatGrass());
        }

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "makeSound")]
        internal static bool InvokeOnMakeSound([ThisBind] object @this)
        {
            return EventCommon.SafeCancellableInvoke(OnMakeSound, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "farmerPushing")]
        internal static bool InvokeOnFarmerPushing([ThisBind] object @this)
        {
            return EventCommon.SafeCancellableInvoke(OnFarmerPushing, @this, new CancelEventArgs());
        }
    }
}
