using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Farmhand.Events.Arguments.ControlEvents
{
    public class EventArgsKeyboardStateChanged : EventArgs
    {
        public EventArgsKeyboardStateChanged(KeyboardState priorState, KeyboardState newState)
        {
            NewState = newState;
            PriorState = priorState;
        }

        public KeyboardState NewState { get; private set; }
        public KeyboardState PriorState { get; private set; }
    }
}
