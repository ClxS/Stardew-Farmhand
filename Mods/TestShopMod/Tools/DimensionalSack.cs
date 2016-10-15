using Farmhand.API.Tools;
using Farmhand.Overrides.UI;
using StardewValley;

namespace TestShopMod.Tools
{
    public class DimensionalSack : StardewValley.Tool
    {
        private static ToolInformation _information;
        public static ToolInformation Information => _information ?? (_information = new Farmhand.API.Tools.ToolInformation
        {
            Name = "Dimensional Sack",
            Texture = TestShopMod.Instance.ModSettings.GetTexture("sprite_DimensionalSack"),
            Description = "There doesn't seem to be a bottom"
        });

        public DimensionalSack() :
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
        }

        public override void beginUsing(GameLocation location, int x, int y, Farmer who)
        {
            Update(who.facingDirection, 0, who);
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
            ShopMenu.OpenShop("DimensionalSack");
        }
    }
}
