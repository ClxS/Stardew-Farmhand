namespace Farmhand.API.Player
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Farmhand.Events;
    using Farmhand.Events.Arguments.MailEvents;
    using Farmhand.Logging;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     Mail-related API functionality
    /// </summary>
    public static class Mail
    {
        static Mail()
        {
            MailEvents.AfterOpenedMail += MailEvents_OpenedMail;
        }

        /// <summary>
        ///     Gets the mail box.
        ///     TODO: Make this internal, provide read-only access
        /// </summary>
        public static Dictionary<string, MailInformation> MailBox { get; } = new Dictionary<string, MailInformation>();

        /// <summary>
        ///     Register a mail item to be inserted into the game.
        /// </summary>
        /// <param name="mailInformation">
        ///     Information on the mail item to insert.
        /// </param>
        public static void RegisterMail(MailInformation mailInformation)
        {
            if (MailBox.ContainsKey(mailInformation.Id) && MailBox[mailInformation.Id] != mailInformation)
            {
                Log.Warning(
                    $"Potential conflict registering new mail. Mail {mailInformation.Id} has been registered by two separate mods."
                    + "Only the last registered one will be used.");
            }

            MailBox[mailInformation.Id] = mailInformation;
        }

        private static void MailEvents_OpenedMail(object sender, EventArgsOpenedMail e)
        {
            var letterMenu = sender as LetterViewerMenu;
            if (letterMenu != null)
            {
                if (e.AttachmentType == "fh-recipe")
                {
                    if (!string.IsNullOrEmpty(e.AttachmentValue)
                        && Content.ContentManager.Load<Dictionary<string, string>>("Data\\CookingRecipes")
                            .ContainsKey(e.AttachmentValue))
                    {
                        Game1.player.cookingRecipes.Add(e.AttachmentValue, 0);

                        // TODO: A non-reflection way of doing this?
                        var type = letterMenu.GetType();
                        try
                        {
                            var learnedRecipeField = type.GetField(
                                "learnedRecipe",
                                BindingFlags.NonPublic | BindingFlags.Instance);
                            var cookingOrCraftingField = type.GetField(
                                "cookingOrCrafting",
                                BindingFlags.NonPublic | BindingFlags.Instance);

                            if (learnedRecipeField == null)
                            {
                                throw new NullReferenceException("Reflection failed to retrieve learnedRecipe field");
                            }

                            if (cookingOrCraftingField == null)
                            {
                                throw new NullReferenceException(
                                    "Reflection failed to retrieve cookingOrCrafting field");
                            }

                            learnedRecipeField.SetValue(letterMenu, e.AttachmentValue);
                            cookingOrCraftingField.SetValue(letterMenu, "cooking");
                        }
                        catch (Exception ex)
                        {
                            Log.Exception("Encountered exception trying to set private LetterViewerMenu fields", ex);
                        }
                    }
                }
            }
        }
    }
}