namespace Farmhand.Content.Injectors.Mail
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Player;
    using Farmhand.Logging;

    internal class MailInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => false;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\mail";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var mailBox = obj as Dictionary<string, string>;
            if (mailBox == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            foreach (var mail in Mail.MailBox)
            {
                mailBox[mail.Value.Id] = mail.Value.ToString();
            }
        }

        #endregion
    }
}