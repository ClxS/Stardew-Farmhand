using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Farmhand.Events;
using StardewValley;
using StardewValley.Menus;

namespace JuminoDepositAnywhereMod
{
    public class JuminoDepositAnywhere : Farmhand.Mod
    {
        public override void Entry()
        {
            GameEvents.AfterUpdateTick += GameEvents_OnAfterUpdateTick;
        }

        private void GameEvents_OnAfterUpdateTick(object sender, EventArgs e)
        {
            if (!Farmhand.API.Game.HasLoadedGame || Farmhand.API.Game.ActiveClickableMenu == null)
                return;
            
            var v = Farmhand.API.Game.ActiveClickableMenu as JunimoNoteMenu;

            var bundleField = v?.GetType().GetField("bundles", BindingFlags.Instance | BindingFlags.NonPublic);
            if (bundleField == null) return;

            var bndl = (List<Bundle>)bundleField.GetValue(v);

            foreach (var b in bndl.Where(b => !b.depositsAllowed))
            {
                b.depositsAllowed = true;
            }
        }
    }
}
