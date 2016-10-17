using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Dialogues
{
    public class DialogueQuestionInformation
    {
        // The mod which owns this question
        public Mod Owner { get; set; }

        // The list of choices the player has to answer from
        public List<DialogueAnswerInformation> Choices { get; set; }

        // The text of the question that is displayed to the player
        public string Question { get; set; }

        // The string Key which identifies which question this is
        public string Key { get; set; }

        public DialogueQuestionInformation(Mod owner, List<DialogueAnswerInformation> choices, string question, string key)
        {
            this.Owner = owner;
            this.Choices = choices;
            this.Question = question;
            this.Key = key;
        }

        public DialogueQuestionInformation(Mod owner, DialogueAnswerInformation[] choices, string question, string key)
        {
            this.Owner = owner;
            this.Choices = choices.ToList();
            this.Question = question;
            this.Key = key;
        }

        public string ModUniqueKey()
        {
            return $"{Owner.ModSettings.Name}/{Key}";
        }
    }
}
