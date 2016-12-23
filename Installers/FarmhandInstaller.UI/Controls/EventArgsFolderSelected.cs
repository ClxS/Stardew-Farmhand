using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmhandInstaller.UI.Controls
{
    public class EventArgsFolderSelected : EventArgs
    {
        public EventArgsFolderSelected(string folder)
        {
            Folder = folder;
            Valid = true;
        }

        public string Folder { get; set; }

        public bool Valid { get; set; }

        public string ValidationFailureReason { get; set; }
    }
}
