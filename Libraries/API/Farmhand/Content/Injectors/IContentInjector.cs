namespace Farmhand.Content.Injectors
{
    using System;

    /// <summary>
    ///     Handles injecting into assets loaded by the <see cref="ContentManager" />.
    /// </summary>
    public interface IContentInjector
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
        ///     Inject the asset using this injector.
        /// </summary>
        /// <param name="obj">
        ///     The loaded object.
        /// </param>
        /// <param name="assetName">
        ///     The asset name.
        /// </param>
        /// <param name="output">
        ///     The output object.
        /// </param>
        /// <typeparam name="T">
        ///     The type of asset being injected.
        /// </typeparam>
        void Inject<T>(T obj, string assetName, ref object output);
    }
}