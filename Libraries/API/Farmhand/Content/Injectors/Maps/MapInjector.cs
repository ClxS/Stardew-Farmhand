namespace Farmhand.Content.Injectors.Maps
{
    using System;

    using Farmhand.API.Locations;

    using xTile;

    internal class MapInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool HandlesAsset(Type type, string asset)
        {
            return type == typeof(Map);
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