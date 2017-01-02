namespace Farmhand.Content.Injectors.Blueprints
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Buildings;
    using Farmhand.Logging;

    internal class BlueprintInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => false;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\Blueprints";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var blueprints = obj as Dictionary<string, string>;
            if (blueprints == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            foreach (var blueprint in Blueprint.Blueprints)
            {
                blueprints[blueprint.Name] = blueprint.BlueprintString;
            }
        }

        #endregion
    }
}