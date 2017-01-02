namespace Farmhand.Installers.Controls
{
    using System;

    internal class EventArgsFolderSelected : EventArgs
    {
        public EventArgsFolderSelected(string folder)
        {
            this.Folder = folder;
            this.Valid = true;
        }

        public string Folder { get; private set; }

        public bool Valid { get; set; }

        public bool SuppressValidationError { get; set; } = false;

        public string ValidationFailureReason { get; set; }
    }
}
