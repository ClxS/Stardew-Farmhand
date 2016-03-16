using Revolution.Events.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    public static class FarmAnimalEvents
    {
        public static EventHandler<EventArgsOnAnimalEatGrass> OnEatGrass = delegate { };
        public static EventHandler OnMakeSound = delegate { };
        public static EventHandler OnFarmerPushing = delegate { };

        public static void InvokeOnEatGrass()
        {
            OnEatGrass.Invoke(null, new EventArgsOnAnimalEatGrass());
        }

        public static void InvokeOnMakeSound()
        {
            OnMakeSound.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeOnFarmerPushing()
        {
            OnFarmerPushing.Invoke(null, EventArgs.Empty);
        }
    }
}
