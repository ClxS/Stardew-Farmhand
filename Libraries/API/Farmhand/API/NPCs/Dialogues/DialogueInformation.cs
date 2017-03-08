namespace Farmhand.API.NPCs.Dialogues
{
    using System.Collections.Generic;

    /// <summary>
    ///     Information about an NPCs dialogue.
    /// </summary>
    public class DialogueInformation
    {
        /// <summary>
        ///     Gets or sets the dialogue entries for this NPC.
        /// </summary>
        public List<DialogueEntry> DialogueEntries { get; set; }

        /// <summary>
        ///     Gets or sets the rainy dialogue.
        /// </summary>
        public DialogueEntry RainyDialogue { get; set; }

        /// <summary>
        ///     Constructs the dialogue information as a game compatible string.
        /// </summary>
        /// <returns>
        ///     The dialogue information as a string.
        /// </returns>
        public Dictionary<string, string> BuildBaseDialogues()
        {
            var ret = new Dictionary<string, string>();

            this.DialogueEntries.ForEach(dialogue => ret.Add(dialogue.DialogueId, dialogue.ToString()));
            return ret;
        }
    }
}