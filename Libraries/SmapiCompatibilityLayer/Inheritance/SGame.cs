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

        private Game1 GameInstance { get; set; }
        
        internal SGame(Game1 inst)
        {
            GameInstance = inst;
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
            get { return typeof (Game1).GetBaseFieldValue<RenderTarget2D>(GameInstance, "screen"); }
            set { typeof (Game1).SetBaseFieldValue<RenderTarget2D>(GameInstance, "screen", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ThumbstickMotionMargin
        {
            get { return (int)typeof(Game1).GetBaseFieldValue<object>(GameInstance, "thumbstickMotionMargin"); }
            set { typeof(Game1).SetBaseFieldValue<object>(GameInstance, "thumbstickMotionMargin", value); }
        }

        /// <summary>
        /// The current Colour in Game1 (Private field, uses reflection)
        /// </summary>
        public Color BgColour
        {
            get { return (Color)typeof(Game1).GetBaseFieldValue<object>(GameInstance, "bgColor"); }
            set { typeof(Game1).SetBaseFieldValue<object>(GameInstance, "bgColor", value); }
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

        public GraphicsDevice GraphicsDevice => GameInstance.GraphicsDevice;

        public GameComponentCollection Components => GameInstance.Components;

        public ContentManager Content => GameInstance.Content;

        public TimeSpan InactiveSleepTime => GameInstance.InactiveSleepTime;
        public bool IsActive => GameInstance.IsActive;
        public bool IsFixedTimeStep => GameInstance.IsFixedTimeStep;
        public bool IsMouseVisible => GameInstance.IsMouseVisible;
        public LaunchParameters LaunchParameters => GameInstance.LaunchParameters;
        public GameServiceContainer Services => GameInstance.Services;
        public TimeSpan TargetElapsedTime => GameInstance.TargetElapsedTime;
        public GameWindow Window => GameInstance.Window;

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