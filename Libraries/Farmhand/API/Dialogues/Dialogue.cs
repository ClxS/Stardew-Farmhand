namespace Farmhand.API.Dialogues
{
    //// ReSharper disable StyleCop.SA1602
    using System.Collections.Generic;

    using Farmhand.Attributes;
    using Farmhand.Logging;

    using StardewValley;

    /// <summary>
    ///     Enumeration of possible questions.
    /// </summary>
    public enum Questions
    {
        RemoveIncubatingEgg, // RemoveIncubatingEgg

        StarTokenShopBuyTokens, // StarTokenShop

        WheelBet, // wheelBet

        SlingshotGame, // slingshotGame

        FishingGame, // fishingGame

        FortuneTeller, // fortuneTeller

        StarTokenShopTradeIn, // starTokenShop

        SecretSanta, // secretSanta

        Cave, // cave

        Pet, // pet

        TaxVote, // taxvote

        Eat, // Eat

        Shipping, // Shipping

        Marnie, // Marnie

        ResetMine, // ResetMine

        Blacksmith, // Blacksmith

        Carpenter, // carpenter

        Museum, // Museum

        HouseUpgrade, // upgrade

        EvilShrineRightDeActivate, // evilShrineRightDeActivate

        EvilShrineRightActivate, // evilShrineRightActivate

        BuyJojaCola, // buyJojaCola

        BuyQiClubCoins, // BuyClubCoins

        BackpackUpgrade, // Backpack

        WizardShrine, // WizardShrine

        CalicoJackHs, // CalicoJackHS

        CalicoJack, // CalicoJack

        BuyQiCoins, // BuyQiCoins

        DivorceCancel, // divorceCancel

        Divorce, // divorce

        EvilShrineCenter, // evilShrineCenter

        Minecart, // Minecart

        ClubSeller, // ClubSeller

        ExitMine, // ExitMine

        EvilShrineLeft, // evilShrineLeft

        BeachBridge, // BeachBridge

        Mariner, // mariner

        Bus, // Bus

        DesertBus, // DesertBus

        JojaSignUp, // JojaSignUp

        Shaft, // Shaft

        ClubCard, // ClubCard

        MinecartGame, // MinecartGame
        // HaveBaby,                   // No string attached, behavior handled by delegate StardewValley.Events.QuestionEvent.answerPregnancyQuestion
        // SelectChannel,              // No string attached, behavior handled by delegate StardewValley.Objects.Tv.selectChannel
        Sleep // Sleep
    }

    /// <summary>
    ///     Enumeration of possible answers.
    /// </summary>
    public enum Answers
    {
        Yes, // Yes

        No, // No

        Leave, // Leave

        Shop, // Shop

        Play, // Play

        StarTokenShopBuy, // Buy

        StarTokenShopLeave, // Leave

        WheelBetOrange, // Orange

        WheelBetGreen, // Green

        WheelBetDontPlay, // I

        SlingshotGamePlay, // Play

        SlingshotGameLeave, // Leave

        FishingGamePlay, // Play

        FishingGameLeave, // Leave

        FortuneTellerRead, // Read

        FortuneTellerNo, // No

        CaveMushrooms, // Mushrooms

        CaveBats, // Bats

        TaxVoteFor, // For

        TaxVoteAgainst, // Against

        MarnieSupplies, // Supplies

        MarniePurchase, // Purchase

        MarnieLeave, // Leave

        BlacksmithShop, // Shop

        BlacksmithUpgrade, // Upgrade

        BlacksmithProcess, // Process

        BlacksmithLeave, // Leave

        CarpenterShop, // Shop

        CarpenterUpgrade, // Upgrade

        CarpenterConstruct, // Construct

        CarpenterLeave, // Leave

        MuseumDonate, // Donate

        MuseumCollect, // Collect

        MuseumLeave, // Leave

        BackpackPurchase, // Purchase

        BackpackNot, // Not

        CalicoJackPlay, // Play

        CalicoJackLeave, // Leave

        CalicoJackRules, // Rules

        MineCartTown, // Town

        MineCartBus, // Bus

        MineCartQuarry, // Quarry

        MineCartCancel, // Cancel

        ClubSellerIll, // I'll

        ClubSellerNo, // No

        ExitMineLeave, // Leave

        ExitMineDoNothing, // Do

        MinecartGameProgress, // Progress

        MinecartGameEndless, // Endless

        MinecartGameExit, // Exit

        MarinerBuy, // Buy

        MarinerNot, // Not

        DesertBusYes, // Yes

        DesertBusNot, // Not

        ShaftJump, // Jump

        ShaftDoNothing, // Do

        ClubCardYes, // Yes.

        ClubCardThatsRight, // That's

        HaveBabyYes, // Yes

        HaveBabyNo, // Not

        SelectChannelWeather, // Weather

        SelectChannelFortune, // Fortune

        SelectChannelLivin, // Livin'

        SelectChannelQueen, // The

        SelectChannelLeave // (Leave)
    }

    /// <summary>
    ///     Dialogue-related API functionality.
    /// </summary>
    public static class Dialogue
    {
        #region Delegates

        /// <summary>
        ///     The delegate called when an answer is chosen.
        /// </summary>
        /// <param name="doDefault">
        ///     Whether the game should do the default answer.
        /// </param>
        public delegate void AnswerResult(ref bool doDefault);

        /// <summary>
        ///     The delegate called to decide if an answer should be included in a question.
        /// </summary>
        /// <returns>
        ///     Whether this answer should be included
        /// </returns>
        public delegate bool IncludeAnswer();

        /// <summary>
        ///     The delegate called to decide if an answer should be removed from a question.
        /// </summary>
        /// <returns>
        ///     Whether this answer should be included
        /// </returns>
        public delegate bool RemoveAnswer();

        #endregion

        // Stores information for answers that are injected into existing questions, the key string being the key for the question
        private static readonly Dictionary<string, List<DialogueAnswerInformation>> AnswerInjectors =
            new Dictionary<string, List<DialogueAnswerInformation>>();

        // Stores information for answers that should be removed from questions, the key string being the key for the question
        private static readonly Dictionary<string, List<DialogueAnswerRemovalInformation>> AnswerRemovals =
            new Dictionary<string, List<DialogueAnswerRemovalInformation>>();

        // Stores information for new results for existing answers and injected answers, the key string being the key for the answer
        private static readonly Dictionary<string, List<DialogueResultInformation>> ResultInjectors =
            new Dictionary<string, List<DialogueResultInformation>>();

        // Stores information for injected questions
        private static readonly Dictionary<string, DialogueQuestionInformation> InjectedQuestions =
            new Dictionary<string, DialogueQuestionInformation>();

        /// <summary>
        ///     Register a new answer which can be used in dialogs
        /// </summary>
        /// <param name="question">Question that this answer will be an answer to</param>
        /// <param name="answer">Information about this answer</param>
        public static void RegisterNewAnswer(Questions question, DialogueAnswerInformation answer)
        {
            RegisterNewAnswer(QuestionsKey(question), answer);
        }

        /// <summary>
        ///     Register a new answer which can be used in dialogs
        /// </summary>
        /// <param name="question">Question that this answer will be an answer to</param>
        /// <param name="answer">Information about this answer</param>
        public static void RegisterNewAnswer(string question, DialogueAnswerInformation answer)
        {
            if (AnswerInjectors.ContainsKey(question))
            {
                AnswerInjectors[question].Add(answer);
            }
            else
            {
                var answerList = new List<DialogueAnswerInformation> { answer };
                AnswerInjectors.Add(question, answerList);

                // Make sure the result of our answer is injected
                RegisterNewResult(question, answer.ModUniqueKey(), answer.Result);
            }
        }

        /// <summary>
        ///     Remove a default answer from a question
        /// </summary>
        /// <param name="question">Question to remove the answer from</param>
        /// <param name="answer">Information about the answer to remove</param>
        public static void RemoveDefaultAnswer(Questions question, DialogueAnswerRemovalInformation answer)
        {
            RemoveDefaultAnswer(QuestionsKey(question), answer);
        }

        /// <summary>
        ///     Remove a default answer from a question
        /// </summary>
        /// <param name="question">Question to remove the answer from</param>
        /// <param name="answer">Information about the answer to remove</param>
        public static void RemoveDefaultAnswer(string question, DialogueAnswerRemovalInformation answer)
        {
            if (AnswerRemovals.ContainsKey(question))
            {
                AnswerRemovals[question].Add(answer);
            }
            else
            {
                var answerList = new List<DialogueAnswerRemovalInformation> { answer };
                AnswerRemovals.Add(question, answerList);
            }
        }

        /// <summary>
        ///     Register a new result which can be used by answers, a result is what happens when an answer is selected
        /// </summary>
        /// <param name="question">The question that was asked to cause this result</param>
        /// <param name="answer">The answer that is selected to cause this result</param>
        /// <param name="result">Information about the result</param>
        public static void RegisterNewResult(Questions question, Answers answer, DialogueResultInformation result)
        {
            RegisterNewResult(QuestionsKey(question), AnswersKey(answer), result);
        }

        /// <summary>
        ///     Register a new result which can be used by answers, a result is what happens when an answer is selected
        /// </summary>
        /// <param name="question">The question that was asked to cause this result</param>
        /// <param name="answer">The answer that is selected to cause this result</param>
        /// <param name="result">Information about the result</param>
        public static void RegisterNewResult(string question, string answer, DialogueResultInformation result)
        {
            if (ResultInjectors.ContainsKey(GetQuestionAndAnswerString(question, answer)))
            {
                ResultInjectors[GetQuestionAndAnswerString(question, answer)].Add(result);
            }
            else
            {
                var resultList = new List<DialogueResultInformation> { result };
                ResultInjectors.Add(GetQuestionAndAnswerString(question, answer), resultList);
            }
        }

        /// <summary>
        ///     Register a new question which can be posed to the player through a dialogue
        /// </summary>
        /// <param name="question">Information about the question</param>
        public static void RegisterQuestion(DialogueQuestionInformation question)
        {
            InjectedQuestions.Add(question.ModUniqueKey(), question);
        }

        /// <summary>
        ///     Open a dialogue window with a question for the player to answer
        /// </summary>
        /// <param name="question">Information about the question that the dialogue window will be opened with</param>
        public static void OpenQuestion(DialogueQuestionInformation question)
        {
            // Check to make sure the question has been registered
            if (InjectedQuestions.ContainsKey(question.ModUniqueKey()))
            {
                var responses = GetResponseArray(question.Choices);

                if (responses.Length == 0)
                {
                    // I'm really not sure what this would do, so I'm leaving it as a warning for now. If it causes some kind of crash or problem, this might have to get upgraded to an exception
                    Log.Warning(
                        $"Question {question.Key} in mod {question.Owner.ModSettings.Name} is attempting to open a question with no possible answers!");
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
        ///     Open a dialogue window that informs the player of something
        /// </summary>
        /// <param name="statement">Text to put in the dialogue window</param>
        public static void OpenStatement(string statement)
        {
            Game1.drawObjectDialogue(statement);
        }

        // Injects the create question dialog call
        [Hook(HookType.Entry, "StardewValley.GameLocation",
            "System.Void StardewValley.GameLocation::createQuestionDialogue(System.String,StardewValley.Response[],System.String)")]
        [Hook(HookType.Entry, "StardewValley.GameLocation",
            "System.Void StardewValley.GameLocation::createQuestionDialogue(System.String,StardewValley.Response[],System.String,StardewValley.Object)")]
        internal static void InjectQuestionDialogueOverload1(
            [InputBind(typeof(Response[]), "answerChoices", true)] ref Response[] answerChoices,
            [InputBind(typeof(string), "dialogKey")] string dialogKey)
        {
            answerChoices = InjectQuestionDialog(answerChoices, dialogKey);
        }

        // Injects where answers are used, in order to inject answer results
        [HookReturnable(HookType.Entry, "StardewValley.GameLocation",
            "System.Boolean StardewValley.GameLocation::answerDialogue(StardewValley.Response)")]
        [HookReturnable(HookType.Entry, "StardewValley.Locations.BusStop",
            "System.Boolean StardewValley.Locations.BusStop::answerDialogue(StardewValley.Response)")]
        [HookReturnable(HookType.Entry, "StardewValley.Locations.Desert",
            "System.Boolean StardewValley.Locations.Desert::answerDialogue(StardewValley.Response)")]
        [HookReturnable(HookType.Entry, "StardewValley.Locations.JojaMart",
            "System.Boolean StardewValley.Locations.JojaMart::answerDialogue(StardewValley.Response)")]
        internal static bool AnswerResultOverride2(
            [UseOutputBind] out bool useOutput,
            [ThisBind] GameLocation @this,
            [InputBind(typeof(Response), "answer")] Response answer)
        {
            var questionKey = GetLastQuestionKey(@this);
            var answerKey = answer.responseKey;

            var doDefault = DoInjectedResults(questionKey, answerKey);

            useOutput = !doDefault;
            return !doDefault;
        }

        // Inject answers into a question dialog
        private static Response[] InjectQuestionDialog(Response[] answerChoices, string dialogKey)
        {
            // The list that will eventually become the responses array
            var finalResponses = new List<Response>();

            // Remove answers registered to be removed
            if (AnswerRemovals.ContainsKey(dialogKey))
            {
                foreach (var answerRemoval in AnswerRemovals[dialogKey])
                {
                    for (var i = 0; i < answerChoices.Length; i++)
                    {
                        // Check if the keys match, and if the answer should be removed
                        if (AnswersKey(answerRemoval.Key).Equals(answerChoices[i].responseKey)
                            && answerRemoval.Owner.ModSettings.ModState == ModState.Loaded && answerRemoval.DoRemove())
                        {
                            // Set the corresponding value in the array to be null
                            answerChoices[i] = null;
                        }
                    }
                }
            }

            // Add any non-null values in the array into the finalResponses list
            foreach (Response t in answerChoices)
            {
                if (t != null)
                {
                    finalResponses.Add(t);
                }
            }

            // Now add any registered answers for this question
            if (AnswerInjectors.ContainsKey(dialogKey))
            {
                foreach (var answer in AnswerInjectors[dialogKey])
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
            var doDefault = true;

            // Check injected results
            if (questionKey != null)
            {
                if (ResultInjectors.ContainsKey(GetQuestionAndAnswerString(questionKey, answerKey)))
                {
                    foreach (var result in ResultInjectors[GetQuestionAndAnswerString(questionKey, answerKey)])
                    {
                        if (result.Owner.ModSettings.ModState == ModState.Loaded)
                        {
                            var resultDoDefault = true;
                            result.Result(ref resultDoDefault);

                            if (resultDoDefault == false)
                            {
                                doDefault = false;
                            }
                        }
                    }
                }

                // Check injected questions for possible answers with results
                foreach (var injectedQuestion in InjectedQuestions)
                {
                    // Check each of its answers
                    foreach (var answerInformation in injectedQuestion.Value.Choices)
                    {
                        // Check that this was the answer given
                        if (answerInformation.ModUniqueKey().Equals(answerKey))
                        {
                            if (answerInformation.Owner.ModSettings.ModState == ModState.Loaded)
                            {
                                var resultDoDefault = true;
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
            return location.lastQuestionKey?.Split(' ')[0];
        }

        // Get the question and answer string for a question key and answer key
        private static string GetQuestionAndAnswerString(string questionKey, string answerKey)
        {
            return $"{questionKey}_{answerKey}";
        }

        /// <summary>
        ///     Gets the string value of a provided question enum.
        /// </summary>
        /// <param name="question">
        ///     Question enum value.
        /// </param>
        /// <returns>
        ///     The enum as a <see cref="string" />.
        /// </returns>
        public static string QuestionsKey(Questions question)
        {
            switch (question)
            {
                case Questions.RemoveIncubatingEgg:
                    {
                        return "RemoveIncubatingEgg";
                    }

                case Questions.StarTokenShopBuyTokens:
                    {
                        return "StarTokenShop";
                    }

                case Questions.WheelBet:
                    {
                        return "wheelBet";
                    }

                case Questions.SlingshotGame:
                    {
                        return "slingshotGame";
                    }

                case Questions.FishingGame:
                    {
                        return "fishingGame";
                    }

                case Questions.FortuneTeller:
                    {
                        return "fortuneTeller";
                    }

                case Questions.StarTokenShopTradeIn:
                    {
                        return "starTokenShop";
                    }

                case Questions.SecretSanta:
                    {
                        return "secretSanta";
                    }

                case Questions.Cave:
                    {
                        return "cave";
                    }

                case Questions.Pet:
                    {
                        return "pet";
                    }

                case Questions.TaxVote:
                    {
                        return "taxvote";
                    }

                case Questions.Eat:
                    {
                        return "Eat";
                    }

                case Questions.Shipping:
                    {
                        return "Shipping";
                    }

                case Questions.Marnie:
                    {
                        return "Marnie";
                    }

                case Questions.ResetMine:
                    {
                        return "ResetMine";
                    }

                case Questions.Blacksmith:
                    {
                        return "Blacksmith";
                    }

                case Questions.Carpenter:
                    {
                        return "carpenter";
                    }

                case Questions.Museum:
                    {
                        return "Museum";
                    }

                case Questions.HouseUpgrade:
                    {
                        return "upgrade";
                    }

                case Questions.EvilShrineRightDeActivate:
                    {
                        return "evilShrineRightDeActivate";
                    }

                case Questions.EvilShrineRightActivate:
                    {
                        return "evilShrineRightActivate";
                    }

                case Questions.BuyJojaCola:
                    {
                        return "buyJojaCola";
                    }

                case Questions.BuyQiClubCoins:
                    {
                        return "BuyClubCoins";
                    }

                case Questions.BackpackUpgrade:
                    {
                        return "Backpack";
                    }

                case Questions.WizardShrine:
                    {
                        return "WizardShrine";
                    }

                case Questions.CalicoJackHs:
                    {
                        return "CalicoJackHS";
                    }

                case Questions.CalicoJack:
                    {
                        return "CalicoJack";
                    }

                case Questions.BuyQiCoins:
                    {
                        return "BuyQiCoins";
                    }

                case Questions.DivorceCancel:
                    {
                        return "divorceCancel";
                    }

                case Questions.Divorce:
                    {
                        return "divorce";
                    }

                case Questions.EvilShrineCenter:
                    {
                        return "evilShrineCenter";
                    }

                case Questions.Minecart:
                    {
                        return "Minecart";
                    }

                case Questions.ClubSeller:
                    {
                        return "ClubSeller";
                    }

                case Questions.ExitMine:
                    {
                        return "ExitMine";
                    }

                case Questions.EvilShrineLeft:
                    {
                        return "evilShrineLeft";
                    }

                case Questions.BeachBridge:
                    {
                        return "BeachBridge";
                    }

                case Questions.Mariner:
                    {
                        return "mariner";
                    }

                case Questions.Bus:
                    {
                        return "Bus";
                    }

                case Questions.DesertBus:
                    {
                        return "DesertBus";
                    }

                case Questions.JojaSignUp:
                    {
                        return "JojaSignUp";
                    }

                case Questions.Shaft:
                    {
                        return "Shaft";
                    }

                case Questions.ClubCard:
                    {
                        return "ClubCard";
                    }

                case Questions.MinecartGame:
                    {
                        return "MinecartGame";
                    }

                // case Questions.HaveBaby:                    { return ""; } // Handled by delegate
                // case Questions.SelectChannel:               { return ""; } // Handled by delegate
                case Questions.Sleep:
                    {
                        return "Sleep";
                    }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get key from Answers.
        /// </summary>
        /// <param name="answer">Answer enum value.</param>
        /// <returns>The enum as a <see cref="System.String" />.</returns>
        public static string AnswersKey(Answers answer)
        {
            switch (answer)
            {
                case Answers.Yes:
                    {
                        return "Yes";
                    }

                case Answers.No:
                    {
                        return "No";
                    }

                case Answers.Leave:
                    {
                        return "Leave";
                    }

                case Answers.Shop:
                    {
                        return "Shop";
                    }

                case Answers.Play:
                    {
                        return "Play";
                    }

                case Answers.StarTokenShopBuy:
                    {
                        return "Buy";
                    }

                case Answers.StarTokenShopLeave:
                    {
                        return "Leave";
                    }

                case Answers.WheelBetOrange:
                    {
                        return "Orange";
                    }

                case Answers.WheelBetGreen:
                    {
                        return "Green";
                    }

                case Answers.WheelBetDontPlay:
                    {
                        return "I";
                    }

                case Answers.SlingshotGamePlay:
                    {
                        return "Play";
                    }

                case Answers.SlingshotGameLeave:
                    {
                        return "Leave";
                    }

                case Answers.FishingGamePlay:
                    {
                        return "Play";
                    }

                case Answers.FishingGameLeave:
                    {
                        return "Leave";
                    }

                case Answers.FortuneTellerRead:
                    {
                        return "Read";
                    }

                case Answers.FortuneTellerNo:
                    {
                        return "No";
                    }

                case Answers.CaveMushrooms:
                    {
                        return "Mushrooms";
                    }

                case Answers.CaveBats:
                    {
                        return "Bats";
                    }

                case Answers.TaxVoteFor:
                    {
                        return "For";
                    }

                case Answers.TaxVoteAgainst:
                    {
                        return "Against";
                    }

                case Answers.MarnieSupplies:
                    {
                        return "Supplies";
                    }

                case Answers.MarniePurchase:
                    {
                        return "Purchase";
                    }

                case Answers.MarnieLeave:
                    {
                        return "Leave";
                    }

                case Answers.BlacksmithShop:
                    {
                        return "Shop";
                    }

                case Answers.BlacksmithUpgrade:
                    {
                        return "Upgrade";
                    }

                case Answers.BlacksmithProcess:
                    {
                        return "Process";
                    }

                case Answers.BlacksmithLeave:
                    {
                        return "Leave";
                    }

                case Answers.CarpenterShop:
                    {
                        return "Shop";
                    }

                case Answers.CarpenterUpgrade:
                    {
                        return "Upgrade";
                    }

                case Answers.CarpenterConstruct:
                    {
                        return "Construct";
                    }

                case Answers.CarpenterLeave:
                    {
                        return "Leave";
                    }

                case Answers.MuseumDonate:
                    {
                        return "Donate";
                    }

                case Answers.MuseumCollect:
                    {
                        return "Collect";
                    }

                case Answers.MuseumLeave:
                    {
                        return "Leave";
                    }

                case Answers.BackpackPurchase:
                    {
                        return "Purchase";
                    }

                case Answers.BackpackNot:
                    {
                        return "Not";
                    }

                case Answers.CalicoJackPlay:
                    {
                        return "Play";
                    }

                case Answers.CalicoJackLeave:
                    {
                        return "Leave";
                    }

                case Answers.CalicoJackRules:
                    {
                        return "Rules";
                    }

                case Answers.MineCartTown:
                    {
                        return "Town";
                    }

                case Answers.MineCartBus:
                    {
                        return "Bus";
                    }

                case Answers.MineCartQuarry:
                    {
                        return "Quarry";
                    }

                case Answers.MineCartCancel:
                    {
                        return "Cancel";
                    }

                case Answers.ClubSellerIll:
                    {
                        return "I'll";
                    }

                case Answers.ClubSellerNo:
                    {
                        return "No";
                    }

                case Answers.ExitMineLeave:
                    {
                        return "Leave";
                    }

                case Answers.ExitMineDoNothing:
                    {
                        return "Do";
                    }

                case Answers.MinecartGameProgress:
                    {
                        return "Progress";
                    }

                case Answers.MinecartGameEndless:
                    {
                        return "Endless";
                    }

                case Answers.MinecartGameExit:
                    {
                        return "Exit";
                    }

                case Answers.MarinerBuy:
                    {
                        return "Buy";
                    }

                case Answers.MarinerNot:
                    {
                        return "Not";
                    }

                case Answers.DesertBusYes:
                    {
                        return "Yes";
                    }

                case Answers.DesertBusNot:
                    {
                        return "Not";
                    }

                case Answers.ShaftJump:
                    {
                        return "Jump";
                    }

                case Answers.ShaftDoNothing:
                    {
                        return "Do";
                    }

                case Answers.ClubCardYes:
                    {
                        return "Yes.";
                    }

                case Answers.ClubCardThatsRight:
                    {
                        return "That's";
                    }

                case Answers.HaveBabyYes:
                    {
                        return "Yes";
                    }

                case Answers.HaveBabyNo:
                    {
                        return "Not";
                    }

                case Answers.SelectChannelWeather:
                    {
                        return "Weather";
                    }

                case Answers.SelectChannelFortune:
                    {
                        return "Fortune";
                    }

                case Answers.SelectChannelLivin:
                    {
                        return "Livin'";
                    }

                case Answers.SelectChannelQueen:
                    {
                        return "The";
                    }

                case Answers.SelectChannelLeave:
                    {
                        return "(Leave)";
                    }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get a <see cref="Response" /> array from a list of DialogueAnswerInformation.
        /// </summary>
        /// <param name="answers">
        ///     The answers to get the responses for.
        /// </param>
        /// <returns>
        ///     The <see cref="Response" /> array for these answers.
        /// </returns>
        public static Response[] GetResponseArray(List<DialogueAnswerInformation> answers)
        {
            var responseList = new List<Response>();

            foreach (var answer in answers)
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