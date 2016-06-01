using Farmhand;
using Farmhand.Events;
using Farmhand.Events.Arguments.GlobalRoute;
using Farmhand.Overrides;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestToolMod.Tools;

namespace TestToolMod
{
    public class TestToolMod : Mod
    {
        public static TestToolMod Instance;

        public override void Entry()
        {
            Instance = this;

            Farmhand.API.Serializer.RegisterType<TestTool>();

            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.PlayerEvents.OnFarmerChanged += PlayerEvents_OnFarmerChanged;
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            // Register the tool so it can be added to the sprite sheet
            Farmhand.API.Tools.Tool.RegisterTool<StardewValley.Tool>(TestTool.Information);
                
        }

        private void PlayerEvents_OnFarmerChanged(object sender, System.EventArgs e)
        {
            // Give the player the tool
            Farmhand.API.Player.Player.AddTool<TestTool>();
        }
    }
}
