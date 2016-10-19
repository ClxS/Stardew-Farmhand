using Farmhand.Attributes;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Dialogues
{
    public enum Questions
    {
        RemoveIncubatingEgg,        // RemoveIncubatingEgg
        StarTokenShopBuyTokens,     // StarTokenShop
        WheelBet,                   // wheelBet
        SlingshotGame,              // slingshotGame
        FishingGame,                // fishingGame
        FortuneTeller,              // fortuneTeller
        StarTokenShopTradeIn,       // starTokenShop
        SecretSanta,                // secretSanta
        Cave,                       // cave
        Pet,                        // pet
        TaxVote,                    // taxvote
        Eat,                        // Eat
        Shipping,                   // Shipping
        Marnie,                     // Marnie
        ResetMine,                  // ResetMine
        Blacksmith,                 // Blacksmith
        Carpenter,                  // carpenter
        Museum,                     // Museum
        HouseUpgrade,               // upgrade
        EvilShrineRightDeActivate,  // evilShrineRightDeActivate
        EvilShrineRightActivate,    // evilShrineRightActivate
        BuyJojaCola,                // buyJojaCola
        BuyQiClubCoins,             // BuyClubCoins
        BackpackUpgrade,            // Backpack
        WizardShrine,               // WizardShrine
        CalicoJackHS,               // CalicoJackHS
        CalicoJack,                 // CalicoJack
        BuyQiCoins,                 // BuyQiCoins
        DivorceCancel,              // divorceCancel
        Divorce,                    // divorce
        EvilShrineCenter,           // evilShrineCenter
        Minecart,                   // Minecart
        ClubSeller,                 // ClubSeller
        ExitMine,                   // ExitMine
        EvilShrineLeft,             // evilShrineLeft
        BeachBridge,                // BeachBridge
        Mariner,                    // mariner
        Bus,                        // Bus
        DesertBus,                  // DesertBus
        JojaSignUp,                 // JojaSignUp
        Shaft,                      // Shaft
        ClubCard,                   // ClubCard
        MinecartGame,               // MinecartGame
        //HaveBaby,                   // No string attached, behavior handled by delegate StardewValley.Events.QuestionEvent.answerPregnancyQuestion
        //SelectChannel,              // No string attached, behavior handled by delegate StardewValley.Objects.Tv.selectChannel
        Sleep,                      // Sleep
    }

    public enum Answers
    {
        Yes,                                        // Yes
        No,                                         // No
        Leave,                                      // Leave
        Shop,                                       // Shop
        Play,                                       // Play
        StarTokenShop_Buy,                          // Buy
        StarTokenShop_Leave,                        // Leave
        WheelBet_Orange,                            // Orange
        WheelBet_Green,                             // Green
        WheelBet_DontPlay,                          // I
        SlingshotGame_Play,                         // Play
        SlingshotGame_Leave,                        // Leave
        FishingGame_Play,                           // Play
        FishingGame_Leave,                          // Leave
        FortuneTeller_Read,                         // Read
        FortuneTeller_No,                           // No
        Cave_Mushrooms,                             // Mushrooms
        Cave_Bats,                                  // Bats
        TaxVote_For,                                // For
        TaxVote_Against,                            // Against
        Marnie_Supplies,                            // Supplies
        Marnie_Purchase,                            // Purchase
        Marnie_Leave,                               // Leave
        Blacksmith_Shop,                            // Shop
        Blacksmith_Upgrade,                         // Upgrade
        Blacksmith_Process,                         // Process
        Blacksmith_Leave,                           // Leave
        Carpenter_Shop,                             // Shop
        Carpenter_Upgrade,                          // Upgrade
        Carpenter_Construct,                        // Construct
        Carpenter_Leave,                            // Leave
        Museum_Donate,                              // Donate
        Museum_Collect,                             // Collect
        Museum_Leave,                               // Leave
        Backpack_Purchase,                          // Purchase
        Backpack_Not,                               // Not
        CalicoJack_Play,                            // Play
        CalicoJack_Leave,                           // Leave
        CalicoJack_Rules,                           // Rules
        MineCart_Town,                              // Town
        MineCart_Bus,                               // Bus
        MineCart_Quarry,                            // Quarry
        MineCart_Cancel,                            // Cancel
        ClubSeller_Ill,                             // I'll
        ClubSeller_No,                              // No
        ExitMine_Leave,                             // Leave
        ExitMine_DoNothing,                         // Do
        MinecartGame_Progress,                      // Progress
        MinecartGame_Endless,                       // Endless
        MinecartGame_Exit,                          // Exit
        Mariner_Buy,                                // Buy
        Mariner_Not,                                // Not
        DesertBus_Yes,                              // Yes
        DesertBus_Not,                              // Not
        Shaft_Jump,                                 // Jump
        Shaft_DoNothing,                            // Do
        ClubCard_Yes,                               // Yes.
        ClubCard_ThatsRight,                        // That's
        HaveBaby_Yes,                               // Yes
        HaveBaby_No,                                // Not
        SelectChannel_Weather,                      // Weather
        SelectChannel_Fortune,                      // Fortune
        SelectChannel_Livin,                        // Livin'
        SelectChannel_Queen,                        // The
        SelectChannel_Leave,                        // (Leave)
    }

    public class Dialogue
    {
        // The delegate called to decide if an answer should be included in a question
        public delegate bool IncludeAnswer();
        // The delegate called to decide if an answer should be removed from a question
        public delegate bool RemoveAnswer();
        // The delegate called when an answer is chosen
        public delegate void AnswerResult(ref bool doDefault);

        // Stores information for answers that are injected into existing questions, the key string being the key for the question
        private static Dictionary<string, List<DialogueAnswerInformation>> answerInjectors = new Dictionary<string, List<DialogueAnswerInformation>>();
        // Stores information for answers that should be removed from questions, the key string being the key for the question
        private static Dictionary<string, List<DialogueAnswerRemovalInformation>> answerRemovals = new Dictionary<string, List<DialogueAnswerRemovalInformation>>();
        // Stores information for new results for existing answers and injected answers, the key string being the key for the answer
        private static Dictionary<string, List<DialogueResultInformation>> resultInjectors = new Dictionary<string, List<DialogueResultInformation>>();
        // Stores information for injected questions
        private static Dictionary<string, DialogueQuestionInformation> injectedQuestions = new Dictionary<string, DialogueQuestionInformation>();

        /// <summary>
        /// Register a new answer which can be used in dialogs
        /// </summary>
        /// <param name="question">Question that this answer will be an answer to</param>
        /// <param name="answer">Information about this answer</param>
        public static void RegisterNewAnswer(Questions question, DialogueAnswerInformation answer)
        {
            RegisterNewAnswer(QuestionsKey(question), answer);
        }

        /// <summary>
        /// Register a new answer which can be used in dialogs
        /// </summary>
        /// <param name="question">Question that this answer will be an answer to</param>
        /// <param name="answer">Information about this answer</param>
        public static void RegisterNewAnswer(string question, DialogueAnswerInformation answer)
        {
            if (answerInjectors.ContainsKey(question))
            {
                answerInjectors[question].Add(answer);
            }
            else
            {
                List<DialogueAnswerInformation> answerList = new List<DialogueAnswerInformation>();
                answerList.Add(answer);
                answerInjectors.Add(question, answerList);

                // Make sure the result of our answer is injected
                RegisterNewResult(question, answer.ModUniqueKey(), answer.Result);
            }
        }

        /// <summary>
        /// Remove a default answer from a question
        /// </summary>
        /// <param name="question">Question to remove the answer from</param>
        /// <param name="answer">Information about the answer to remove</param>
        public static void RemoveDefaultAnswer(Questions question, DialogueAnswerRemovalInformation answer)
        {
            RemoveDefaultAnswer(QuestionsKey(question), answer);
        }

        /// <summary>
        /// Remove a default answer from a question
        /// </summary>
        /// <param name="question">Question to remove the answer from</param>
        /// <param name="answer">Information about the answer to remove</param>
        public static void RemoveDefaultAnswer(string question, DialogueAnswerRemovalInformation answer)
        {
            if (answerRemovals.ContainsKey(question))
            {
                answerRemovals[question].Add(answer);
            }
            else
            {
                List<DialogueAnswerRemovalInformation> answerList = new List<DialogueAnswerRemovalInformation>();
                answerList.Add(answer);
                answerRemovals.Add(question, answerList);
            }
        }

        /// <summary>
        /// Register a new result which can be used by answers, a result is what happens when an answer is selected
        /// </summary>
        /// <param name="question">The question that was asked to cause this result</param>
        /// <param name="answer">The answer that is selected to cause this result</param>
        /// <param name="result">Information about the result</param>
        public static void RegisterNewResult(Questions question, Answers answer, DialogueResultInformation result)
        {
            RegisterNewResult(QuestionsKey(question), AnswersKey(answer), result);
        }

        /// <summary>
        /// Register a new result which can be used by answers, a result is what happens when an answer is selected
        /// </summary>
        /// <param name="question">The question that was asked to cause this result</param>
        /// <param name="answer">The answer that is selected to cause this result</param>
        /// <param name="result">Information about the result</param>
        public static void RegisterNewResult(string question, string answer, DialogueResultInformation result)
        {
            if (resultInjectors.ContainsKey(GetQuestionAndAnswerString(question, answer)))
            {
                resultInjectors[GetQuestionAndAnswerString(question, answer)].Add(result);
            }
            else
            {
                List<DialogueResultInformation> resultList = new List<DialogueResultInformation>();
                resultList.Add(result);
                resultInjectors.Add(GetQuestionAndAnswerString(question, answer), resultList);
            }
        }

        /// <summary>
        /// Register a new question which can be posed to the player through a dialogue
        /// </summary>
        /// <param name="question">Information about the question</param>
        public static void RegisterQuestion(DialogueQuestionInformation question)
        {
            injectedQuestions.Add(question.ModUniqueKey(), question);
        }

        /// <summary>
        /// Open a dialogue window with a question for the player to answer
        /// </summary>
        /// <param name="question">Information about the question that the dialogue window will be opened with</param>
        public static void OpenQuestion(DialogueQuestionInformation question)
        {
            // Check to make sure the question has been registered
            if (injectedQuestions.ContainsKey(question.ModUniqueKey()))
            {
                Response[] responses = GetResponseArray(question.Choices);

                if(responses.Length == 0)
                {
                    // I'm really not sure what this would do, so I'm leaving it as a warning for now. If it causes some kind of crash or problem, this might have to get upgraded to an exception
                    Farmhand.Logging.Log.Warning($"Question {question.Key} in mod {question.Owner.ModSettings.Name} is attempting to open a question with no possible answers!");
                }

                Game1.currentLocation.createQuestionDialogue(question.Question, responses, question.ModUniqueKey());
            }
            else
            {
                // This question was never registered, but since we have all the information, we can do it for the mod real fast, then re-call OpenQuestion
                RegisterQuestion(question);
                OpenQuestion(question);
            }
        }

        /// <summary>
        /// Open a dialogue window that informs the player of something
        /// </summary>
        /// <param name="statement">Text to put in the dialogue window</param>
        public static void OpenStatement(string statement)
        {
            Game1.drawObjectDialogue(statement);
        }






        // Injects the create question dialog call
        [Hook(HookType.Entry, "StardewValley.GameLocation", "System.Void StardewValley.GameLocation::createQuestionDialogue(System.String,StardewValley.Response[],System.String)")]
        [Hook(HookType.Entry, "StardewValley.GameLocation", "System.Void StardewValley.GameLocation::createQuestionDialogue(System.String,StardewValley.Response[],System.String,StardewValley.Object)")]
        internal static void injectQuestionDialgueOverload1(
            [InputBind(typeof(StardewValley.Response[]), "answerChoices", true)] ref StardewValley.Response[] answerChoices,
            [InputBind(typeof(string), "dialogKey")] string dialogKey)
        {
            answerChoices = InjectQuestionDialog(answerChoices, dialogKey);
        }

        // Injects where answers are used, in order to inject answer results
        [HookReturnable(HookType.Entry, "StardewValley.GameLocation", "System.Boolean StardewValley.GameLocation::answerDialogue(StardewValley.Response)")]
        [HookReturnable(HookType.Entry, "StardewValley.Locations.BusStop", "System.Boolean StardewValley.Locations.BusStop::answerDialogue(StardewValley.Response)")]
        [HookReturnable(HookType.Entry, "StardewValley.Locations.Desert", "System.Boolean StardewValley.Locations.Desert::answerDialogue(StardewValley.Response)")]
        [HookReturnable(HookType.Entry, "StardewValley.Locations.JojaMart", "System.Boolean StardewValley.Locations.JojaMart::answerDialogue(StardewValley.Response)")]
        internal static bool answerResultOverride2(
            [UseOutputBind] ref bool useOutput,
            [ThisBind] GameLocation @this,
            [InputBind(typeof(Response), "answer")] Response answer)
        {
            string questionKey = GetLastQuestionKey(@this);
            string answerKey = answer.responseKey;

            bool doDefault = DoInjectedResults(questionKey, answerKey);

            useOutput = !doDefault;
            return !doDefault;
        }

        // Inject answers into a question dialog
        private static Response[] InjectQuestionDialog(Response[] answerChoices, string dialogKey)
        {
            // The list that will eventually become the responses array
            List<Response> finalResponses = new List<Response>();

            // Remove answers registered to be removed
            if (answerRemovals.ContainsKey(dialogKey))
            {
                foreach (DialogueAnswerRemovalInformation answerRemoval in answerRemovals[dialogKey])
                {
                    for (int i = 0; i < answerChoices.Length; i++)
                    {
                        // Check if the keys match, and if the answer should be removed
                        if (AnswersKey(answerRemoval.Key).Equals(answerChoices[i].responseKey) && answerRemoval.Owner.ModSettings.ModState == ModState.Loaded && answerRemoval.DoRemove())
                        {
                            // Set the corresponding value in the array to be null
                            answerChoices[i] = null;
                        }
                    }
                }
            }

            // Add any non-null values in the array into the finalResponses list
            for (int i = 0; i < answerChoices.Length; i++)
            {
                if (answerChoices[i] != null)
                {
                    finalResponses.Add(answerChoices[i]);
                }
            }

            // Now add any registered answers for this question
            if (answerInjectors.ContainsKey(dialogKey))
            {
                foreach (DialogueAnswerInformation answer in answerInjectors[dialogKey])
                {
                    if (answer.Owner.ModSettings.ModState == ModState.Loaded && answer.DoInclude())
                    {
                        // Add the answer to the final list
                        finalResponses.Add(answer.ToResponse());
                    }
                }
            }

            // Return the final responses
            return finalResponses.ToArray();
        }

        // Uses any results that match the keys given for a question and answer
        private static bool DoInjectedResults(string questionKey, string answerKey)
        {
            bool doDefault = true;

            // Check injected results
            if (questionKey != null)
            {
                if (resultInjectors.ContainsKey(GetQuestionAndAnswerString(questionKey, answerKey)))
                {
                    foreach (var result in resultInjectors[GetQuestionAndAnswerString(questionKey, answerKey)])
                    {
                        if (result.Owner.ModSettings.ModState == ModState.Loaded)
                        {
                            bool resultDoDefault = true;
                            result.Result(ref resultDoDefault);

                            if (resultDoDefault == false)
                            {
                                doDefault = false;
                            }
                        }
                    }
                }

                // Check injected questions for possible answers with results
                foreach (var injectedQuestion in injectedQuestions)
                {
                    // Check each of its answers
                    foreach (var answerInformation in injectedQuestion.Value.Choices)
                    {
                        // Check that this was the answer given
                        if (answerInformation.ModUniqueKey().Equals(answerKey))
                        {
                            if (answerInformation.Owner.ModSettings.ModState == ModState.Loaded)
                            {
                                bool resultDoDefault = true;
                                answerInformation.Result.Result(ref resultDoDefault);

                                if (resultDoDefault == false)
                                {
                                    doDefault = false;
                                }
                            }
                        }
                    }
                }
            }

            return doDefault;
        }

        // Either returns the key for the last question asked in a game location, or null
        private static string GetLastQuestionKey(GameLocation location)
        {
            return (location.lastQuestionKey != null) ? location.lastQuestionKey.Split(' ')[0] : null;
        }

        // Get the question and answer string for a question key and answer key
        private static string GetQuestionAndAnswerString(string questionKey, string answerKey)
        {
            return $"{questionKey}_{answerKey}";
        }

        // Get key from Questions
        public static string QuestionsKey(Questions question)
        {
            switch (question)
            {
                case Questions.RemoveIncubatingEgg:         { return "RemoveIncubatingEgg"; }
                case Questions.StarTokenShopBuyTokens:      { return "StarTokenShop"; }
                case Questions.WheelBet:                    { return "wheelBet"; }
                case Questions.SlingshotGame:               { return "slingshotGame"; }
                case Questions.FishingGame:                 { return "fishingGame"; }
                case Questions.FortuneTeller:               { return "fortuneTeller"; }
                case Questions.StarTokenShopTradeIn:        { return "starTokenShop"; }
                case Questions.SecretSanta:                 { return "secretSanta"; }
                case Questions.Cave:                        { return "cave"; }
                case Questions.Pet:                         { return "pet"; }
                case Questions.TaxVote:                     { return "taxvote"; }
                case Questions.Eat:                         { return "Eat"; }
                case Questions.Shipping:                    { return "Shipping"; }
                case Questions.Marnie:                      { return "Marnie"; }
                case Questions.ResetMine:                   { return "ResetMine"; }
                case Questions.Blacksmith:                  { return "Blacksmith"; }
                case Questions.Carpenter:                   { return "carpenter"; }
                case Questions.Museum:                      { return "Museum"; }
                case Questions.HouseUpgrade:                { return "upgrade"; }
                case Questions.EvilShrineRightDeActivate:   { return "evilShrineRightDeActivate"; }
                case Questions.EvilShrineRightActivate:     { return "evilShrineRightActivate"; }
                case Questions.BuyJojaCola:                 { return "buyJojaCola"; }
                case Questions.BuyQiClubCoins:              { return "BuyClubCoins"; }
                case Questions.BackpackUpgrade:             { return "Backpack"; }
                case Questions.WizardShrine:                { return "WizardShrine"; }
                case Questions.CalicoJackHS:                { return "CalicoJackHS"; }
                case Questions.CalicoJack:                  { return "CalicoJack"; }
                case Questions.BuyQiCoins:                  { return "BuyQiCoins"; }
                case Questions.DivorceCancel:               { return "divorceCancel"; }
                case Questions.Divorce:                     { return "divorce"; }
                case Questions.EvilShrineCenter:            { return "evilShrineCenter"; }
                case Questions.Minecart:                    { return "Minecart"; }
                case Questions.ClubSeller:                  { return "ClubSeller"; }
                case Questions.ExitMine:                    { return "ExitMine"; }
                case Questions.EvilShrineLeft:              { return "evilShrineLeft"; }
                case Questions.BeachBridge:                 { return "BeachBridge"; }
                case Questions.Mariner:                     { return "mariner"; }
                case Questions.Bus:                         { return "Bus"; }
                case Questions.DesertBus:                   { return "DesertBus"; }
                case Questions.JojaSignUp:                  { return "JojaSignUp"; }
                case Questions.Shaft:                       { return "Shaft"; }
                case Questions.ClubCard:                    { return "ClubCard"; }
                case Questions.MinecartGame:                { return "MinecartGame"; }
                //case Questions.HaveBaby:                    { return ""; } // Handled by delegate
                //case Questions.SelectChannel:               { return ""; } // Handled by delegate
                case Questions.Sleep:                       { return "Sleep"; }
            }

            return "";
        }

        // Get key from Answers
        public static string AnswersKey(Answers answer)
        {
            switch (answer)
            {
                case Answers.Yes:                       { return "Yes"; }
                case Answers.No:                        { return "No"; }
                case Answers.Leave:                     { return "Leave"; }
                case Answers.Shop:                      { return "Shop"; }
                case Answers.Play:                      { return "Play"; }
                case Answers.StarTokenShop_Buy:         { return "Buy"; }
                case Answers.StarTokenShop_Leave:       { return "Leave"; }
                case Answers.WheelBet_Orange:           { return "Orange"; }
                case Answers.WheelBet_Green:            { return "Green"; }
                case Answers.WheelBet_DontPlay:         { return "I"; }
                case Answers.SlingshotGame_Play:        { return "Play"; }
                case Answers.SlingshotGame_Leave:       { return "Leave"; }
                case Answers.FishingGame_Play:          { return "Play"; }
                case Answers.FishingGame_Leave:         { return "Leave"; }
                case Answers.FortuneTeller_Read:        { return "Read"; }
                case Answers.FortuneTeller_No:          { return "No"; }
                case Answers.Cave_Mushrooms:            { return "Mushrooms"; }
                case Answers.Cave_Bats:                 { return "Bats"; }
                case Answers.TaxVote_For:               { return "For"; }
                case Answers.TaxVote_Against:           { return "Against"; }
                case Answers.Marnie_Supplies:           { return "Supplies"; }
                case Answers.Marnie_Purchase:           { return "Purchase"; }
                case Answers.Marnie_Leave:              { return "Leave"; }
                case Answers.Blacksmith_Shop:           { return "Shop"; }
                case Answers.Blacksmith_Upgrade:        { return "Upgrade"; }
                case Answers.Blacksmith_Process:        { return "Process"; }
                case Answers.Blacksmith_Leave:          { return "Leave"; }
                case Answers.Carpenter_Shop:            { return "Shop"; }
                case Answers.Carpenter_Upgrade:         { return "Upgrade"; }
                case Answers.Carpenter_Construct:       { return "Construct"; }
                case Answers.Carpenter_Leave:           { return "Leave"; }
                case Answers.Museum_Donate:             { return "Donate"; }
                case Answers.Museum_Collect:            { return "Collect"; }
                case Answers.Museum_Leave:              { return "Leave"; }
                case Answers.Backpack_Purchase:         { return "Purchase"; }
                case Answers.Backpack_Not:              { return "Not"; }
                case Answers.CalicoJack_Play:           { return "Play"; }
                case Answers.CalicoJack_Leave:          { return "Leave"; }
                case Answers.CalicoJack_Rules:          { return "Rules"; }
                case Answers.MineCart_Town:             { return "Town"; }
                case Answers.MineCart_Bus:              { return "Bus"; }
                case Answers.MineCart_Quarry:           { return "Quarry"; }
                case Answers.MineCart_Cancel:           { return "Cancel"; }
                case Answers.ClubSeller_Ill:            { return "I'll"; }
                case Answers.ClubSeller_No:             { return "No"; }
                case Answers.ExitMine_Leave:            { return "Leave"; }
                case Answers.ExitMine_DoNothing:        { return "Do"; }
                case Answers.MinecartGame_Progress:     { return "Progress"; }
                case Answers.MinecartGame_Endless:      { return "Endless"; }
                case Answers.MinecartGame_Exit:         { return "Exit"; }
                case Answers.Mariner_Buy:               { return "Buy"; }
                case Answers.Mariner_Not:               { return "Not"; }
                case Answers.DesertBus_Yes:             { return "Yes"; }
                case Answers.DesertBus_Not:             { return "Not"; }
                case Answers.Shaft_Jump:                { return "Jump"; }
                case Answers.Shaft_DoNothing:           { return "Do"; }
                case Answers.ClubCard_Yes:              { return "Yes."; }
                case Answers.ClubCard_ThatsRight:       { return "That's"; }
                case Answers.HaveBaby_Yes:              { return "Yes"; }
                case Answers.HaveBaby_No:               { return "Not"; }
                case Answers.SelectChannel_Weather:     { return "Weather"; }
                case Answers.SelectChannel_Fortune:     { return "Fortune"; }
                case Answers.SelectChannel_Livin:       { return "Livin'"; }
                case Answers.SelectChannel_Queen:       { return "The"; }
                case Answers.SelectChannel_Leave:       { return "(Leave)"; }
            }

            return "";
        }

        // Get a StardewValley.Response array from a list of DialogueAnswerInformation
        public static Response[] GetResponseArray(List<DialogueAnswerInformation> answers)
        {
            List<Response> responseList = new List<Response>();

            foreach (DialogueAnswerInformation answer in answers)
            {
                if (answer.DoInclude())
                {
                    responseList.Add(answer.ToResponse());
                }
            }

            return responseList.ToArray();
        }
    }
}
