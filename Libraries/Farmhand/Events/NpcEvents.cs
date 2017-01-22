namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.NpcEvents;

    using StardewValley;

    /// <summary>
    ///     Contains events related to mailing.
    /// </summary>
    public static class NpcEvents
    {
        /// <summary>
        ///     Fires upon exiting an in-game event.
        /// </summary>
        public static event EventHandler<BeforeCheckActionEventArgs> BeforeCheckAction = delegate { };

        [HookReturnable(HookType.Entry, "StardewValley.NPC", "checkAction")]
        internal static bool OnBeforeLocationLoadObjects(
            [UseOutputBind] ref bool useOutput,
            [ThisBind] object @this,
            [InputBind(typeof(Farmer), "who")] Farmer who,
            [InputBind(typeof(GameLocation), "l")] GameLocation location)
        {
            var eventArgs = new BeforeCheckActionEventArgs(location, who);
            EventCommon.SafeInvoke(BeforeCheckAction, @this, eventArgs);
            return eventArgs.Handled;
        }
    }
}