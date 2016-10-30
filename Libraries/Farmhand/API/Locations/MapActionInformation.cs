using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Locations
{
    public class MapActionInformation
    {
        public Mod Owner { get; set; }
        
        public string Action { get; set; }

        public Func<string, bool> Callback { get; set; }
        
        public MapActionInformation(Mod owner, string action, Func<string, bool> callback)
        {
            Owner = owner;
            Action = action;
            Callback = callback;
        }
    }
}
