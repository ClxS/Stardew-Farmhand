namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.MailEvents;

    public class MailEvents
    {
        public static event EventHandler<EventArgsOpenedMail> OpenedMail = delegate { };

        public static event EventHandler<EventArgsOpenedMailWithAttachment> OpenedMailWithAttachment = delegate { };

        [Hook(HookType.Entry, "StardewValley.Menus.LetterViewerMenu",
            "System.Void StardewValley.Menus.LetterViewerMenu::.ctor(System.String)")]
        internal static void OnOpenedMail([ThisBind] object @this, [InputBind(typeof(string), "text")] string text)
        {
            EventCommon.SafeInvoke(OpenedMail, @this, new EventArgsOpenedMail(text));
        }

        [Hook(HookType.Entry, "StardewValley.Menus.LetterViewerMenu",
            "System.Void StardewValley.Menus.LetterViewerMenu::.ctor(System.String,System.String)")]
        internal static void OnOpenedMailWithAttachment(
            [ThisBind] object @this,
            [InputBind(typeof(string), "mail")] string mail,
            [InputBind(typeof(string), "mailTitle")] string mailTitle)
        {
            EventCommon.SafeInvoke(
                OpenedMailWithAttachment,
                @this,
                new EventArgsOpenedMailWithAttachment(mail, mailTitle));
        }
    }
}