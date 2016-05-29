using Farmhand.Attributes;
using Farmhand.Events.Arguments.LocationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Events
{
    public class MineShaftEvents
    {
        public static event EventHandler<EventArgsGetMonsterForThisLevel> OnGetMonsterForThisLevel = delegate { };

        [Hook(HookType.Exit, "StardewValley.Locations.MineShaft", "getMonsterForThisLevel")]
        internal static void GetMonsterForThisLevel([ThisBind] object @this,
            [InputBind(typeof(int), "level")] int level,
            [InputBind(typeof(int), "xTile")] int xTile,
            [InputBind(typeof(int), "yTile")] int yTile
        )
        {
            EventCommon.SafeInvoke(OnGetMonsterForThisLevel, @this, new EventArgsGetMonsterForThisLevel(level, xTile, yTile));
        }
    }
}
