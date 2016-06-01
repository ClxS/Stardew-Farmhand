using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Tools
{
    public class ToolInformation
    {
        // Tool name
        public string Name { get; set; }

        // Tool Texture
        public Texture2D Texture { get; set; }

        // Tool Index Id
        public int Id { get; set; }

        // Tool Description
        public string Description { get; set; }

        // Stackable?
        public bool Stackable { get; set; } = false;

        // Number of attachment slots
        public int AttachmentSlots { get; set; } = 0;

        // Upgrade level
        public int UpgradeLevel { get; set; } = -1;

        // Determine, based off the Id of the tool, where the InitialParentIndex is
        public int GetInitialParentIndex()
        {
            int largeIndex = Tool.InitialTools + Id;

            int XLarge = largeIndex % 3;
            int YLarge = largeIndex / 3;

            int XSmall = XLarge * 7;
            int YSmall = YLarge * 2;

            int SmallIndex = XSmall + (YSmall * 21);

            return SmallIndex;
        }

        // Determine, based off the Id of the tool, where the IndexOfMenuItem is
        public int GetIndexOfMenuItem()
        {
            return GetInitialParentIndex() + 26;
        }
    }
}
