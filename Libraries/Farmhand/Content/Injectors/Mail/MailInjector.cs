using System;
using System.Collections.Generic;

namespace Farmhand.Content.Injectors.Mail
{
    public class MailInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\mail";
        }
        
        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var mailBox = obj as Dictionary<string, string>;
            if (mailBox == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var mail in API.Player.Mail.MailBox)
            {
                mailBox[mail.Value.Id] = mail.Value.ToString();
            }
        }
    }
}
