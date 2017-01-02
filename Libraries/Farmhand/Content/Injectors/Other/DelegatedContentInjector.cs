namespace Farmhand.Content.Injectors.Other
{
    using System;
    using System.Collections.Generic;

    using Farmhand.Logging;

    internal class DelegatedContentInjector : IContentInjector
    {
        #region Delegates

        public delegate void FileInjectMethod<in T>(T file, string assetName, ref object output);

        public delegate T FileLoadMethod<T>(LoadBase<T> loadBase, string assetName);

        public delegate T LoadBase<out T>(string assetName);

        #endregion

        private static readonly List<KeyValuePair<string, Type>> HandledAssets = new List<KeyValuePair<string, Type>>();

        private static readonly Dictionary<KeyValuePair<string, Type>, Delegate> LoaderRegistry =
            new Dictionary<KeyValuePair<string, Type>, Delegate>();

        private static readonly Dictionary<KeyValuePair<string, Type>, List<Delegate>> InjectorRegistry =
            new Dictionary<KeyValuePair<string, Type>, List<Delegate>>();

        #region IContentInjector Members

        public bool IsLoader { get; } = true;

        public bool IsInjector { get; } = true;

        public bool HandlesAsset(Type type, string assetName)
        {
            return HandledAssets.Exists(x => x.Key == assetName && x.Value == type);
        }

        public T Load<T>(ContentManager manager, string assetName)
        {
            var output = default(T);
            var key = new KeyValuePair<string, Type>(assetName, typeof(T));
            if (LoaderRegistry.ContainsKey(key))
            {
                output = ((FileLoadMethod<T>)LoaderRegistry[key])(manager.LoadDirect<T>, assetName);
            }

            if (output != null && output.Equals(default(T)))
            {
                return manager.LoadDirect<T>(assetName);
            }

            return output;
        }

        public void Inject<T>(T item, string assetName, ref object output)
        {
            var key = new KeyValuePair<string, Type>(assetName, typeof(T));
            if (!InjectorRegistry.ContainsKey(key))
            {
                return;
            }

            foreach (var handler in InjectorRegistry[key])
            {
                ((FileInjectMethod<T>)handler)(item, assetName, ref output);
            }
        }

        #endregion

        public static void RegisterFileLoader<T>(string assetName, FileLoadMethod<T> handler)
        {
            if (HandledAssets.Exists(x => x.Key == assetName && x.Value == typeof(T)))
            {
                Log.Warning($"Load handler conflict on asset {assetName}. Using first");
                return;
            }

            var key = new KeyValuePair<string, Type>(assetName, typeof(T));
            HandledAssets.Add(key);
            LoaderRegistry.Add(key, handler);
        }

        public static void RegisterFileInjector<T>(string assetName, FileInjectMethod<T> handler)
        {
            var key = new KeyValuePair<string, Type>(assetName, typeof(T));
            if (!HandledAssets.Exists(x => x.Key == assetName && x.Value == typeof(T)))
            {
                HandledAssets.Add(key);
            }

            if (!InjectorRegistry.ContainsKey(key))
            {
                InjectorRegistry.Add(key, new List<Delegate>());
            }

            InjectorRegistry[key].Add(handler);
        }
    }
}