namespace SmapiCompatibilityLayer
{
    using StardewModdingAPI.Inheritance;

    internal class SmapiGameOverride : SGame
    {
        public SmapiGameOverride()
            : base(CompatibilityLayer.Monitor)
        {
        }
    }
}
