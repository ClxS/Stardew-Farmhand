using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Tools;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace StardewModdingAPI.Inheritance
{
    /// <summary>
    /// The 'SGame' class.
    /// This summary, and many others, only exists because XML doc tags.
    /// </summary>
    public class SGame
    {
        /// <summary>
        /// Useless right now.
        /// </summary>
        public const int LowestModItemID = 1000;
        
        internal SGame()
        {
            Instance = this;
        }

        /// <summary>
        /// Useless at this time.
        /// </summary>
        [Obsolete]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public static Dictionary<int, SObject> ModItems { get; private set; }
        
        /// <summary>
        /// The current RenderTarget in Game1 (Private field, uses reflection)
        /// </summary>
        public RenderTarget2D Screen
        {
            get { return typeof (Game1).GetBaseFieldValue<RenderTarget2D>(Game1.game1, "screen"); }
            set { typeof (Game1).SetBaseFieldValue<RenderTarget2D>(this, "screen", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ThumbstickMotionMargin
        {
            get { return (int)typeof(Game1).GetBaseFieldValue<object>(Game1.game1, "thumbstickMotionMargin"); }
            set { typeof(Game1).SetBaseFieldValue<object>(this, "thumbstickMotionMargin", value); }
        }

        /// <summary>
        /// The current Colour in Game1 (Private field, uses reflection)
        /// </summary>
        public Color BgColour
        {
            get { return (Color)typeof(Game1).GetBaseFieldValue<object>(Game1.game1, "bgColor"); }
            set { typeof(Game1).SetBaseFieldValue<object>(this, "bgColor", value); }
        }

        /// <summary>
        /// Static accessor for an Instance of the class SGame
        /// </summary>
        public static SGame Instance { get; private set; }

        /// <summary>
        /// The game's FPS. Re-determined every Draw update.
        /// </summary>
        public static float FramesPerSecond { get; private set; }

        /// <summary>
        /// Whether or not we're in a pseudo 'debug' mode. Mostly for displaying information like FPS.
        /// </summary>
        public static bool Debug { get; private set; }
        internal static Queue<String> DebugMessageQueue { get; private set; }

        /// <summary>
        /// The current player (equal to Farmer.Player)
        /// </summary>
        [Obsolete("Use Farmer.Player instead")]
        public Farmer CurrentFarmer => Game1.player;

        public GraphicsDevice GraphicsDevice => Game1.game1.GraphicsDevice;
        public GameComponentCollection Components => Game1.game1.Components;
        public ContentManager Content => Game1.game1.Content;
        public TimeSpan InactiveSleepTime => Game1.game1.InactiveSleepTime;
        public bool IsActive => Game1.game1.IsActive;
        public bool IsFixedTimeStep => Game1.game1.IsFixedTimeStep;
        public bool IsMouseVisible => Game1.game1.IsMouseVisible;
        public LaunchParameters LaunchParameters => Game1.game1.LaunchParameters;
        public GameServiceContainer Services => Game1.game1.Services;
        public TimeSpan TargetElapsedTime => Game1.game1.TargetElapsedTime;
        public GameWindow Window => Game1.game1.Window;

        /// <summary>
        /// Gets ALL static fields that belong to 'Game1'
        /// </summary>
        public static FieldInfo[] GetStaticFields => typeof (Game1).GetFields();
        
        /// <summary>
        /// Whether or not the game's zoom level is 1.0f
        /// </summary>
        public bool ZoomLevelIsOne => Game1.options.zoomLevel.Equals(1.0f);
        
        [Obsolete("Do not use at this time.")]
        // ReSharper disable once UnusedMember.Local
        private static int RegisterModItem(SObject modItem)
        {
            if (modItem.HasBeenRegistered)
            {
                Log.AsyncR($"The item {modItem.Name} has already been registered with ID {modItem.RegisteredId}");
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

        [Obsolete("Do not use at this time.")]
        // ReSharper disable once UnusedMember.Local
        private static SObject PullModItemFromDict(int id, bool isIndex)
        {
            if (isIndex)
            {
                if (ModItems.ElementAtOrDefault(id).Value != null)
                {
                    return ModItems.ElementAt(id).Value.Clone();
                }
                Log.AsyncR("ModItem Dictionary does not contain index: " + id);
                return null;
            }
            if (ModItems.ContainsKey(id))
            {
                return ModItems[id].Clone();
            }
            Log.AsyncR("ModItem Dictionary does not contain ID: " + id);
            return null;
        }
        
        /// <summary>
        /// Invokes a private, non-static method in Game1 via Reflection
        /// </summary>
        /// <param name="name">The name of the method</param>
        /// <param name="parameters">Any parameters needed</param>
        /// <returns>Whatever the method normally returns. Null if void.</returns>
        public static object InvokeBasePrivateInstancedMethod(string name, params object[] parameters)
        {
            try
            {
                return typeof (Game1).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Game1.game1, parameters);
            }
            catch
            {
                Log.AsyncR("Failed to call base method: " + name);
                return null;
            }
        }

        /// <summary>
        /// Queue's a message to be drawn in Debug mode (F3)
        /// </summary>
        /// <returns></returns>
        public static bool QueueDebugMessage(string message)
        {
            if (!Debug)
                return false;

            if (DebugMessageQueue.Count > 32)
                return false;

            DebugMessageQueue.Enqueue(message);
            return true;
        }
    }
}