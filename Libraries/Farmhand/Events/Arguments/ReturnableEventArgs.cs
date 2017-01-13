namespace Farmhand.Events.Arguments
{
    using System;

    public class ReturnableEventArgs : EventArgs
    {
        public ReturnableEventArgs()
        {
            this.IsHandled = false;
        }

        public bool IsHandled { get; protected set; }
    }
}