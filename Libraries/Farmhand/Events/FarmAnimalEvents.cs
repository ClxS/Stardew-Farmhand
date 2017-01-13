namespace Farmhand.Events
{
    using System;
    using System.ComponentModel;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments;
    using Farmhand.Events.Arguments.AnimalEvents;

    /// <summary>
    ///     Contains events relating to farm animals
    /// </summary>
    public static class FarmAnimalEvents
    {
        /// <summary>
        ///     Fires when an animal eats grass.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to prevent the animal eating grass.
        /// </remarks>
        public static event EventHandler<AnimalEatGrassEventArgs> BeforeEatGrass = delegate { };

        /// <summary>
        ///     Fires when an animal makes sound.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to prevent the animal making sound.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> MakeSound = delegate { };

        /// <summary>
        ///     Fires when an animal is pushed by a player.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to prevent the farmer pushing an animal.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> FarmerPushing = delegate { };

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "eatGrass")]
        internal static bool OnBeforeEatGrass([ThisBind] object @this)
        {
            return EventCommon.SafeCancellableInvoke(BeforeEatGrass, @this, new AnimalEatGrassEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "makeSound")]
        internal static bool OnMakeSound([ThisBind] object @this)
        {
            return EventCommon.SafeCancellableInvoke(MakeSound, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.FarmAnimal", "farmerPushing")]
        internal static bool OnFarmerPushing([ThisBind] object @this)
        {
            return EventCommon.SafeCancellableInvoke(FarmerPushing, @this, new CancelEventArgs());
        }
    }
}