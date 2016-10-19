using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Farmhand.Logging;

namespace Farmhand.Content
{
    class DelegatedContentInjector : IContentInjector
    {
        public bool IsLoader { get; } = true;
        public bool IsInjector { get; } = true;

        public delegate T LoadBase<T>(string assetName);
        public delegate T FileLoadMethod<T>(LoadBase<T> loadBase, string assetName);
        public delegate void FileInjectMethod<T>(T file, string assetName, ref object output);

        private static List<KeyValuePair<string, Type>> _HandledAssets;
        private static Dictionary<KeyValuePair<string, Type>, Delegate> _LoaderRegistry=new Dictionary<KeyValuePair<string, Type>, Delegate>();
        private static Dictionary<KeyValuePair<string, Type>, List<Delegate>> _InjectorRegistry = new Dictionary<KeyValuePair<string, Type>, List<Delegate>>();

        public static void RegisterFileLoader<T>(string assetName, FileLoadMethod<T> handler)
        {
            if (_HandledAssets.Exists(x => x.Key == assetName && x.Value == typeof(T)))
            {
                Log.Warning($"Load handler conflict on asset {assetName}. Using first");
                return;
            }
            KeyValuePair<string, Type> key = new KeyValuePair<string, Type>(assetName, typeof(T));
            _HandledAssets.Add(key);
            _LoaderRegistry.Add(key, handler);
        }
        public static void RegisterFileInjector<T>(string assetName, FileInjectMethod<T> handler)
        {
            KeyValuePair<string, Type> key = new KeyValuePair<string, Type>(assetName, typeof(T));
            if (!_HandledAssets.Exists(x => x.Key == assetName && x.Value == typeof(T)))
                _HandledAssets.Add(key);
            if (!_InjectorRegistry.ContainsKey(key))
                _InjectorRegistry.Add(key, new List<Delegate>());
            _InjectorRegistry[key].Add(handler);
        }
        public bool HandlesAsset(Type type, string assetName)
        {
            return _HandledAssets.Exists(x => x.Key == assetName && x.Value == type);
        }
        public T Load<T>(ContentManager manager, string assetName)
        {
            T output = default(T);
            KeyValuePair<string, Type> key = new KeyValuePair<string, Type>(assetName, typeof(T));
            if (_LoaderRegistry.ContainsKey(key))
                output = ((FileLoadMethod<T>)_LoaderRegistry[key])(manager.LoadDirect<T>, assetName);
            if (output.Equals(default(T)))
                return manager.LoadDirect<T>(assetName);
            return output;
        }
        public void Inject<T>(T item, string assetName,ref object output)
        {
            KeyValuePair<string, Type> key = new KeyValuePair<string, Type>(assetName, typeof(T));
            if (!_InjectorRegistry.ContainsKey(key))
                return;
            foreach(Delegate handler in _InjectorRegistry[key])
                ((FileInjectMethod<T>)handler)(item,assetName,ref output);
        }
    }
}
