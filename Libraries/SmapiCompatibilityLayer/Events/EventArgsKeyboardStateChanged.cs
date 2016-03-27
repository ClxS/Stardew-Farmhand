using Microsoft.Xna.Framework.Input;
using System;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Events
{
    public class EventArgsKeyboardStateChanged : EventArgs
    {
        public EventArgsKeyboardStateChanged(KeyboardState priorState, KeyboardState newState)
        {
            PriorState = priorState;
            NewState = newState;
        }
        public KeyboardState NewState { get; private set; }
        public KeyboardState PriorState { get; private set; }
    }
}
