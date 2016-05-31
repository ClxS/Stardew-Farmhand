using Farmhand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCropMod.Crops;
using TestCropMod.Items;

namespace TestCropMod
{
    public class TestCropMod : Mod
    {
        public static TestCropMod Instance;

        public override void Entry()
        {
            Instance = this;

            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.PlayerEvents.OnFarmerChanged += PlayerEvents_OnFarmerChanged;

            Farmhand.API.Serializer.RegisterType<Bluemelon>();
            Farmhand.API.Serializer.RegisterType<BluemelonSeeds>();
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            Farmhand.API.Items.Item.RegisterItem<Bluemelon>(Bluemelon.Information);
            Farmhand.API.Items.Item.RegisterItem<BluemelonSeeds>(BluemelonSeeds.Information);
            Farmhand.API.Crops.Crop.RegisterCrop<TestCrop>(TestCrop.Information);
        }

        private void PlayerEvents_OnFarmerChanged(object sender, System.EventArgs e)
        {
            Farmhand.API.Player.Player.AddObject(Bluemelon.Information.Id);
            Farmhand.API.Player.Player.AddObject(BluemelonSeeds.Information.Id);
        }
    }
}
