using StardewValley;
using System;

namespace Farmhand.Events.Arguments.PlayerEvents
{
    public class EventArgsOnFarmerChanged : EventArgs
    {
        public EventArgsOnFarmerChanged(Farmer priorFarmer, Farmer newFarmer)
        {
            PreviousFarmer = priorFarmer;
            NewFarmer = newFarmer;
        }

        public Farmer PreviousFarmer { get; set; }
        public Farmer NewFarmer { get; set; }
    }
}
