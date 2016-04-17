using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Content
{
    class BlueprintInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\Blueprints";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName)
        {
            var blueprints = obj as Dictionary<string, string>;
            if(blueprints == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var blueprint in Farmhand.API.Buildings.Blueprint.Blueprints)
            {
                blueprints[blueprint.Name] = blueprint.BlueprintString;
            }
        }
    }
}
