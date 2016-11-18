using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.Content;

namespace Farmhand.API
{
    public static class Content
    {
        public static ContentManager ContentManager => StardewValley.Game1.content as ContentManager;
    }
}
