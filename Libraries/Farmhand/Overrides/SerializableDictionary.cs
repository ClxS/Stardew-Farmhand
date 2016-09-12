using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Overrides
{
    [HookForceVirtualBase]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", ".ctor", typeof(Vector2), typeof(StardewValley.Object))]
    public class SerializableOverride<TKey, TValue> : SerializableDictionary<TKey, TValue>
    {
    }
}
