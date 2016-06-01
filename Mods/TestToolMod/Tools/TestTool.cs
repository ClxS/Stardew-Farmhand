using Farmhand.API.Tools;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestToolMod.Tools
{
    public class TestTool : StardewValley.Tool
    {
        private static ToolInformation _information;
        public static ToolInformation Information => _information ?? (_information = new Farmhand.API.Tools.ToolInformation
        {
            Name = "TestTool",
            Texture = TestToolMod.Instance.ModSettings.GetTexture("sprite_TestTool"),
            Description = "Waving this will summon a Parsnip.",
        });

        public TestTool() :
            base(Information.Name,
                Information.UpgradeLevel,
                Information.GetInitialParentIndex(),
                Information.GetIndexOfMenuItem(),
                Information.Description,
                Information.Stackable,
                Information.AttachmentSlots)
        {
            this.upgradeLevel = Information.UpgradeLevel;
        }

        // Overriding this to return without any chances keeps base from altering our sprite sheet index
        public override void setNewTileIndexForUpgradeLevel()
        {
            return;
        }

        public override void beginUsing(GameLocation location, int x, int y, Farmer who)
        {
            base.Update(who.facingDirection, 0, who);
            if (who.IsMainPlayer)
            {
                Game1.releaseUseToolButton();
                return;
            }
            switch (who.FacingDirection)
            {
                case 0:
                    who.FarmerSprite.setCurrentFrame(176);
                    who.CurrentTool.Update(0, 0);
                    return;
                case 1:
                    who.FarmerSprite.setCurrentFrame(168);
                    who.CurrentTool.Update(1, 0);
                    return;
                case 2:
                    who.FarmerSprite.setCurrentFrame(160);
                    who.CurrentTool.Update(2, 0);
                    return;
                case 3:
                    who.FarmerSprite.setCurrentFrame(184);
                    who.CurrentTool.Update(3, 0);
                    return;
                default:
                    return;
            }
        }

        public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
        {
            Farmhand.API.Player.Player.AddObject(24);
        }
    }
}
