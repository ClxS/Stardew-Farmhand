using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Revolution.Events.Arguments.GameEvents
{
    public class EventArgsOnBeforeGameUpdate : CancelEventArgs
    {
        public EventArgsOnBeforeGameUpdate(GameTime gameTime)
        {
            GameTime = gameTime;
        }
        
        public GameTime GameTime { get; set; }
    }
}
