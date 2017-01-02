namespace Farmhand.Content.Injectors.Maps
{
    using System;

    using Farmhand.API.Locations;
    using Farmhand.Logging;

    using xTile;

    internal class MapInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => false;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return type == typeof(Map);
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var map = obj as Map;
            if (map == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            map = LocationUtilities.MergeMaps(map, assetName);
            output = map;
        }

        #endregion
    }
}