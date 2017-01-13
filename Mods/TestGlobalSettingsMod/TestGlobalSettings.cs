namespace TestGlobalSettings
{
    using Farmhand;
    using Farmhand.Events;
    using Farmhand.Logging;

    using Microsoft.Xna.Framework.Input;

    internal class TestGlobalSettings : Mod
    {
        private readonly Settings settings = new Settings();

        public override ModSettings ConfigurationSettings => this.settings;

        public override void Entry()
        {
            ControlEvents.KeyPressed += ControlEvents_OnKeyPressed;
        }

        private void ControlEvents_OnKeyPressed(object sender, Farmhand.Events.Arguments.ControlEvents.KeyPressedEventArgs e)
        {
            if (e.KeyPressed == Keys.F7)
            {
                this.settings.TestStringGlobal = "Global";
                this.settings.Save();
            }
            else if (e.KeyPressed == Keys.F9)
            {
                Log.Verbose("GlobalSettingsTest: " + this.settings.TestStringGlobal);
            }
        }
    }
}
