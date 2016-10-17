using Farmhand;
using Farmhand.API.Locations;

namespace TestMapMergeMod
{
    public class TestMapMergeMod : Mod
    {
        public static TestMapMergeMod Instance;

        public override void Entry()
        {
            Instance = this;

            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            MapInformation busStopEdit1 = new MapInformation(Instance, Instance.ModSettings.GetMap("busStop_edit1"));
            MapInformation busStopEdit2 = new MapInformation(Instance, Instance.ModSettings.GetMap("busStop_edit2"));

            LocationUtilities.RegisterMap(Instance, "Maps\\BusStop", busStopEdit1);
            LocationUtilities.RegisterMap(Instance, "Maps\\BusStop", busStopEdit2);
        }
    }
}
