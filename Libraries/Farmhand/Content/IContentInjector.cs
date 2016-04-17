using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Farmhand.Content
{
    public interface IContentInjector
    {
        bool IsLoader { get; }
        bool IsInjector { get; }

        bool HandlesAsset(Type type, string asset);

        T Load<T>(ContentManager contentManager, string assetName);
        void Inject<T>(T obj, string assetName);
    }
}
