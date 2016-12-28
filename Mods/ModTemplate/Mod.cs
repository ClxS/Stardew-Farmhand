namespace ModTemplate
{
    using Farmhand;

    internal class Mod : Farmhand.Mod
    {
        #region ModSettings
        private readonly Settings settings = new Settings();

        public override ModSettings ConfigurationSettings => this.settings;
        #endregion

        public override void Entry()
        {
            // Add your entry logic here
        }
    }
}
