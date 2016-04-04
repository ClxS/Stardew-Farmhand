using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Farmhand.Events.Arguments.ControlEvents
{
    public class EventArgsControllerTriggerReleased : EventArgs
    {
        public EventArgsControllerTriggerReleased(PlayerIndex playerIndex, Buttons buttonReleased, float value)
        {
            PlayerIndex = playerIndex;
            ButtonReleased = buttonReleased;
            Value = value;
        }

        public PlayerIndex PlayerIndex { get; private set; }
        public Buttons ButtonReleased { get; private set; }
        public float Value { get; private set; }
    }
}
