using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Farmhand.Events.Arguments.ControlEvents
{
    public class EventArgsMouseStateChanged : EventArgs
    {
        public EventArgsMouseStateChanged(MouseState priorState, MouseState newState)
        {
            NewState = newState;
            PriorState = priorState;
        }

        public MouseState NewState { get; private set; }
        public MouseState PriorState { get; private set; }
    }
}
