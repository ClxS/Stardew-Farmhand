using Revolution.Attributes;
using Revolution.Events.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            OnBeforeEatGrass.Invoke(null, new EventArgsOnAnimalEatGrass());
        }

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "makeSound")]
        internal static void InvokeOnMakeSound()
        {
            OnMakeSound.Invoke(null, EventArgs.Empty);
        }

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "farmerPushing")]
        internal static void InvokeOnFarmerPushing()
        {
            OnFarmerPushing.Invoke(null, EventArgs.Empty);
        }
    }
}
