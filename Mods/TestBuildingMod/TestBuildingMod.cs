using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand;
using TestBuildingMod.Blueprints;

namespace TestBuildingMod
{
    public class RecipeTestMod : Mod
    {
        public override void Entry()
        {
            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
        }

        private void GameEvents_OnAfterLoadedContent(object sender, EventArgs e)
        {
            Farmhand.API.Buildings.Blueprint.RegisterBlueprint(Shed.Information);
        }
    }
}
