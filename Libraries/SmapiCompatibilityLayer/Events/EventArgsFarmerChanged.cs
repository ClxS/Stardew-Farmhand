using System;
using StardewValley;

namespace StardewModdingAPI.Events
{
    public class EventArgsFarmerChanged : EventArgs
    {
        public EventArgsFarmerChanged(Farmer priorFarmer, Farmer newFarmer)
        {
            NewFarmer = newFarmer;
            PriorFarmer = priorFarmer;
        }
        public Farmer NewFarmer { get; }
        public Farmer PriorFarmer { get; }
    }
}