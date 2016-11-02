using System.ComponentModel;
using StardewValley;

namespace Farmhand.Events.Arguments.LocationEvents
{
    public class EventArgsOnBeforeWarp : CancelEventArgs
    {
        public EventArgsOnBeforeWarp(GameLocation locationAfterWarp, int tileX, int tileY, int facingDirectionAfterWarp)
        {
            LocationAfterWarp = locationAfterWarp;
            TileX = tileX;
            TileY = tileY;
            FacingDirectionAfterWarp = facingDirectionAfterWarp;
        }

        public GameLocation LocationAfterWarp { get; set; }
        public int TileX { get; set; }
        public int TileY { get; set; }
        public int FacingDirectionAfterWarp { get; set; }
    }
}
