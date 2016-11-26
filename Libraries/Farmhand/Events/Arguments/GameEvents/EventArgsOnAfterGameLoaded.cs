using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments.GameEvents
{
    public class EventArgsOnAfterGameLoaded : EventArgs
    {
        public EventArgsOnAfterGameLoaded(bool loadedGame = false)
        {
            LoadedGame = loadedGame;
        }

        public bool LoadedGame { get; set; }
    }
}
