namespace Farmhand.API.Dialogues
{
    using StardewValley;

    /// <summary>
    ///     Information about a dialogue answer.
    /// </summary>
    public class DialogueAnswerInformation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueAnswerInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owning mod.
        /// </param>
        /// <param name="key">
        ///     The key that identifies this answer.
        /// </param>
        /// <param name="text">
        ///     The text for this answer.
        /// </param>
        /// <param name="result">
        ///     The answer result information.
        /// </param>
        public DialogueAnswerInformation(Mod owner, string key, string text, DialogueResultInformation result)
        {
            this.Owner = owner;
            this.Key = key;
            this.Text = text;
            this.Result = result;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueAnswerInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owning mod.
        /// </param>
        /// <param name="key">
        ///     The key that identifies this answer.
        /// </param>
        /// <param name="text">
        ///     The text for this answer.
        /// </param>
        /// <param name="result">
        ///     The answer result information.
        /// </param>
        /// <param name="doInclude">
        ///     The delegate used to decide if this answer should be used.
        /// </param>
        public DialogueAnswerInformation(
            Mod owner,
            string key,
            string text,
            DialogueResultInformation result,
            Dialogue.IncludeAnswer doInclude)
        {
            this.Owner = owner;
            this.Key = key;
            this.Text = text;
            this.Result = result;
            this.DoInclude = doInclude;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueAnswerInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owning mod.
        /// </param>
        /// <param name="key">
        ///     The key that identifies this answer.
        /// </param>
        /// <param name="text">
        ///     The text for this answer.
        /// </param>
        /// <param name="result">
        ///     The answer result information.
        /// </param>
        public DialogueAnswerInformation(Mod owner, Answers key, string text, DialogueResultInformation result)
        {
            this.Owner = owner;
            this.Key = Dialogue.AnswersKey(key);
            this.Text = text;
            this.Result = result;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueAnswerInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owning mod.
        /// </param>
        /// <param name="key">
        ///     The key that identifies this answer.
        /// </param>
        /// <param name="text">
        ///     The text for this answer.
        /// </param>
        /// <param name="result">
        ///     The answer result information.
        /// </param>
        /// <param name="doInclude">
        ///     The delegate used to decide if this answer should be used.
        /// </param>
        public DialogueAnswerInformation(
            Mod owner,
            Answers key,
            string text,
            DialogueResultInformation result,
            Dialogue.IncludeAnswer doInclude)
        {
            this.Owner = owner;
            this.Key = Dialogue.AnswersKey(key);
            this.Text = text;
            this.Result = result;
            this.DoInclude = doInclude;
        }

        /// <summary>
        ///     Gets or sets the mod which owns this answer.
        /// </summary>
        public Mod Owner { get; set; }

        /// <summary>
        ///     Gets or sets the key that identifies this answer.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Gets or sets the text of this answer to display to the player.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Gets or sets the information of the results of choosing this answer.
        /// </summary>
        public DialogueResultInformation Result { get; set; }

        /// <summary>
        ///     Gets or sets the delegate called to decide if this answer should be included.
        /// </summary>
        public Dialogue.IncludeAnswer DoInclude { get; set; } = DefaultResult;

        /// <summary>
        ///     The default delegate used for <see cref="Result" /> if one was not provided.
        /// </summary>
        /// <returns>
        ///     Always returns true.
        /// </returns>
        public static bool DefaultResult()
        {
            return true;
        }

        /// <summary>
        ///     Gets the mod-unique key for this answer.
        /// </summary>
        /// <returns>
        ///     The mod-specific key as a <see cref="string" />.
        /// </returns>
        public string ModUniqueKey()
        {
            return $"{this.Owner.ModSettings.Name}/{this.Key}";
        }

        /// <summary>
        ///     Converts this answer to a response.
        /// </summary>
        /// <returns>
        ///     The <see cref="Response" />.
        /// </returns>
        public Response ToResponse()
        {
            return new Response(this.ModUniqueKey(), this.Text);
        }
    }
}