namespace Farmhand.Events.Arguments.ControlEvents
{
    using System;

    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Arguments for KeyPressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPressedEventArgs"/> class.
        /// </summary>
        /// <param name="keyPressed">
        /// The key pressed.
        /// </param>
        public KeyPressedEventArgs(Keys keyPressed)
        {
            this.KeyPressed = keyPressed;
        }

        /// <summary>
        /// Gets the key pressed.
        /// </summary>
        public Keys KeyPressed { get; }
    }
}