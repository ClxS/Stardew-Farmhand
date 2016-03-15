using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingMod
{
    class Logging : Revolution.Mod
    {
        public override string Name
        {
            get { return "Logging Mod"; }
        }

        public override string Authour
        {
            get { return "ClxS"; }
        }

        public override string Version
        {
            get { return "0.1"; }
        }

        public override string Description
        {
            get { return "Logs various events."; }
        }

        public override void Entry()
        {
            Revolution.Events.GameEvents.OnBeforeGameInitialised += OnGameInitialising;
            Revolution.Events.GameEvents.OnAfterGameInitialised += OnGameInitialised;
        }

        private void OnGameInitialising(object sender, EventArgs e)
        {
            Console.WriteLine("On Game Initialised");
        }

        private void OnGameInitialised(object sender, EventArgs e)
        {
            Console.WriteLine("On Game Done Initialising");
        }
    }
}
