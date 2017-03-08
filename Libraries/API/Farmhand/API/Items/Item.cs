namespace Farmhand.API.Items
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Utilities;
    using Farmhand.Attributes;
    using Farmhand.Logging;
    using Farmhand.Registries;

    using StardewValley;

    using Object = StardewValley.Object;

    /// <summary>
    ///     Provides functions relating to items
    /// </summary>
    public static class Item
    {
        private static readonly List<Object> DeserializedObjects = new List<Object>();

        /// <summary>
        ///     Gets the list of registered items.
        /// </summary>
        public static List<ItemInformation> Items { get; } = new List<ItemInformation>();

        /// <summary>
        ///     Gets the type information pairing for registered items.
        /// </summary>
        public static Dictionary<Type, ItemInformation> RegisteredTypeInformation { get; } =
            new Dictionary<Type, ItemInformation>();

        /// <summary>
        ///     Registers a new item
        /// </summary>
        /// <typeparam name="T">
        ///     The type used to instantiate this item.
        /// </typeparam>
        /// <param name="item">
        ///     Information of item to register
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if item is null.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if Game1.objectSpriteSheet is null.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Thrown if item.Texture could not be found in <see cref="TextureRegistry"/> or could not be loaded.
        /// </exception>
        public static void RegisterItem<T>(ItemInformation item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));    
            }

            if (Game1.objectSpriteSheet == null)
            {
                throw new Exception(
                    "objectInformation is null! This likely occurs if you try to register an item before AfterContentLoaded");
            }

            var texture = TextureRegistry.GetItem(item.Texture)?.Texture;
            if (texture == null)
            {
                throw new NullReferenceException($"Texture ({item.Texture}) could not be located in registry.");
            }

            item.Id = IdManager.AssignNewIdSequential(Game1.objectInformation);
            Items.Add(item);
            RegisteredTypeInformation[typeof(T)] = item;
            TextureUtility.AddSpriteToSpritesheet(
                ref Game1.objectSpriteSheet,
                texture,
                item.Id,
                16,
                16);
            Game1.objectInformation[item.Id] = item.ToString();
        }

        // Uses the registered de-serialized objects list to fix all the IDs
        internal static void FixupItemIds(object sender, EventArgs e)
        {
            foreach (var deserializedObject in DeserializedObjects)
            {
                var type = deserializedObject.GetType();

                if (RegisteredTypeInformation.ContainsKey(type))
                {
                    var expectedId = RegisteredTypeInformation[type].Id;

                    if (deserializedObject.parentSheetIndex != expectedId)
                    {
                        Log.Warning(
                            $"Correcting id mismatch - {type.Name} - {deserializedObject.parentSheetIndex} != {expectedId}");
                        deserializedObject.parentSheetIndex = expectedId;
                    }
                }
            }
        }

        // Is called from the default constructor of StardewValley.Object, and alerts this item registry to fix its ID after loading is finished
        [Hook(HookType.Exit, "StardewValley.Object", "System.Void StardewValley.Object::.ctor()")]
        internal static void RegisterDeserializingObject([ThisBind] object @this)
        {
            var o = @this as Object;
            if (o != null)
            {
                DeserializedObjects.Add(o);
            }
        }
    }
}