using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand.Events;
using Farmhand.Events.Arguments.ControlEvents;
using Microsoft.Xna.Framework.Input;

using StardewValley;
using StardewValley.Menus;

namespace Farmhand.UI
{
    /// <summary>
    /// Enables overlaying any <see cref="IClickableMenu"/> over the Gui, both in and outside of menus
    /// </summary>
    public static class OverlayManager
    {
        /// <summary>
        /// Adds the given overlay to the list of <see cref="IClickableMenu"/>'s that should receive emulated events
        /// </summary>
        /// <param name="overlay">The <see cref="IClickableMenu"/> to add</param>
        public static void Add(IClickableMenu overlay)
        {
            Overlays.Add(overlay);
            if (Overlays.Count == 1)
                Init();
        }
        /// <summary>
        /// Removes the given overlay from the list of <see cref="IClickableMenu"/>'s that should receive emulated events
        /// </summary>
        /// <param name="overlay">The <see cref="IClickableMenu"/> to remove</param>
        public static void Remove(IClickableMenu overlay)
        {
            Overlays.Remove(overlay);
            if (Overlays.Count == 0)
                Uninit();
        }
        /// <summary>
        /// Empties the list of <see cref="IClickableMenu"/>'s that should receive emulated events
        /// </summary>
        public static void Clear()
        {
            Overlays.Clear();
            Uninit();
        }
        /// <summary>
        /// Checks if a given <see cref="IClickableMenu"/> is in the list of emulated <see cref="IClickableMenu"/>'s
        /// </summary>
        /// <param name="overlay">The <see cref="IClickableMenu"/> to check for</param>
        /// <returns></returns>
        public static bool Contains(IClickableMenu overlay)
        {
            return Overlays.Contains(overlay);
        }
        /// <summary>
        /// Count of how many <see cref="IClickableMenu"/>'s are currently in the list
        /// </summary>
        public static int Count
        {
            get
            {
                return Overlays.Count;
            }
        }
        /// <summary>
        /// Tries to remove the given <see cref="IClickableMenu"/> from the list and returns a <see cref="bool"/> with the result of this attempt
        /// </summary>
        /// <param name="overlay">The <see cref="IClickableMenu"/> to remove</param>
        /// <returns>Returns true if the given overlay was removed successfully, or false otherwise</returns>
        public static bool TryRemove(IClickableMenu overlay)
        {
            bool a = Contains(overlay);
            if (a)
                Remove(overlay);
            return a;
        }
        // Internal functional code, not related to API level code.
        private static List<IClickableMenu> Overlays = new List<IClickableMenu>();
        private static void Init()
        {
            GameEvents.OnAfterUpdateTick += GameEvents_UpdateTick;
            GraphicsEvents.OnPostRenderGuiEvent += GraphicsEvents_OnPostRenderGuiEvent;
            ControlEvents.OnMouseChanged += ControlEvents_MouseChanged;
            ControlEvents.OnKeyboardChanged += ControlEvents_KeyboardChanged;
        }
        private static void Uninit()
        {
            GameEvents.OnAfterUpdateTick -= GameEvents_UpdateTick;
            GraphicsEvents.OnPostRenderGuiEvent -= GraphicsEvents_OnPostRenderGuiEvent;
            ControlEvents.OnMouseChanged -= ControlEvents_MouseChanged;
            ControlEvents.OnKeyboardChanged -= ControlEvents_KeyboardChanged;
        }
        internal static void GameEvents_UpdateTick(object s, EventArgs e)
        {
            foreach (IClickableMenu overlay in Overlays)
            {
                overlay.update(Game1.currentGameTime);
                overlay.performHoverAction(Game1.getMouseX(), Game1.getMouseY());
            }
        }
        internal static void GraphicsEvents_OnPostRenderGuiEvent(object s, EventArgs e)
        {
            foreach (IClickableMenu overlay in Overlays)
                overlay.draw(Game1.spriteBatch);
        }
        internal static void ControlEvents_MouseChanged(object s, EventArgsMouseStateChanged e)
        {
            if (e.NewState.LeftButton == ButtonState.Pressed && e.PriorState.LeftButton == ButtonState.Released)
                foreach (IClickableMenu overlay in Overlays)
                    overlay.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
            if (e.NewState.RightButton == ButtonState.Pressed && e.PriorState.RightButton == ButtonState.Released)
                foreach (IClickableMenu overlay in Overlays)
                    overlay.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
            if (e.NewState.ScrollWheelValue != e.PriorState.ScrollWheelValue)
                foreach (IClickableMenu overlay in Overlays)
                    overlay.receiveScrollWheelAction(e.NewState.ScrollWheelValue - e.PriorState.ScrollWheelValue);
            if (e.NewState.RightButton == ButtonState.Released && e.PriorState.RightButton == ButtonState.Pressed)
                foreach (IClickableMenu overlay in Overlays)
                    overlay.releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
            if (e.NewState.RightButton == ButtonState.Pressed && e.PriorState.RightButton == ButtonState.Pressed)
                foreach (IClickableMenu overlay in Overlays)
                    overlay.leftClickHeld(Game1.getMouseX(), Game1.getMouseY());
        }
        internal static void ControlEvents_KeyboardChanged(object s, EventArgsKeyboardStateChanged e)
        {
            IEnumerable<Keys> old = e.PriorState.GetPressedKeys();
            foreach (Keys key in e.NewState.GetPressedKeys())
                if (!old.Contains(key))
                    foreach (IClickableMenu overlay in Overlays)
                        overlay.receiveKeyPress(key);
        }
    }
}
