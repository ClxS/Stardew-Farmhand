namespace Farmhand.Content.Injectors.NPCs
{
    using System;

    using Farmhand.API.NPCs;

    internal class PortraitLoader : IContentLoader
    {
        #region IContentLoader Members

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

        #endregion
    }
}