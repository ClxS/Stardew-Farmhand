using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Dialogues
{
    public class DialogueAnswerRemovalInformation
    {
        // The mod which owns this answer removal
        public Mod Owner { get; set; }

        // The key that identifies this answer
        public Answers Key { get; set; }

        // The delegate called to decide if this answer should be removed
        public Dialogue.RemoveAnswer DoRemove { get; set; } = new Dialogue.RemoveAnswer(DefaultResult);

        public DialogueAnswerRemovalInformation(Mod owner, Answers key, Dialogue.RemoveAnswer doRemove)
        {
            Owner = owner;
            Key = key;
            DoRemove = doRemove;
        }

        public DialogueAnswerRemovalInformation(Mod owner, Answers key)
        {
            Owner = owner;
            Key = key;
        }

        // The default delegate call, if one was not provided
        public static bool DefaultResult()
        {
            return true;
        }
    }
}
