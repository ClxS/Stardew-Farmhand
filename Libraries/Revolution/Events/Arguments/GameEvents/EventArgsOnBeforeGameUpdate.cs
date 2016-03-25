using System.ComponentModel;
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
