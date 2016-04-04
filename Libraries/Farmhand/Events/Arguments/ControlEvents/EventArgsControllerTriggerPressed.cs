using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Farmhand.Events.Arguments.ControlEvents
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
