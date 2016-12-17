using System;

namespace Farmhand.Content.Injectors
{
    public interface IContentInjector
    {
        bool IsLoader { get; }
        bool IsInjector { get; }

        bool HandlesAsset(Type type, string asset);

        T Load<T>(ContentManager contentManager, string assetName);
        void Inject<T>(T obj, string assetName, ref object output);
    }
}
