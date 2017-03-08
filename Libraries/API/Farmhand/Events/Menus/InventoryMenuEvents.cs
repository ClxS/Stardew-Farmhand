namespace Farmhand.Events.Menus
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.Menus.InventoryMenuEvents;

    using StardewValley;

    /// <summary>
    ///     Contains events relating to the InventoryMenu.
    /// </summary>
    public class InventoryMenuEvents
    {
        /// <summary>
        ///     Fires just before deciding whether an item is hovered.
        /// </summary>
        public static event EventHandler<BeforeItemHoverEventArgs> BeforeItemHover = delegate { };

        /// <summary>
        ///     Fires just after deciding whether an item is hovered.
        /// </summary>
        public static event EventHandler<AfterItemHoverEventArgs> AfterItemHover = delegate { };

        [HookReturnable(HookType.Entry, "StardewValley.Menus.InventoryMenu", "hover")]
        internal static Item OnBeforeItemHover(
            [UseOutputBind] out bool useOutput,
            [ThisBind] object @this,
            [InputBind(typeof(int), "x")] int x,
            [InputBind(typeof(int), "y")] int y,
            [InputBind(typeof(Item), "heldItem")] Item heldItem)
        {
            var eventArgs = new BeforeItemHoverEventArgs(x, y, heldItem);
            EventCommon.SafeInvoke(BeforeItemHover, @this, eventArgs);
            useOutput = eventArgs.IsHandled;
            return eventArgs.Item;
        }

        [HookReturnable(HookType.Exit, "StardewValley.Menus.InventoryMenu", "hover")]
        internal static Item OnAfterItemHover(
            [UseOutputBind] out bool useOutput,
            [ThisBind] object @this,
            [MethodOutputBind] Item item,
            [InputBind(typeof(int), "x")] int x,
            [InputBind(typeof(int), "y")] int y,
            [InputBind(typeof(Item), "heldItem")] Item heldItem)
        {
            var eventArgs = new AfterItemHoverEventArgs(item, x, y, heldItem);
            EventCommon.SafeInvoke(AfterItemHover, @this, eventArgs);
            useOutput = eventArgs.IsHandled;
            return eventArgs.Item;
        }
    }
}