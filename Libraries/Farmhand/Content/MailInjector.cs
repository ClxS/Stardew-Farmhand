using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Content
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

            foreach (var mail in Farmhand.API.Player.Mail.MailBox)
            {
                mailBox[mail.Value.Id] = mail.Value.ToString();
            }
        }
    }
}
