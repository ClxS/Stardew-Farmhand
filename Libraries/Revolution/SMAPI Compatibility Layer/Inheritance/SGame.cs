using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using static StardewModdingAPI.Events.GameEvents;
using Cat = StardewValley.Characters.Cat;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Inheritance
{
    [Obsolete("SMAPI methods are obsolete. These will not work in Revolution mods")]
    public class SGame : Game1
    {
        public static List<SGameLocation> ModLocations = new List<SGameLocation>();
        public static SGameLocation CurrentLocation { get; internal set; }
        public static Dictionary<int, SObject> ModItems { get; private set; }
        public const int LowestModItemID = 1000;

        public static FieldInfo[] StaticFields => GetStaticFields();

        public static FieldInfo[] GetStaticFields()
        {
            return typeof(Game1).GetFields();
        }
        
        public int PreviousGameLocations { get; private set; }
        public int PreviousLocationObjects { get; private set; }
        public int PreviousItems_ { get; private set; }
        public Dictionary<Item, int> PreviousItems { get; private set; }

        public int PreviousCombatLevel { get; private set; }
        public int PreviousFarmingLevel { get; private set; }
        public int PreviousFishingLevel { get; private set; }
        public int PreviousForagingLevel { get; private set; }
        public int PreviousMiningLevel { get; private set; }
        public int PreviousLuckLevel { get; private set; }

        public GameLocation PreviousGameLocation { get; private set; }
        public IClickableMenu PreviousActiveMenu { get; private set; }
        
        public Farmer PreviousFarmer { get; private set; }

        public static SGame Instance { get; private set; }

        public Farmer CurrentFarmer => player;

        public SGame(int previousItems)
        {
            PreviousItems_ = previousItems;
            Instance = this;
        }
        
        protected override void Initialize()
        {
            ModItems = new Dictionary<Int32, SObject>();
            base.Initialize();
        }
        
        [Obsolete]
        public static int RegisterModItem(SObject modItem)
        {
            if (modItem.HasBeenRegistered)
            {
                Log.Error($"The item {modItem.Name} has already been registered with ID {modItem.RegisteredId}");
                return modItem.RegisteredId;
            }
            var newId = LowestModItemID;
            if (ModItems.Count > 0)
                newId = Math.Max(LowestModItemID, ModItems.OrderBy(x => x.Key).First().Key + 1);
            ModItems.Add(newId, modItem);
            modItem.HasBeenRegistered = true;
            modItem.RegisteredId = newId;
            return newId;
        }

        [Obsolete]
        public static SObject PullModItemFromDict(Int32 id, bool isIndex)
        {
            if (isIndex)
            {
                if (ModItems.ElementAtOrDefault(id).Value != null)
                {
                    return ModItems.ElementAt(id).Value.Clone();
                }
                Log.Error("ModItem Dictionary does not contain index: " + id);
                return null;
            }
            if (ModItems.ContainsKey(id))
            {
                return ModItems[id].Clone();
            }
            Log.Error("ModItem Dictionary does not contain ID: " + id);
            return null;
        }

        [Obsolete]
        public static SGameLocation GetLocationFromName(String name)
        {
            return ModLocations.FirstOrDefault(n => n.name == name);
        }

        [Obsolete]
        public static SGameLocation LoadOrCreateSGameLocationFromName(String name)
        {
            if (GetLocationFromName(name) != null)
                return GetLocationFromName(name);
            var gl = locations.FirstOrDefault(x => x.name == name);
            if (gl != null)
            {
                Log.Debug("A custom location was created for the new name: " + name);
                var s = SGameLocation.ConstructFromBaseClass(gl);
                ModLocations.Add(s);
                return s;
            }
            if (currentLocation != null && currentLocation.name == name)
            {
                gl = currentLocation;
                Log.Debug("A custom location was created from the current location for the new name: " + name);
                var s = SGameLocation.ConstructFromBaseClass(gl);
                ModLocations.Add(s);
                return s;
            }

            Log.Debug("A custom location could not be created for: " + name);
            return null;
        }

        public void UpdateEventCalls()
        {
        }
    }
}
