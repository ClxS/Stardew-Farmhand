namespace Farmhand.Installers.Frames
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// The base frame.
    /// </summary>
    public abstract class BaseFrame : UserControl
    {
        internal event EventHandler<EventArgsFrameCommand> Navigate = delegate { };

        internal event EventHandler Back = delegate { };

        internal virtual void OnNavigate(string command)
        {
            this.Navigate(this, new EventArgsFrameCommand(command));
        }

        internal virtual void OnBack()
        {
            this.Back(this, EventArgs.Empty);
        }

        /// <summary>
        /// Clears frame information
        /// </summary>
        internal virtual void ClearFrame()
        {
        }

        /// <summary>
        /// Called when a frame has finished transitioning in.
        /// </summary>
        internal virtual void Start()
        {
        }
    }
}
