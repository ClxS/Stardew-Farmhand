namespace Farmhand.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    using Farmhand.Attributes;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GameEvents;
    using Farmhand.Logging;

    using StardewValley;
    using StardewValley.Characters;
    using StardewValley.Monsters;
    using StardewValley.Objects;
    using StardewValley.Quests;
    using StardewValley.TerrainFeatures;

    using Cat = StardewValley.Characters.Cat;

    /// <summary>
    ///     Providers override serializer functionality.
    /// </summary>
    public class Serializer
    {
        private static XmlSerializer injectedSerializer;

        private static XmlSerializer injectedFarmerSerializer;

        private static XmlSerializer injectedLocationSerializer;

        private static bool injected;

        /// <summary>
        ///     Gets the injected types.
        /// </summary>
        public static List<Type> InjectedTypes { get; } = new List<Type>();

        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void AttachListeners()
        {
            GameEvents.OnBeforeUpdateTick += GameEvents_OnBeforeUpdateTick;
        }

        private static void GameEvents_OnBeforeUpdateTick(object sender, EventArgsOnBeforeGameUpdate e)
        {
            if (!injected)
            {
                return;
            }

            SaveGame.serializer = injectedSerializer;
            SaveGame.farmerSerializer = injectedFarmerSerializer;
            SaveGame.locationSerializer = injectedLocationSerializer;
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Initialize")]
        internal static void InjectSerializers()
        {
            injectedSerializer = new XmlSerializer(typeof(SaveGame), SerialiserTypes.Concat(InjectedTypes).ToArray());
            injectedFarmerSerializer = new XmlSerializer(typeof(Farmer), FarmerTypes.Concat(InjectedTypes).ToArray());
            injectedLocationSerializer = new XmlSerializer(
                typeof(GameLocation),
                LocationTypes.Concat(InjectedTypes).ToArray());
            SaveGame.serializer = injectedSerializer;
            SaveGame.farmerSerializer = injectedFarmerSerializer;
            SaveGame.locationSerializer = injectedLocationSerializer;
            injected = true;
        }

        /// <summary>
        ///     Registers a type for serialization.
        /// </summary>
        /// <typeparam name="T">
        ///     The type to register.
        /// </typeparam>
        public static void RegisterType<T>()
        {
            if (injected)
            {
                Log.Error(
                    "RegisterType<T> called too late, must be called prior to Game1.Initialize or it will likely not be added to the serialiser");
            }

            if (!typeof(T).IsPublic)
            {
                Log.Error("ERROR: Types added to RegisterType<T> must be set as public");
            }
            else
            {
                var type = typeof(T);
                if (!InjectedTypes.Contains(type))
                {
                    InjectedTypes.Add(type);
                }
            }
        }

        #region Default Serialiser Types

        // TODO See whether this could be automatically set by the installer for future proofing.
        private static readonly Type[] SerialiserTypes =
            {
                typeof(Tool), typeof(GameLocation), typeof(Crow),
                typeof(Duggy), typeof(Bug), typeof(BigSlime),
                typeof(Fireball), typeof(Ghost), typeof(Child), typeof(Pet),
                typeof(Dog), typeof(Cat), typeof(Horse), typeof(GreenSlime),
                typeof(LavaCrab), typeof(RockCrab), typeof(ShadowGuy),
                typeof(SkeletonMage), typeof(SquidKid), typeof(Grub),
                typeof(Fly), typeof(DustSpirit), typeof(Quest),
                typeof(MetalHead), typeof(ShadowGirl), typeof(Monster),
                typeof(TerrainFeature)
            };

        private static readonly Type[] FarmerTypes = { typeof(Tool) };

        private static readonly Type[] LocationTypes =
            {
                typeof(Tool), typeof(Crow), typeof(Duggy), typeof(Fireball),
                typeof(Ghost), typeof(GreenSlime), typeof(LavaCrab),
                typeof(RockCrab), typeof(ShadowGuy), typeof(SkeletonWarrior),
                typeof(Child), typeof(Pet), typeof(Dog), typeof(Cat),
                typeof(Horse), typeof(SquidKid), typeof(Grub), typeof(Fly),
                typeof(DustSpirit), typeof(Bug), typeof(BigSlime),
                typeof(BreakableContainer), typeof(MetalHead),
                typeof(ShadowGirl), typeof(Monster), typeof(TerrainFeature)
            };

        #endregion
    }
}