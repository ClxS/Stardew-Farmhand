using Farmhand.Attributes;
using Farmhand.Events.Arguments;
using System;
using System.ComponentModel;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to farm animals
    /// </summary>
    public static class FarmAnimalEvents
    {
        /// <summary>
        /// Triggers when an animal eats grass
        /// </summary>
        public static event EventHandler<EventArgsOnAnimalEatGrass> OnBeforeEatGrass = delegate { };

        /// <summary>
        /// Triggers when an animal makes sound
        /// </summary>
        public static event EventHandler<CancelEventArgs> OnMakeSound = delegate { };

        /// <summary>
        /// Triggers when an animal is pushed by a player
        /// </summary>
        public static event EventHandler<CancelEventArgs> OnFarmerPushing = delegate { };
        
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
