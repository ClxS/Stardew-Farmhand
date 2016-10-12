using System;
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
