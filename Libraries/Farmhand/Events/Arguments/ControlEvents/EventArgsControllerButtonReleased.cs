using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Farmhand.Events.Arguments.ControlEvents
{
    public class EventArgsControllerButtonReleased : EventArgs
    {
        public EventArgsControllerButtonReleased(PlayerIndex playerIndex, Buttons buttonReleased)
        {
            PlayerIndex = playerIndex;
            ButtonReleased = buttonReleased;
        }

        public PlayerIndex PlayerIndex { get; private set; }
        public Buttons ButtonReleased { get; private set; }
    }
}
