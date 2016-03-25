using System;
using Microsoft.Xna.Framework;
using StardewValley;

namespace StardewModdingAPI.Events
{
    public class EventArgsLocationObjectsChanged : EventArgs
    {
        public EventArgsLocationObjectsChanged(SerializableDictionary<Vector2, StardewValley.Object> newObjects)
        {
            NewObjects = newObjects;
        }
        public SerializableDictionary<Vector2, StardewValley.Object> NewObjects { get; private set; }
    }
}