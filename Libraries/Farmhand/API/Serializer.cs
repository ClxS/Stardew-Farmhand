using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Farmhand.Attributes;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;

namespace Farmhand.API
{
    public class Serializer
    {
        #region Default Serialiser Types
        //TODO See whether this could be automatically set by the installer for future proofing.
        private static Type[] _serialiserTypes = new Type[27]
        {
            typeof (Tool), typeof (GameLocation), typeof (Crow), typeof (Duggy), typeof (Bug), typeof (BigSlime),
            typeof (Fireball), typeof (Ghost), typeof (Child), typeof (Pet), typeof (Dog),
            typeof (StardewValley.Characters.Cat),
            typeof (Horse), typeof (GreenSlime), typeof (LavaCrab), typeof (RockCrab), typeof (ShadowGuy),
            typeof (SkeletonMage),
            typeof (SquidKid), typeof (Grub), typeof (Fly), typeof (DustSpirit), typeof (Quest), typeof (MetalHead),
            typeof (ShadowGirl),
            typeof (Monster), typeof (TerrainFeature)
        };

        private static Type[] _farmerTypes = new Type[1] {
            typeof (Tool)
        };

        private static Type[] _locationTypes = new Type[26]
        {
            typeof (Tool), typeof (Crow), typeof (Duggy), typeof (Fireball), typeof (Ghost),
            typeof (GreenSlime), typeof (LavaCrab), typeof (RockCrab), typeof (ShadowGuy), typeof (SkeletonWarrior),
            typeof (Child), typeof (Pet), typeof (Dog), typeof (StardewValley.Characters.Cat), typeof (Horse),
            typeof (SquidKid),
            typeof (Grub), typeof (Fly), typeof (DustSpirit), typeof (Bug), typeof (BigSlime),
            typeof (BreakableContainer),
            typeof (MetalHead), typeof (ShadowGirl), typeof (Monster), typeof (TerrainFeature)
        };
      
        #endregion

        private static bool _injected = false;

        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void AttachListeners()
        {
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Initialize")]
        internal static void InjectSerializers()
        {
            var typeArray = InjectedTypes.ToArray();
            SaveGame.serializer = new XmlSerializer(typeof(SaveGame), _serialiserTypes.Concat(InjectedTypes).ToArray());
            SaveGame.farmerSerializer = new XmlSerializer(typeof(Farmer), _farmerTypes.Concat(InjectedTypes).ToArray());
            SaveGame.locationSerializer = new XmlSerializer(typeof(GameLocation), _locationTypes.Concat(InjectedTypes).ToArray());
            _injected = true;
        }

        public static readonly List<Type> InjectedTypes = new List<Type>();

        public static void RegisterType<T>()
        {
            if(_injected)
                Logging.Log.Error("RegisterType<T> called too late, must be called prior to Game1.Initialize or it will likely not be added to the serialiser");

            if (!typeof (T).IsPublic)
            {
                Logging.Log.Error("ERROR: Types added to RegisterType<T> must be set as public");
            }
            else
            {
                var type = typeof(T);
                if (!InjectedTypes.Contains(type))
                    InjectedTypes.Add(type);
            }

            
        }
        
    }
}
