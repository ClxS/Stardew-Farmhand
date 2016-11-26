using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.NPCs
{
    public static class NPCUtility
    {
        public static class Maps
        {
            public static readonly string Wizard_House = "WizardHouse";
            public static readonly string Alex_House = "JoshHouse";
            public static readonly string Mountain = "Mountain";
        }

        public static class BirthdaySeason
        {
            public static readonly string Spring = "spring";
            public static readonly string Summer = "summer";
            public static readonly string Fall = "fall";
            public static readonly string Winter = "winter";
        }

        public static class Direction
        {
            public static readonly int South = 0;
            public static readonly int East = 1;
            public static readonly int North = 2;
            public static readonly int West = 3;
        }
    }
}
