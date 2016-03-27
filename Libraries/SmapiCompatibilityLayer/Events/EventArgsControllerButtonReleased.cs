using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardewModdingAPI.Events
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