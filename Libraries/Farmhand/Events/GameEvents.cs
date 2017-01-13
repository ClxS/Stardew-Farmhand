namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.GameEvents;

    using Microsoft.Xna.Framework;

    /// <summary>
    ///     Contains events relating to the main game state
    /// </summary>
    public static class GameEvents
    {
        /// <summary>
        ///     Fires just before the game is initialised.
        /// </summary>
        public static event EventHandler<GameInitialiseEventArgs> BeforeGameInitialised = delegate { };

        /// <summary>
        ///     Fires just after the game is initialised.
        /// </summary>
        public static event EventHandler<GameInitialisedEventArgs> AfterGameInitialised = delegate { };

        /// <summary>
        ///     Fires just before the game loads it's initial content.
        /// </summary>
        public static event EventHandler BeforeLoadContent = delegate { };

        /// <summary>
        ///     Fires just after the game loads it's initial content.
        /// </summary>
        public static event EventHandler AfterLoadedContent = delegate { };

        /// <summary>
        ///     Fires just before the game unloads it's content.
        /// </summary>
        public static event EventHandler BeforeUnloadContent = delegate { };

        /// <summary>
        ///     Fires just after the game unloads it's content.
        /// </summary>
        public static event EventHandler AfterUnloadedContent = delegate { };

        /// <summary>
        ///     Fires just before executing the games update tick.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to prevent the game from being able to update it's own state.
        ///     You should only use this if you know what you are doing.
        /// </remarks>
        public static event EventHandler<BeforeGameUpdateEventArgs> BeforeUpdateTick = delegate { };

        /// <summary>
        ///     Fires just after executing the games update tick.
        /// </summary>
        public static event EventHandler AfterUpdateTick = delegate { };

        /// <summary>
        ///     Fires just after a game is loaded.
        /// </summary>
        /// <remarks>
        ///     This event is fired at the end of Game1::loadForNewGame
        /// </remarks>
        public static event EventHandler<AfterGameLoadedEventArgs> AfterGameLoaded = delegate { };

        /// <summary>
        ///     Fires every 0.5 seconds.
        /// </summary>
        /// <remarks>
        ///     This event is managed by the property watcher, so will fire at the
        ///     beginning of Game1.Update in the frame after the event occurs.
        /// </remarks>
        public static event EventHandler HalfSecondTick = delegate { };

        [Hook(HookType.Entry, "StardewValley.Game1", "Initialize")]
        internal static void OnBeforeGameInitialise([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(BeforeGameInitialised, @this, new GameInitialiseEventArgs());
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "Initialize")]
        internal static void OnAfterGameInitialise([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(AfterGameInitialised, @this, new GameInitialisedEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "LoadContent")]
        internal static void OnBeforeLoadContent([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(BeforeLoadContent, @this);
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "LoadContent")]
        internal static void OnAfterLoadedContent([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(AfterLoadedContent, @this);
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "UnloadContent")]
        internal static void OnBeforeUnloadContent([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(BeforeUnloadContent, @this);
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "UnloadContent")]
        internal static void OnAfterUnloadedContent([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(AfterUnloadedContent, @this);
        }

        // Invoked by property watcher
        internal static void OnHalfSecondTick()
        {
            EventCommon.SafeInvoke(HalfSecondTick, null);
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Update")]
        internal static bool OnBeforeUpdate(
            [ThisBind] object @this,
            [InputBind(typeof(GameTime), "gameTime")] GameTime gt)
        {
            return EventCommon.SafeCancellableInvoke(BeforeUpdateTick, @this, new BeforeGameUpdateEventArgs(gt));
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "Update")]
        internal static void OnAfterUpdate([ThisBind] object @this)
        {
            TimeEvents.DidShouldTimePassCheckThisFrame = false;
            EventCommon.SafeInvoke(AfterUpdateTick, @this);
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "loadForNewGame")]
        internal static void OnAfterGameLoaded([InputBind(typeof(bool), "loadedGame")] bool loadedGame)
        {
            EventCommon.SafeInvoke(AfterGameLoaded, null, new AfterGameLoadedEventArgs(loadedGame));
        }
    }
}