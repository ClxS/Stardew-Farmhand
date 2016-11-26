namespace Farmhand.API.NPCs.Dialogues
{
    public class DialogueEntry
    {
        public string DialogueId { get; set; }
        public string Message { get; set; }

        public DialogueEntry(string dialogueId, string message)
        {
            DialogueId = dialogueId;
            Message = message;
        }

        public override string ToString() => Message;
    }
}
