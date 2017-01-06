namespace Farmhand.Events
{
    using System;
    using System.Linq;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.MailEvents;

    public static class MailEvents
    {
        private static string previousAttachType;

        private static string previousAttachValue;

        public static event EventHandler<EventArgsOpenedLetter> BeforeOpenedLetter = delegate { };

        public static event EventHandler<EventArgsOpenedLetter> AfterOpenedLetter = delegate { };

        public static event EventHandler<EventArgsOpenedMail> BeforeOpenedMail = delegate { };

        public static event EventHandler<EventArgsOpenedMail> AfterOpenedMail = delegate { };

        [Hook(HookType.Entry, "StardewValley.Menus.LetterViewerMenu",
            "System.Void StardewValley.Menus.LetterViewerMenu::.ctor(System.String)")]
        internal static void OnBeforeOpenedMail(
            [ThisBind] object @this,
            [InputBind(typeof(string), "text")] string text)
        {
            EventCommon.SafeInvoke(BeforeOpenedLetter, @this, new EventArgsOpenedLetter(text));
        }

        [Hook(HookType.Exit, "StardewValley.Menus.LetterViewerMenu",
            "System.Void StardewValley.Menus.LetterViewerMenu::.ctor(System.String)")]
        internal static void OnAfterOpenedMail([ThisBind] object @this, [InputBind(typeof(string), "text")] string text)
        {
            EventCommon.SafeInvoke(AfterOpenedLetter, @this, new EventArgsOpenedLetter(text));
        }

        [Hook(HookType.Entry, "StardewValley.Menus.LetterViewerMenu",
            "System.Void StardewValley.Menus.LetterViewerMenu::.ctor(System.String,System.String)")]
        internal static void OnBeforeMailOpened(
            [ThisBind] object @this,
            [InputBind(typeof(string), "mail")] string mail,
            [InputBind(typeof(string), "mailTitle")] string mailTitle)
        {
            var itemIndex = mail.IndexOf("%item", StringComparison.Ordinal);

            var message = mail;

            if (itemIndex >= 0)
            {
                message = mail.Substring(0, itemIndex);
                var itemKey = mail.Substring(itemIndex);
                itemKey = itemKey.Replace("%item ", string.Empty).Replace(" %%", string.Empty);

                var attachParams = itemKey.Split(' ');
                previousAttachType = attachParams.Length > 0 ? attachParams[0] : string.Empty;

                previousAttachValue = string.Join(" ", attachParams.Skip(1));
            }

            EventCommon.SafeInvoke(
                BeforeOpenedMail,
                @this,
                new EventArgsOpenedMail(message, mailTitle, previousAttachType, previousAttachValue));
        }

        [Hook(HookType.Exit, "StardewValley.Menus.LetterViewerMenu",
            "System.Void StardewValley.Menus.LetterViewerMenu::.ctor(System.String,System.String)")]
        internal static void OnAfterMailOpened(
            [ThisBind] object @this,
            [InputBind(typeof(string), "mail")] string mail,
            [InputBind(typeof(string), "mailTitle")] string mailTitle)
        {
            EventCommon.SafeInvoke(
                AfterOpenedMail,
                @this,
                new EventArgsOpenedMail(mail, mailTitle, previousAttachType, previousAttachValue));
        }
    }
}