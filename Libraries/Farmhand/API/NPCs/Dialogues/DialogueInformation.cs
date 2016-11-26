using System.Collections.Generic;

namespace Farmhand.API.NPCs.Dialogues
{
    public class DialogueInformation
    {
        public List<DialogueEntry> DialogueEntries { get; set; }
        public DialogueEntry RainyDialogue { get; set; }

        public Dictionary<string, string> BuildBaseDialogues()
        {
            var ret = new Dictionary<string, string>();

            DialogueEntries.ForEach(dialogue => ret.Add(dialogue.DialogueId, dialogue.ToString()));
            return ret;
        }

        public string GetRainyDialogue => RainyDialogue.ToString();
    }
}
