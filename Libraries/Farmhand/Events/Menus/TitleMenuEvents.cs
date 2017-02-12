namespace Farmhand.Events.Menus
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.Menus.TitleMenuEvents;

    /// <summary>
    ///     Contains events relating to the TitleMenu.
    /// </summary>
    public static class TitleMenuEvents
    {
        /// <summary>
        ///     Fires just before the menu is clicked.
        /// </summary>
        /// <remarks>
        ///     This is cancellable, allowing you to prevent clicks being passed to the title menu controls.
        /// </remarks>
        public static event EventHandler<BeforeReceiveLeftClickEventArgs> BeforeReceiveLeftClick =
            delegate { };

        /// <summary>
        ///     Fires just after the menu is clicked.
        /// </summary>
        public static event EventHandler<AfterReceiveLeftClickEventArgs> AfterReceiveLeftClick =
            delegate { };

        /// <summary>
        ///     Fires just before the menu is hovered.
        /// </summary>
        /// <remarks>
        ///     This is cancellable, allowing you to prevent hover checks being passed to the title menu controls.
        /// </remarks>
        public static event EventHandler<BeforeHoverEventArgs> BeforeHoverAction =
            delegate { };

        /// <summary>
        ///     Fires just after the menu is hovered.
        /// </summary>
        public static event EventHandler<AfterHoverEventArgs> AfterHoverAction =
            delegate { };

        [Hook(HookType.Entry, "StardewValley.Menus.TitleMenu", "receiveLeftClick")]
        internal static bool OnBeforeReceiveLeftClick(
            [ThisBind] object @this,
            [InputBind(typeof(int), "x")] int x,
            [InputBind(typeof(int), "y")] int y,
            [InputBind(typeof(bool), "playSound")] bool playSound)
        {
            var eventArgs = new BeforeReceiveLeftClickEventArgs(x, y, playSound);
            EventCommon.SafeInvoke(BeforeReceiveLeftClick, @this, eventArgs);
            return eventArgs.Cancel;
        }

        [Hook(HookType.Exit, "StardewValley.Menus.TitleMenu", "receiveLeftClick")]
        internal static void OnAfterReceiveLeftClick(
            [ThisBind] object @this,
            [InputBind(typeof(int), "x")] int x,
            [InputBind(typeof(int), "y")] int y,
            [InputBind(typeof(bool), "playSound")] bool playSound)
        {
            EventCommon.SafeInvoke(
                AfterReceiveLeftClick,
                @this,
                new AfterReceiveLeftClickEventArgs(x, y, playSound));
        }

        [Hook(HookType.Entry, "StardewValley.Menus.TitleMenu", "performHoverAction")]
        internal static bool OnBeforeHover(
            [ThisBind] object @this,
            [InputBind(typeof(int), "x")] int x,
            [InputBind(typeof(int), "y")] int y)
        {
            var eventArgs = new BeforeHoverEventArgs(x, y);
            EventCommon.SafeInvoke(BeforeHoverAction, @this, eventArgs);
            return eventArgs.Cancel;
        }

        [Hook(HookType.Exit, "StardewValley.Menus.TitleMenu", "performHoverAction")]
        internal static void OnAfterHover(
            [ThisBind] object @this,
            [InputBind(typeof(int), "x")] int x,
            [InputBind(typeof(int), "y")] int y)
        {
            EventCommon.SafeInvoke(
                AfterHoverAction,
                @this,
                new AfterHoverEventArgs(x, y));
        }
    }
}