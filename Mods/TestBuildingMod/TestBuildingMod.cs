using System;
using Farmhand;
using TestBuildingMod.Blueprints;

namespace TestBuildingMod
{
    public class RecipeTestMod : Mod
    {
        public override void Entry()
        {
            Farmhand.Events.GameEvents.AfterLoadedContent += GameEvents_OnAfterLoadedContent;
        }

        private void GameEvents_OnAfterLoadedContent(object sender, EventArgs e)
        {
            Farmhand.API.Buildings.Blueprint.RegisterBlueprint(Shed.Information);
        }
    }
}
