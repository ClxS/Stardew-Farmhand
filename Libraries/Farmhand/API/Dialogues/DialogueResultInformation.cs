using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Dialogues
{
    public class DialogueResultInformation
    {
        // Mod which owns this dialogue result
        public Mod Owner { get; set; }

        // The delegate called when this result is chosen
        public Dialogue.AnswerResult Result { get; set; } = new Dialogue.AnswerResult(DefaultResult);

        public DialogueResultInformation(Mod owner, Dialogue.AnswerResult result)
        {
            Owner = owner;
            Result = result;
        }

        // The default delegate call, if for some reason one was not chosen
        public static void DefaultResult(ref bool doDefault)
        {
            Farmhand.Logging.Log.Warning($"Delegate was never given for a dialogue result");
        }
    }
}
