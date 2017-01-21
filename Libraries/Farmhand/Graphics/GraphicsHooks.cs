namespace Farmhand.Graphics
{
    using Farmhand.Attributes;

    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    internal static class GraphicsHooks
    {
        [Hook(HookType.Exit, "StardewValley.Game1", ".ctor")]
        internal static void SetHiDefProfile()
        {
            Game1.graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Game1.graphics.PreparingDeviceSettings +=
                (sender, args) =>
                    {
                        args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage =
                            RenderTargetUsage.PreserveContents;
                    };
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "Window_ClientSizeChanged")]
        [Hook(HookType.Exit, "StardewValley.Game1", "Initialize")]
        internal static void EnforcePreserveContents()
        {

        }
    }
}