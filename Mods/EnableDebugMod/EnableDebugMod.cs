using Farmhand;
using Farmhand.Events;
using Farmhand.Events.Arguments.ControlEvents;
using Microsoft.Xna.Framework.Input;
using StardewValley;

namespace EnableDebugMod
{
    public class EnableDebugMod : Mod
    {
        public static string lastDebugInput;

        public override void Entry()
        {
            ControlEvents.OnKeyPressed += ControlEvents_OnKeyPressed;
        }

        private void ControlEvents_OnKeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (Game1.paused)
                return;

            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.F8) && !Game1.oldKBState.IsKeyDown(Keys.F8))
                Game1.game1.requestDebugInput();
        }
    }
}
