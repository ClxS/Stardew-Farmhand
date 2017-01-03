namespace Farmhand.API.Player
{
    using System.Collections.Generic;

    using Farmhand.Logging;

    /// <summary>
    ///     Mail-related API functionality
    /// </summary>
    public static class Mail
    {
        internal static Dictionary<string, MailInformation> MailBox { get; } 
            = new Dictionary<string, MailInformation>();

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
    }
}