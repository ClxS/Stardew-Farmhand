using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Farmhand.Events.Arguments.ControlEvents
{
    public class EventArgsControllerButtonPressed : EventArgs
    {
        public EventArgsControllerButtonPressed(PlayerIndex playerIndex, Buttons buttonPressed)
        {
            PlayerIndex = playerIndex;
            ButtonPressed = buttonPressed;
        }

        public PlayerIndex PlayerIndex { get; private set; }
        public Buttons ButtonPressed { get; private set; }
    }
}
