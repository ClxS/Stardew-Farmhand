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
    public class SGame : Farmhand.Overrides.Game1
    {
        /// <summary>
        /// Useless right now.
        /// </summary>
        public const int LowestModItemID = 1000;
        
        /// <summary>
        /// Gets a jagged array of all buttons pressed on the gamepad the prior frame.
        /// </summary>
        public Buttons[][] PreviouslyPressedButtons;

        public SGame()
        {
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Farmhand.Logging.Log.Success("Using SMAPI game override");
            Instance = this;
            FirstUpdate = true;
        }

        /// <summary>
        /// Useless at this time.
        /// </summary>
        [Obsolete]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public static Dictionary<int, SObject> ModItems { get; private set; }

        /// <summary>
        /// The current KeyboardState
        /// </summary>
        public KeyboardState KStateNow { get; private set; } = Keyboard.GetState();
        /// <summary>
        /// The prior KeyboardState
        /// </summary>
        public KeyboardState KStatePrior { get; private set; } = Keyboard.GetState();

        /// <summary>
        /// The current MouseState
        /// </summary>
        public MouseState MStateNow { get; private set; } = Mouse.GetState();

        /// <summary>
        /// The prior MouseState
        /// </summary>
        public MouseState MStatePrior { get; private set; } = Mouse.GetState();

        /// <summary>
        /// All keys pressed on the current frame
        /// </summary>
        public Keys[] CurrentlyPressedKeys => KStateNow.GetPressedKeys();

        /// <summary>
        /// All keys pressed on the prior frame
        /// </summary>
        public Keys[] PreviouslyPressedKeys => KStatePrior.GetPressedKeys();

        /// <summary>
        /// All keys pressed on this frame except for the ones pressed on the prior frame
        /// </summary>
        public Keys[] FramePressedKeys => CurrentlyPressedKeys.Except(PreviouslyPressedKeys).ToArray();

        /// <summary>
        /// All keys pressed on the prior frame except for the ones pressed on the current frame
        /// </summary>
        public Keys[] FrameReleasedKeys => PreviouslyPressedKeys.Except(CurrentlyPressedKeys).ToArray();

        /// <summary>
        /// Whether or not a save was tagged as 'Loaded' the prior frame.
        /// </summary>
        public bool PreviouslyLoadedGame { get; private set; }

        /// <summary>
        /// The list of GameLocations on the prior frame
        /// </summary>
        public int PreviousGameLocations { get; private set; }

        /// <summary>
        /// The list of GameObjects on the prior frame
        /// </summary>
        public int PreviousLocationObjects { get; private set; }

        /// <summary>
        /// The list of Items in the player's inventory on the prior frame
        /// </summary>
        public Dictionary<Item, int> PreviousItems { get; private set; }

        /// <summary>
        /// The player's Combat level on the prior frame
        /// </summary>
        public int PreviousCombatLevel { get; private set; }
        /// <summary>
        /// The player's Farming level on the prior frame
        /// </summary>
        public int PreviousFarmingLevel { get; private set; }
        /// <summary>
        /// The player's Fishing level on the prior frame
        /// </summary>
        public int PreviousFishingLevel { get; private set; }
        /// <summary>
        /// The player's Foraging level on the prior frame
        /// </summary>
        public int PreviousForagingLevel { get; private set; }
        /// <summary>
        /// The player's Mining level on the prior frame
        /// </summary>
        public int PreviousMiningLevel { get; private set; }
        /// <summary>
        /// The player's Luck level on the prior frame
        /// </summary>
        public int PreviousLuckLevel { get; private set; }
        
        /// <summary>
        /// The player's previous game location
        /// </summary>
        public GameLocation PreviousGameLocation { get; private set; }

        /// <summary>
        /// The previous ActiveGameMenu in Game1
        /// </summary>
        public IClickableMenu PreviousActiveMenu { get; private set; }
        
        /// <summary>
        /// The previous mine level
        /// </summary>
        public int PreviousMineLevel { get; private set; }

        /// <summary>
        /// The previous TimeOfDay (Int32 between 600 and 2400?)
        /// </summary>
        public int PreviousTimeOfDay { get; private set; }

        /// <summary>
        /// The previous DayOfMonth (Int32 between 1 and 28?)
        /// </summary>
        public int PreviousDayOfMonth { get; private set; }

        /// <summary>
        /// The previous Season (String as follows: "winter", "spring", "summer", "fall")
        /// </summary>
        public string PreviousSeasonOfYear { get; private set; }

        /// <summary>
        /// The previous Year
        /// </summary>
        public int PreviousYearOfGame { get; private set; }

        /// <summary>
        /// The previous result of Game1.newDay
        /// </summary>
        public bool PreviousIsNewDay { get; private set; }

        /// <summary>
        /// The previous 'Farmer' (Player)
        /// </summary>
        public Farmer PreviousFarmer { get; private set; }

        /// <summary>
        /// The current index of the update tick. Recycles every 60th tick to 0. (Int32 between 0 and 59)
        /// </summary>
        public int CurrentUpdateTick { get; private set; }

        /// <summary>
        /// Whether or not this update frame is the very first of the entire game
        /// </summary>
        public bool FirstUpdate { get; private set; }

        /// <summary>
        /// The current RenderTarget in Game1 (Private field, uses reflection)
        /// </summary>
        public RenderTarget2D Screen
        {
            get { return this.screen; }
            set { this.screen = value; }
        }

        /// <summary>
        /// The current Colour in Game1 (Private field, uses reflection)
        /// </summary>
        public Color BgColour
        {
            get { return this.bgColor; }
            set { this.bgColor = value; }
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
        internal static Queue<string> DebugMessageQueue { get; private set; }

        /// <summary>
        /// The current player (equal to Farmer.Player)
        /// </summary>
        [Obsolete("Use Farmer.Player instead")]
        public Farmer CurrentFarmer => player;

        /// <summary>
        /// Gets ALL static fields that belong to 'Game1'
        /// </summary>
        public static FieldInfo[] GetStaticFields => typeof(Game1).GetFields();
        
        /// <summary>
        /// Gets an array of all Buttons pressed on a joystick
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Buttons[] GetButtonsDown(PlayerIndex index)
        {
            return Farmhand.Events.EventManager.Watcher.GetButtonsDown(index);
        }

        /// <summary>
        /// Gets all buttons that were pressed on the current frame of a joystick
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Buttons[] GetFramePressedButtons(PlayerIndex index)
        {
            return Farmhand.Events.EventManager.Watcher.GetFramePressedButtons(index);
        }

        /// <summary>
        /// Gets all buttons that were released on the current frame of a joystick
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Buttons[] GetFrameReleasedButtons(PlayerIndex index)
        {
            return Farmhand.Events.EventManager.Watcher.GetFrameReleasedButtons(index);
        }

        /// <summary>
        /// 
        /// </summary>
        public static MethodInfo DrawFarmBuildings = typeof(Game1).GetMethod("drawFarmBuildings", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// 
        /// </summary>
        public static MethodInfo DrawHUD = typeof(Game1).GetMethod("drawHUD", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// 
        /// </summary>
        public static MethodInfo DrawDialogueBox = typeof(Game1).GetMethod("drawDialogueBox", BindingFlags.NonPublic | BindingFlags.Instance);

        public static MethodInfo CheckForEscapeKeys = typeof(Game1).GetMethod("checkForEscapeKeys", BindingFlags.NonPublic | BindingFlags.Instance);
        
        /// <summary>
        /// Whether or not the game's zoom level is 1.0f
        /// </summary>
        public bool ZoomLevelIsOne => options.zoomLevel.Equals(1.0f);
        
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