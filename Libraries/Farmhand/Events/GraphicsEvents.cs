namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.GraphicsEvents;

    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     Contains events relating to graphics
    /// </summary>
    public static class GraphicsEvents
    {
        /// <summary>
        ///     Fires when the game's window resizes.
        /// </summary>
        public static event EventHandler<EventArgsClientSizeChanged> Resize = delegate { };

        /// <summary>
        ///     Fires just before the main Draw method.
        /// </summary>
        public static event EventHandler BeforeDraw = delegate { };

        /// <summary>
        ///     First just after the main Draw method.
        /// </summary>
        public static event EventHandler AfterDraw = delegate { };

        /// <summary>
        ///     Fires before anything is drawn.
        /// </summary>
        public static event EventHandler PreRenderEvent = delegate { };

        /// <summary>
        ///     Fires before the GUI is drawn.
        /// </summary>
        public static event EventHandler PreRenderGuiEvent = delegate { };

        /// <summary>
        ///     Fires after the GUI is drawn.
        /// </summary>
        public static event EventHandler PostRenderGuiEvent = delegate { };

        /// <summary>
        ///     Fires before the HUD is drawn.
        /// </summary>
        public static event EventHandler PreRenderHudEvent = delegate { };

        /// <summary>
        ///     Fires after the HUD is drawn.
        /// </summary>
        public static event EventHandler PostRenderHudEvent = delegate { };

        /// <summary>
        ///     Fires after everything is drawn.
        /// </summary>
        public static event EventHandler PostRenderEvent = delegate { };

        /// <summary>
        ///     Fires before the GUI is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler PreRenderGuiEventNoCheck = delegate { };

        /// <summary>
        ///     Fires after the GUI is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler PostRenderGuiEventNoCheck = delegate { };

        /// <summary>
        ///     Fires before the HUD is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler PreRenderHudEventNoCheck = delegate { };

        /// <summary>
        ///     Fires after the HUD is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler PostRenderHudEventNoCheck = delegate { };

        /// <summary>
        ///     Fires just prior to drawing into final render target.
        /// </summary>
        public static event EventHandler DrawInRenderTick = delegate { };

        /// <summary>
        ///     Occurs when the chat box is drawn.
        /// </summary>
        public static event EventHandler<EventArgsChatBoxDraw> ChatBoxDraw = delegate { };

        // ReSharper disable once StyleCop.SA1600
        [Hook(HookType.Entry, "StardewValley.Game1", "Window_ClientSizeChanged")]
        public static void OnResize([ThisBind] Game1 @this)
        {
            EventCommon.SafeInvoke(
                Resize,
                @this,
                new EventArgsClientSizeChanged(@this.Window.ClientBounds.Width, @this.Window.ClientBounds.Height));
        }

        // ReSharper disable once StyleCop.SA1600
        [Hook(HookType.Entry, "StardewValley.Game1", "Draw")]
        public static void OnBeforeDraw([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(BeforeDraw, @this);
        }

        // ReSharper disable once StyleCop.SA1600
        [Hook(HookType.Exit, "StardewValley.Game1", "Draw")]
        public static void OnAfterDraw([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(AfterDraw, @this);
        }

        // ReSharper disable once StyleCop.SA1600
        [Hook(HookType.Entry, "StardewValley.Menus.ChatBox", "draw")]
        public static bool OnChatBoxDraw([ThisBind] object @this, [InputBind(typeof(SpriteBatch), "b")] SpriteBatch b)
        {
            return EventCommon.SafeCancellableInvoke(ChatBoxDraw, @this, new EventArgsChatBoxDraw((ChatBox)@this, b));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderEvent(object @this)
        {
            PreRenderEvent.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderGuiEvent(object @this)
        {
            PreRenderGuiEvent.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderGuiEvent(object @this)
        {
            PostRenderGuiEvent.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderHudEvent(object @this)
        {
            PreRenderHudEvent.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderHudEvent(object @this)
        {
            PostRenderHudEvent.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderEvent(object @this)
        {
            PostRenderEvent.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderGuiEventNoCheck(object @this)
        {
            PreRenderGuiEventNoCheck.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderGuiEventNoCheck(object @this)
        {
            PostRenderGuiEventNoCheck.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderHudEventNoCheck(object @this)
        {
            PreRenderHudEventNoCheck.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderHudEventNoCheck(object @this)
        {
            PostRenderHudEventNoCheck.Invoke(@this, EventArgs.Empty);
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnDrawInRenderTargetTick(object @this)
        {
            DrawInRenderTick.Invoke(null, EventArgs.Empty);
        }
    }
}