namespace Farmhand.API.Dialogues
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Information about a dialogue question.
    /// </summary>
    public class DialogueQuestionInformation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueQuestionInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod which owns this question.
        /// </param>
        /// <param name="choices">
        ///     The list of choices the player has to answer from.
        /// </param>
        /// <param name="question">
        ///     The text of the question that is displayed to the player.
        /// </param>
        /// <param name="key">
        ///     The key which identifies which question this is.
        /// </param>
        public DialogueQuestionInformation(
            Mod owner,
            List<DialogueAnswerInformation> choices,
            string question,
            string key)
        {
            this.Owner = owner;
            this.Choices = choices;
            this.Question = question;
            this.Key = key;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogueQuestionInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod which owns this question.
        /// </param>
        /// <param name="choices">
        ///     The array of choices the player has to answer from.
        /// </param>
        /// <param name="question">
        ///     The text of the question that is displayed to the player.
        /// </param>
        /// <param name="key">
        ///     The key which identifies which question this is.
        /// </param>
        public DialogueQuestionInformation(Mod owner, DialogueAnswerInformation[] choices, string question, string key)
        {
            this.Owner = owner;
            this.Choices = choices.ToList();
            this.Question = question;
            this.Key = key;
        }

        /// <summary>
        ///     Gets or sets the mod which owns this question.
        /// </summary>
        public Mod Owner { get; set; }

        /// <summary>
        ///     Gets or sets the list of choices the player has to answer from.
        /// </summary>
        public List<DialogueAnswerInformation> Choices { get; set; }

        /// <summary>
        ///     Gets or sets the text of the question that is displayed to the player.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        ///     Gets or sets the key which identifies which question this is.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Returns the mod-unique key for this question.
        /// </summary>
        /// <returns>
        ///     The mod-specific key as a <see cref="string" />.
        /// </returns>
        public string ModUniqueKey()
        {
            return $"{this.Owner.ModSettings.Name}/{this.Key}";
        }
    }
}