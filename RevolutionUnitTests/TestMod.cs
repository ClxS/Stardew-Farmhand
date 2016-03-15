using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution
{
    public class TestMod
    {
        public void Entry()
        {
            Events.GameEvents.OnBeforeGameInitialised += OnGameInitialising;
            Events.GameEvents.OnAfterGameInitialised += OnGameInitialised;
        }

        private void OnGameInitialising(object sender, EventArgs e)
        {

        }

        private void OnGameInitialised(object sender, EventArgs e)
        {

        }
    }
}
