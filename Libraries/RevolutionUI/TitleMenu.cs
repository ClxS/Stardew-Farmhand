using Revolution.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevolutionUI
{
    [HookForceVirtualBase]
    [HookAlterBaseProtection(LowestProtection.Protected)]
    public class TitleMenu : StardewValley.Menus.TitleMenu
    {

    }
}
