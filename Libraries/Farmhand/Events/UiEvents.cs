namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;

    /// <summary>
    ///     Contains events relating to the UI
    /// </summary>
    public static class UiEvents
    {
        /// <summary>
        ///     Fired after an IClickableMenu is initialised.
        /// </summary>
        public static event EventHandler AfterIClickableMenuInitialized = delegate { };

        /// <summary>
        ///     Fired after a new instance of Bundle is constructed.
        /// </summary>
        public static event EventHandler AfterBundleConstructed = delegate { };

#if WINDOWS
        [Hook(HookType.Exit, "StardewValley.Menus.IClickableMenu", "initialize")]
#endif
        internal static void OnAfterIClickableMenuInitialized(
            [ThisBind] object @this,
            [InputBind(typeof(int), "x")] int x,
            [InputBind(typeof(int), "y")] int y,
            [InputBind(typeof(int), "width")] int width,
            [InputBind(typeof(int), "height")] int height,
            [InputBind(typeof(bool), "showUpperRightCloseButton")] bool showCloseBtn)
        {
            EventCommon.SafeInvoke(AfterIClickableMenuInitialized, null);
        }

#if WINDOWS
        [Hook(HookType.Exit, "StardewValley.Menus.Bundle", ".ctor")]
#endif
        internal static void OnAfterBundleConstructed([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(AfterBundleConstructed, @this);
        }
    }
}