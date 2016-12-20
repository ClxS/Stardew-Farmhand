﻿using Microsoft.Xna.Framework;

using StardewValley;

namespace Farmhand.UI
{
    class ClickableConfirmComponent : ClickableTextureComponent
    {
        public ClickableConfirmComponent(Point position, ClickHandler handler = null) : base(new Rectangle(position.X, position.Y, 16, 16), Game1.mouseCursors, handler, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), true)
        {

        }
    }
}
