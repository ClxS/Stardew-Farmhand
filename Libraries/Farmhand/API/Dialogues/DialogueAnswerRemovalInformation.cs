namespace Farmhand.API.Dialogues
{
    /// <summary>
    ///     Information about a dialogue answer removal.
    /// </summary>
    public class DialogueAnswerRemovalInformation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueAnswerRemovalInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod which owns this answer removal.
        /// </param>
        /// <param name="key">
        ///     The key that identifies this answer.
        /// </param>
        /// <param name="doRemove">
        ///     The delegate called to decide if this answer should be removed.
        /// </param>
        public DialogueAnswerRemovalInformation(Mod owner, Answers key, Dialogue.RemoveAnswer doRemove)
        {
            this.Owner = owner;
            this.Key = key;
            this.DoRemove = doRemove;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueAnswerRemovalInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod which owns this answer removal.
        /// </param>
        /// <param name="key">
        ///     The key that identifies this answer.
        /// </param>
        public DialogueAnswerRemovalInformation(Mod owner, Answers key)
        {
            this.Owner = owner;
            this.Key = key;
        }

        /// <summary>
        ///     Gets or sets the mod which owns this answer removal.
        /// </summary>
        public Mod Owner { get; set; }

        /// <summary>
        ///     Gets or sets the key that identifies this answer.
        /// </summary>
        public Answers Key { get; set; }

        /// <summary>
        ///     Gets or sets the delegate called to decide if this answer should be removed.
        /// </summary>
        public Dialogue.RemoveAnswer DoRemove { get; set; } = DefaultResult;

        /// <summary>
        ///     The default delegate call, if one was not provided.
        /// </summary>
        /// <returns>
        ///     Always returns true.
        /// </returns>
        public static bool DefaultResult()
        {
            return true;
        }
    }
}