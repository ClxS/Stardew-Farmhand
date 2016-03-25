using System;
using Microsoft.Xna.Framework.Input;

namespace StardewModdingAPI.Events
{
    public class EventArgsMouseStateChanged : EventArgs
    {
        public EventArgsMouseStateChanged(MouseState priorState, MouseState newState)
        {
            PriorState = priorState;
            NewState = newState;
        }
        public MouseState NewState { get; private set; }
        public MouseState PriorState { get; private set; }
    }
}