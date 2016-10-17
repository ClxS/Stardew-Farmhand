using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Dialogues
{
    public class DialogueAnswerInformation
    {
        // The mod which owns this answer
        public Mod Owner { get; set; }

        // The key that identifies this answer
        public string Key { get; set; }

        // The text of this answer to display to the player
        public string Text { get; set; }

        // The information of the results of choosing this answer
        public DialogueResultInformation Result { get; set; }

        // The delegate called to decide if this answer should be included
        public Dialogue.IncludeAnswer DoInclude { get; set; } = new Dialogue.IncludeAnswer(DefaultResult);

        public DialogueAnswerInformation(Mod owner, string key, string text, DialogueResultInformation result)
        {
            Owner = owner;
            Key = key;
            Text = text;
            Result = result;
        }

        public DialogueAnswerInformation(Mod owner, string key, string text, DialogueResultInformation result, Dialogue.IncludeAnswer doInclude)
        {
            Owner = owner;
            Key = key;
            Text = text;
            Result = result;
            DoInclude = doInclude;
        }

        public string ModUniqueKey()
        {
            return $"{Owner.ModSettings.Name}/{Key}";
        }

        public StardewValley.Response ToResponse()
        {
            return new StardewValley.Response(ModUniqueKey(), Text);
        }

        // The default delegate call, if one was not provided
        public static bool DefaultResult()
        {
            return true;
        }
    }
}
