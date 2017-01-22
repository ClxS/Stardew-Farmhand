namespace Farmhand.Content.Injectors.NPCs
{
    using System;

    using Farmhand.API.NPCs;
    using Farmhand.Logging;

    internal class PortraitLoader : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => true;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string assetName)
        {
            var baseName = assetName.Replace("Portraits\\", string.Empty);
            return Npc.Npcs.ContainsKey(baseName);
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Portraits\\", string.Empty);
            var texture = Npc.Npcs[baseName].Item1.Portrait;

            return (T)Convert.ChangeType(texture, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            Log.Error("You shouldn't be here!");
        }

        #endregion
    }
}