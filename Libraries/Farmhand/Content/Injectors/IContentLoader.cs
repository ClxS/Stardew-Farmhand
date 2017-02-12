namespace Farmhand.Content.Injectors
{
    using System;

    /// <summary>
    ///     Handles loading assets loaded by the <see cref="ContentManager" />.
    /// </summary>
    public interface IContentLoader
    {
        /// <summary>
        ///     Gets whether this injector should be used for the specified asset.
        /// </summary>
        /// <param name="type">
        ///     The type of asset.
        /// </param>
        /// <param name="asset">
        ///     The asset path.
        /// </param>
        /// <returns>
        ///     Whether this is used to load/inject this asset.
        /// </returns>
        bool HandlesAsset(Type type, string asset);

        /// <summary>
        ///     Loads the asset using this injector.
        /// </summary>
        /// <param name="contentManager">
        ///     The parent <see cref="ContentManager" />.
        /// </param>
        /// <param name="assetName">
        ///     The asset path.
        /// </param>
        /// <typeparam name="T">
        ///     The type of asset being loaded.
        /// </typeparam>
        /// <returns>
        ///     The loaded asset.
        /// </returns>
        T Load<T>(ContentManager contentManager, string assetName);
    }
}