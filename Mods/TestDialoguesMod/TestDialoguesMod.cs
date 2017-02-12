using Farmhand;
using Farmhand.API.Dialogues;

namespace TestDialoguesMod
{
    public class TestDialoguesMod : Mod
    {
        public static TestDialoguesMod Instance;

        public override void Entry()
        {
            Instance = this;

            // Create a result, dictating what will happen if our answer is picked
            DialogueResultInformation newRobinResult = new DialogueResultInformation(Instance, new Dialogue.AnswerResult(RobinTeleport));
            // Create an answer, an option that will show up as a response to a question
            DialogueAnswerInformation newRobinAnswer = new DialogueAnswerInformation(Instance, "RobinTeleport", "Teleport", newRobinResult);
            // Register our new answer to be included in robins's question
            Dialogue.RegisterNewAnswer(Questions.Carpenter, newRobinAnswer);

            // Remove robin's "Leave" option
            DialogueAnswerRemovalInformation removeRobinLeave = new DialogueAnswerRemovalInformation(Instance, Answers.CarpenterLeave);
            Dialogue.RemoveDefaultAnswer(Questions.Carpenter, removeRobinLeave);
        }

        public static void RobinTeleport(ref bool doDefault)
        {
            Farmhand.API.Game.Player.Halt();
            Farmhand.API.Game.Player.freezePause = 700;
            StardewValley.Game1.warpFarmer("Town", 105, 80, 1);

            doDefault = true;
        }
    }
}
