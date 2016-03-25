using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardewModdingAPI.Events
{
    public class EventArgsControllerTriggerPressed : EventArgs
    {
        public EventArgsControllerTriggerPressed(PlayerIndex playerIndex, Buttons buttonPressed, float value)
        {
            PlayerIndex = playerIndex;
            ButtonPressed = buttonPressed;
            Value = value;
        }
        public PlayerIndex PlayerIndex { get; private set; }
        public Buttons ButtonPressed { get; private set; }
        public float Value { get; private set; }
    }
}