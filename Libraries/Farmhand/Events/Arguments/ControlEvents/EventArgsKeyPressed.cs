using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Farmhand.Events.Arguments.ControlEvents
{
    public class EventArgsKeyPressed : EventArgs
    {
        public EventArgsKeyPressed(Keys keyPressed)
        {
            KeyPressed = keyPressed;
        }

        public Keys KeyPressed { get; private set; }
    }
}
