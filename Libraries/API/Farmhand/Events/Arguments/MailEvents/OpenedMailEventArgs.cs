namespace Farmhand.Events.Arguments.MailEvents
{
    using System;

    /// <summary>
    ///     Arguments for OpenedMail.
    /// </summary>
    public class OpenedMailEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenedMailEventArgs" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="mailTitle">
        ///     The mail title.
        /// </param>
        /// <param name="attachmentType">
        ///     The attachment type.
        /// </param>
        /// <param name="attachmentValue">
        ///     The attachment value.
        /// </param>
        public OpenedMailEventArgs(string message, string mailTitle, string attachmentType, string attachmentValue)
        {
            this.Message = message;
            this.MailTitle = mailTitle;
            this.AttachmentType = attachmentType;
            this.AttachmentValue = attachmentValue;
        }

        /// <summary>
        ///     Gets the message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gets the mail title.
        /// </summary>
        public string MailTitle { get; }

        /// <summary>
        ///     Gets the attachment type.
        /// </summary>
        public string AttachmentType { get; }

        /// <summary>
        ///     Gets the attachment value.
        /// </summary>
        public string AttachmentValue { get; }
    }
}