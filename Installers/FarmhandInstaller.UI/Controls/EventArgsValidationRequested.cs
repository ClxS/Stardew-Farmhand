namespace Farmhand.Installers.Controls
{
    using System;

    internal class EventArgsValidationRequested : EventArgs
    {
        public EventArgsValidationRequested(string value)
        {
            this.Value = value;
            this.Valid = true;
        }

        public string Value { get; private set; }

        public bool Valid { get; set; }

        public string ValidationFailureReason { get; set; }
    }
}
