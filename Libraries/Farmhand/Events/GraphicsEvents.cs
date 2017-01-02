using Farmhand.Attributes;
using System;
using Farmhand.Events.Arguments.GraphicsEvents;
using StardewValley;

namespace Farmhand.Events
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Contains events relating to graphics
    /// </summary>
    public static class GraphicsEvents
    {
        public static event EventHandler<EventArgsClientSizeChanged> OnResize = delegate { };
        public static event EventHandler OnBeforeDraw = delegate { };
        public static event EventHandler OnAfterDraw = delegate { };
        
        /// <summary>
        /// Occurs before anything is drawn.
        /// </summary>
        public static event EventHandler OnPreRenderEvent = delegate { };

        /// <summary>
        /// Occurs before the GUI is drawn.
        /// </summary>
        public static event EventHandler OnPreRenderGuiEvent = delegate { };

        /// <summary>
        /// Occurs after the GUI is drawn.
        /// </summary>
        public static event EventHandler OnPostRenderGuiEvent = delegate { };

        /// <summary>
        /// Occurs before the HUD is drawn.
        /// </summary>
        public static event EventHandler OnPreRenderHudEvent = delegate { };

        /// <summary>
        /// Occurs after the HUD is drawn.
        /// </summary>
        public static event EventHandler OnPostRenderHudEvent = delegate { };

        /// <summary>
        /// Occurs after everything is drawn.
        /// </summary>
        public static event EventHandler OnPostRenderEvent = delegate { };

        /// <summary>
        /// Occurs before the GUI is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler OnPreRenderGuiEventNoCheck = delegate { };

        /// <summary>
        /// Occurs after the GUI is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler OnPostRenderGuiEventNoCheck = delegate { };

        /// <summary>
        /// Occurs before the HUD is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler OnPreRenderHudEventNoCheck = delegate { };

        /// <summary>
        /// Occurs after the HUD is drawn. Does not check for conditional statements.
        /// </summary>
        public static event EventHandler OnPostRenderHudEventNoCheck = delegate { };

        public static event EventHandler OnDrawInRenderTick = delegate { };

        /// <summary>
        /// Occurs when the chat box is drawn.
        /// </summary>
        public static event EventHandler<EventArgsChatBoxDraw> ChatBoxDraw = delegate { };

        [Hook(HookType.Entry, "StardewValley.Game1", "Window_ClientSizeChanged")]
        public static void InvokeResize([ThisBind] Game1 @this)
        {
            EventCommon.SafeInvoke(OnResize, @this, new EventArgsClientSizeChanged(@this.Window.ClientBounds.Width, @this.Window.ClientBounds.Height));
        }
        
        [Hook(HookType.Entry, "StardewValley.Game1", "Draw")]
        public static void InvokeBeforeDraw([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnBeforeDraw, @this);
        }
        
        [Hook(HookType.Exit, "StardewValley.Game1", "Draw")]
        public static void InvokeAfterDraw([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterDraw, @this);
        }

        [Hook(HookType.Entry, "StardewValley.Menus.ChatBox", "draw")]
        public static bool OnChatBoxDraw([ThisBind] object @this, [InputBind(typeof(SpriteBatch), "b")] SpriteBatch b)
        {
            return EventCommon.SafeCancellableInvoke(ChatBoxDraw, @this, new EventArgsChatBoxDraw((StardewValley.Menus.ChatBox)@this, b));
        }

        public static void InvokeOnPreRenderEvent(object @this)
        {
            OnPreRenderEvent.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeOnPreRenderGuiEvent(object @this)
        {
            OnPreRenderGuiEvent.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeOnPostRenderGuiEvent(object @this)
        {
            OnPostRenderGuiEvent.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeOnPreRenderHudEvent(object @this)
        {
            OnPreRenderHudEvent.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeOnPostRenderHudEvent(object @this)
        {
            OnPostRenderHudEvent.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeOnPostRenderEvent(object @this)
        {
            OnPostRenderEvent.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeOnPreRenderGuiEventNoCheck(object @this)
        {
            OnPreRenderGuiEventNoCheck.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeOnPostRenderGuiEventNoCheck(object @this)
        {
            OnPostRenderGuiEventNoCheck.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeOnPreRenderHudEventNoCheck(object @this)
        {
            OnPreRenderHudEventNoCheck.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeOnPostRenderHudEventNoCheck(object @this)
        {
            OnPostRenderHudEventNoCheck.Invoke(@this, EventArgs.Empty);
        }

        public static void InvokeDrawInRenderTargetTick(object @this)
        {
            OnDrawInRenderTick.Invoke(null, EventArgs.Empty);
        }
    }
}
