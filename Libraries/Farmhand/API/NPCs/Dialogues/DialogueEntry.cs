namespace Farmhand.API.NPCs.Dialogues
{
    /// <summary>
    ///     A single dialogue entry.
    /// </summary>
    public class DialogueEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueEntry" /> class.
        /// </summary>
        /// <param name="dialogueId">
        ///     The dialogue entry ID.
        /// </param>
        /// <param name="message">
        ///     The message text.
        /// </param>
        public DialogueEntry(string dialogueId, string message)
        {
            this.DialogueId = dialogueId;
            this.Message = message;
        }

        /// <summary>
        ///     Gets or sets the dialogue ID.
        /// </summary>
        public string DialogueId { get; set; }

        /// <summary>
        ///     Gets or sets the message text.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Gets the dialogue text.
        /// </summary>
        /// <returns>
        ///     The dialogue text.
        /// </returns>
        public override string ToString() => this.Message;
    }
}