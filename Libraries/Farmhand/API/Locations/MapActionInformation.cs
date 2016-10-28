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

        public Func<bool> Callback { get; set; }
        
        public MapActionInformation(Mod owner, string action, Func<bool> callback)
        {
            Owner = owner;
            Action = action;
            Callback = callback;
        }
    }
}
