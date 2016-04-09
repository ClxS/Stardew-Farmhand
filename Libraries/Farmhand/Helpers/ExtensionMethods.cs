using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.API;
using Farmhand.API.Generic;

namespace Farmhand.Helpers
{
    public static class ExtensionMethods
    {
        public static string ToItemSetString(this List<ItemQuantityPair> items)
        {
            return string.Join(" ", items.Select(x => $"{x.ItemId} {x.Count}"));
        } 
    }
}
