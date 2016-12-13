using System.Collections.Generic;

namespace Farmhand.API.NPCs
{
    public class NPCDialoguesInformation
    {
        public List<DialogueInformation> DialogueInformation { get; set; }
        public DialogueInformation RainyDialogue { get; set; }

        public Dictionary<string, string> BuildBaseDialogues()
        {
            var ret = new Dictionary<string, string>();

            DialogueInformation.ForEach(dialogue => ret.Add(dialogue.DialogueId, dialogue.ToString()));
            return ret;
        }

        public string GetRainyDialogue => RainyDialogue.ToString();
    }

    public class DialogueInformation
    {
        public string DialogueId { get; set; }
        public string Message { get; set; }

        public DialogueInformation(string dialogueId, string message)
        {
            DialogueId = dialogueId;
            Message = message;
        }
        
        public override string ToString() => Message;
    }
}