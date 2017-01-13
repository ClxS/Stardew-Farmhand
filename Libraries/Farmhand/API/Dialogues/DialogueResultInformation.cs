namespace Farmhand.API.Dialogues
{
    using Farmhand.Logging;

    /// <summary>
    ///     Information about a dialogue result.
    /// </summary>
    public class DialogueResultInformation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueResultInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod which owns this dialogue result.
        /// </param>
        /// <param name="result">
        ///     The delegate called when this result is chosen.
        /// </param>
        public DialogueResultInformation(Mod owner, Dialogue.AnswerResult result)
        {
            this.Owner = owner;
            this.Result = result;
        }

        /// <summary>
        ///     Gets or sets the mod which owns this dialogue result.
        /// </summary>
        public Mod Owner { get; set; }

        /// <summary>
        ///     Gets or sets the delegate called when this result is chosen.
        /// </summary>
        public Dialogue.AnswerResult Result { get; set; }

        /// <summary>
        ///     The default delegate call, if for some reason one was not chosen.
        /// </summary>
        /// <param name="doDefault">
        ///     Unused parameter
        /// </param>
        /// <remarks>
        ///     Always logs a warning stating: "Delegate was never given for a dialogue result"
        /// </remarks>
        public static void DefaultResult(ref bool doDefault)
        {
            Log.Warning("Delegate was never given for a dialogue result");
        }
    }
}