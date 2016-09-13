using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Overrides
{
    //[HookForceVirtualBase]
    public class SerializableOverride<TKey, TValue> : SerializableDictionary<TKey, TValue>
    {
    }
}
