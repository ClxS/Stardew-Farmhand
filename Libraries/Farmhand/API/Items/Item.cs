using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand.API.Utilities;
using Farmhand.Registries;
using StardewValley;
using Farmhand.Logging;

namespace Farmhand.API.Items
{
    /// <summary>
    /// Provides functions relating to items
    /// </summary>
    public static class Item
    {
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

        internal static void FixupItemIds(object sender, System.EventArgs e)
        {
            if (Game1.player != null)
            {
                if (Game1.player.items == null)
                    Log.Error("Game1.player.items is null");

                if (RegisteredTypeInformation == null)
                    Log.Error("RegisteredTypeInformation is null");

                foreach (var item in Game1.player.items.Where(n => n != null && RegisteredTypeInformation.ContainsKey(n.GetType())))
                {
                    var obj = item as StardewValley.Object;
                    if (obj != null)
                    {
                        var type = item.GetType();
                        var info = RegisteredTypeInformation[type];
                        if (obj.parentSheetIndex != info.Id)
                        {
                            Log.Error($"Correcting item mismatch - {info.Name} - {obj.parentSheetIndex} != {info.Id}");
                            obj.parentSheetIndex = info.Id;
                        }
                    }
                }
            }
        }
    }
}
