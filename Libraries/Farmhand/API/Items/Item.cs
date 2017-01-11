using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand.API.Utilities;
using Farmhand.Registries;
using StardewValley;
using Farmhand.Logging;
using Farmhand.Attributes;

namespace Farmhand.API.Items
{
    /// <summary>
    /// Provides functions relating to items
    /// </summary>
    public static class Item
    {
        private static List<StardewValley.Object> deserializedObjects = new List<StardewValley.Object>();

        public static List<ItemInformation> Items { get; } = new List<ItemInformation>();
        public static Dictionary<Type, ItemInformation> RegisteredTypeInformation { get; } = new Dictionary<Type, ItemInformation>();

        /// <summary>
        /// Registers a new item
        /// </summary>
        /// <param name="item">Information of item to register</param>
        public static void RegisterItem<T>(ItemInformation item)
        {
            if (Game1.objectSpriteSheet == null)
            {
                throw new Exception("objectInformation is null! This likely occurs if you try to register an item before AfterContentLoaded");
            }
            
            item.Id = IdManager.AssignNewIdSequential(Game1.objectInformation); 
            Items.Add(item);
            RegisteredTypeInformation[typeof(T)] = item;
            TextureUtility.AddSpriteToSpritesheet(ref Game1.objectSpriteSheet, TextureRegistry.GetItem(item.Texture)?.Texture, item.Id, 16, 16);
            Game1.objectInformation[item.Id] = item.ToString();
        }

        // Uses the registered deserialized objects list to fix all the IDs
        internal static void FixupItemIds(object sender, System.EventArgs e)
        {
            foreach(StardewValley.Object deserializedObject in deserializedObjects)
            {
                var type = deserializedObject.GetType();
                int expectedId = -1;

                if (RegisteredTypeInformation.ContainsKey(type))
                {
                    expectedId = RegisteredTypeInformation[type].Id;

                    if (deserializedObject.parentSheetIndex != expectedId)
                    {
                        Log.Warning($"Correcting id mismatch - {type.Name} - {deserializedObject.parentSheetIndex} != {expectedId}");
                        deserializedObject.parentSheetIndex = expectedId;
                    }
                }
            }
        }

        // Is called from the default constructor of StardewValley.Object, and alerts this item registry to fix its ID after loading is finished
        [Hook(HookType.Exit, "StardewValley.Object", "System.Void StardewValley.Object::.ctor()")]
        internal static void RegisterDeserializingObject([ThisBind] object @this)
        {
            if (@this is StardewValley.Object)
            {
                deserializedObjects.Add(@this as StardewValley.Object);
            }
        }
    }
}
