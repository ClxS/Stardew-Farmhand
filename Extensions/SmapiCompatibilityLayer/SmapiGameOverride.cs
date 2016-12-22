using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewModdingAPI.Inheritance;

namespace SmapiCompatibilityLayer
{
    class SmapiGameOverride : SGame
    {
        public SmapiGameOverride() : base(CompatibilityLayer.Monitor)
        {

        }
    }
}
