namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.GraphicsEvents;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;
    using StardewValley.Menus;
    
    /// <summary>
    ///     Contains events relating to graphics
    /// </summary>
    public static class GraphicsEvents
    {
        /// <summary>
        ///     Fires just before the game's window resizes.
        /// </summary>
        public static event EventHandler<ClientSizeChangedEventArgs> BeforeResize = delegate { };

        /// <summary>
        ///     Fires just after the game's window resizes.
        /// </summary>
        public static event EventHandler<ClientSizeChangedEventArgs> AfterResize = delegate { };

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
        public static event EventHandler<DrawEventArgs> PreRenderEvent = delegate { };

        /// <summary>
        ///     Fires before the GUI is drawn.
        /// </summary>
        public static event EventHandler<DrawEventArgs> PreRenderGuiEvent = delegate { };

        /// <summary>
        ///     Fires after the GUI is drawn.
        /// </summary>
        public static event EventHandler<DrawEventArgs> PostRenderGuiEvent = delegate { };

        /// <summary>
        ///     Fires before the HUD is drawn.
        /// </summary>
        public static event EventHandler<DrawEventArgs> PreRenderHudEvent = delegate { };

        /// <summary>
        ///     Fires after the HUD is drawn.
        /// </summary>
        public static event EventHandler<DrawEventArgs> PostRenderHudEvent = delegate { };

        /// <summary>
        ///     Fires after everything is drawn.
        /// </summary>
        public static event EventHandler<DrawEventArgs> PostRenderEvent = delegate { };

        /// <summary>
        ///     Fires before the GUI is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler<DrawEventArgs> PreRenderGuiEventNoCheck = delegate { };

        /// <summary>
        ///     Fires after the GUI is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler<DrawEventArgs> PostRenderGuiEventNoCheck = delegate { };

        /// <summary>
        ///     Fires before the HUD is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler<DrawEventArgs> PreRenderHudEventNoCheck = delegate { };

        /// <summary>
        ///     Fires after the HUD is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler<DrawEventArgs> PostRenderHudEventNoCheck = delegate { };

        /// <summary>
        ///     Fires just prior to drawing into final render target.
        /// </summary>
        public static event EventHandler<DrawEventArgs> DrawInRenderTick = delegate { };

        /// <summary>
        ///     Occurs when the chat box is drawn.
        /// </summary>
        public static event EventHandler<ChatBoxDrawEventArgs> ChatBoxDraw = delegate { };

        internal static bool IsMidDrawCall { get; set; }

        // ReSharper disable once StyleCop.SA1600
        [Hook(HookType.Entry, "StardewValley.Game1", "Window_ClientSizeChanged")]
        public static void OnBeforeResize([ThisBind] Game1 @this)
        {
            EventCommon.SafeInvoke(
                BeforeResize,
                @this,
                new ClientSizeChangedEventArgs(@this.Window.ClientBounds.Width, @this.Window.ClientBounds.Height));
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "Window_ClientSizeChanged")]
        public static void OnAfterResize([ThisBind] Game1 @this)
        {
            EventCommon.SafeInvoke(
                AfterResize,
                @this,
                new ClientSizeChangedEventArgs(@this.Window.ClientBounds.Width, @this.Window.ClientBounds.Height));
        }

        // ReSharper disable once StyleCop.SA1600
        [Hook(HookType.Entry, "StardewValley.Game1", "Draw")]
        public static void OnBeforeDraw([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(BeforeDraw, @this);
            IsMidDrawCall = true;
        }

        // ReSharper disable once StyleCop.SA1600
        [Hook(HookType.Exit, "StardewValley.Game1", "Draw")]
        public static void OnAfterDraw([ThisBind] object @this)
        {
            IsMidDrawCall = false;
            EventCommon.SafeInvoke(AfterDraw, @this);
        }

        // ReSharper disable once StyleCop.SA1600
        [Hook(HookType.Entry, "StardewValley.Menus.ChatBox", "draw")]
        public static bool OnChatBoxDraw([ThisBind] object @this, [InputBind(typeof(SpriteBatch), "b")] SpriteBatch b)
        {
            return EventCommon.SafeCancellableInvoke(ChatBoxDraw, @this, new ChatBoxDrawEventArgs((ChatBox)@this, b));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderEvent(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PreRenderEvent.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderGuiEvent(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PreRenderGuiEvent.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderGuiEvent(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PostRenderGuiEvent.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderHudEvent(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PreRenderHudEvent.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderHudEvent(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PostRenderHudEvent.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderEvent(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PostRenderEvent.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderGuiEventNoCheck(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PreRenderGuiEventNoCheck.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderGuiEventNoCheck(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PostRenderGuiEventNoCheck.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPreRenderHudEventNoCheck(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PreRenderHudEventNoCheck.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnPostRenderHudEventNoCheck(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            PostRenderHudEventNoCheck.Invoke(@this, new DrawEventArgs(b, gt, screen));
        }

        // ReSharper disable once StyleCop.SA1600
        public static void OnDrawInRenderTargetTick(object @this, SpriteBatch b, GameTime gt, RenderTarget2D screen)
        {
            DrawInRenderTick.Invoke(null, new DrawEventArgs(b, gt, screen));
        }
    }
}