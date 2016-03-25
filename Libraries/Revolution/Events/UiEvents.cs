using System;
using Revolution.Attributes;

namespace Revolution.Events
{
    public static class UiEvents
    {
        public static event EventHandler OnAfterIClickableMenuInitialized = delegate { };
        public static event EventHandler OnAfterBundleConstructed = delegate { };

        [Hook(HookType.Exit, "StardewValley.Menus.IClickableMenu", "initialize")]
        internal static void InvokeAfterIClickableMenuInitialized([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterIClickableMenuInitialized, null);
        }
        
        [Hook(HookType.Exit, "StardewValley.Menus.Bundle", ".ctor")]
        internal static void InvokeAfterBundleConstructed([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterBundleConstructed, @this);
        }
    }
}
