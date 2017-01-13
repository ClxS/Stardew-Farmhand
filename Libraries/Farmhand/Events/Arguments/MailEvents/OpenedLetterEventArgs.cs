namespace Farmhand.Events.Arguments.MailEvents
{
    using System;

    /// <summary>
    ///     Arguments for OpenedLetter.
    /// </summary>
    public class OpenedLetterEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenedLetterEventArgs" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        public OpenedLetterEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        ///     Gets the message.
        /// </summary>
        public string Message { get; }
    }
}