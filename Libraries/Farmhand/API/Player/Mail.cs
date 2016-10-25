using System.Collections.Generic;

namespace Farmhand.API.Player
{
    public class Mail
    {
        public static Dictionary<string, MailInformation> MailBox { get; } = new Dictionary<string, MailInformation>();

        public static void RegisterMail(MailInformation mailInformation)
        {
            if (MailBox.ContainsKey(mailInformation.Id) && MailBox[mailInformation.Id] != mailInformation)
            {
                Logging.Log.Warning($"Potential conflict registering new mail. Mail {mailInformation.Id} has been registered by two separate mods." +
                                    "Only the last registered one will be used.");
            }
            MailBox[mailInformation.Id] = mailInformation;
        }
    }
}
