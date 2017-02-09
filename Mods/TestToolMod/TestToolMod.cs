using Farmhand;
using Farmhand.Overrides;
using TestToolMod.Tools;
using TestToolMod.Weapons;

namespace TestToolMod
{
    using Farmhand.Game;

    public class TestToolMod : Mod
    {
        public static TestToolMod Instance;

        public override void Entry()
        {
            Instance = this;

            Farmhand.API.Serializer.RegisterType<TestTool>();
            Farmhand.API.Serializer.RegisterType<TestWeapon>();

            Farmhand.Events.GameEvents.AfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.PlayerEvents.FarmerChanged += PlayerEvents_OnFarmerChanged;
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            // Register the tool so it can be added to the sprite sheet
            Farmhand.API.Tools.Tool.RegisterTool<TestTool>(TestTool.Information);

            // Register the weapon
            Farmhand.API.Tools.Weapon.RegisterWeapon<TestWeapon>(TestWeapon.Information);
        }

        private void PlayerEvents_OnFarmerChanged(object sender, System.EventArgs e)
        {
            // Check if the player already has this tool
            bool hasTool = false;
            for(int i=0; i< Farmhand.API.Game.Player.items.Count; i++)
            {
                if(Farmhand.API.Game.Player.items[i] is TestTool)
                {
                    hasTool = true;
                }
            }

            // Give the player the tool
            if(!hasTool)
            {
                Farmhand.API.Player.Player.AddTool<TestTool>();
            }


            // Check if the player already has this weapon
            bool hasWeapon = false;
            for (int i = 0; i < Farmhand.API.Game.Player.items.Count; i++)
            {
                if (Farmhand.API.Game.Player.items[i] is TestWeapon)
                {
                    hasWeapon = true;
                }
            }

            // Give the player the tool
            if (!hasWeapon)
            {
                Farmhand.API.Player.Player.AddTool(new TestWeapon(TestWeapon.Information));
            }
        }
    }
}
