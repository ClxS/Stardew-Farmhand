using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using StardewValley;
using xTile.Dimensions;

namespace Farmhand.Events.Arguments.LocationEvents
{
    public class EventArgsOnBeforeCheckAction : EventArgs
    {
        public EventArgsOnBeforeCheckAction(GameLocation gameLocation, Location tileLocation, Rectangle viewport, Farmer farmer)
        {
            GameLocation = gameLocation;
            TileLocation = tileLocation;
            Viewport = viewport;
            Farmer = farmer;
            Handled = false;
        }

        /// <summary>
        /// The location where this event is
        /// </summary>
        public GameLocation GameLocation { get; set; }

        /// <summary>
        /// The tile location of the event
        /// </summary>
        public Location TileLocation { get; set; }

        /// <summary>
        /// The current screen viewport
        /// </summary>
        public Rectangle Viewport { get; set; }

        /// <summary>
        /// The farmer who triggered this event
        /// </summary>
        public Farmer Farmer { get; set; }

        /// <summary>
        /// Whether this event has been handled by a mod. You should only set this to true to 
        /// prevent interfering with other mods.
        /// </summary>
        public bool Handled { get; set; }
    }
}
