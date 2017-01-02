namespace Farmhand.Content.Injectors
{
    using System;

    /// <summary>
    ///     Handles loading or injecting into assets loaded by the <see cref="ContentManager" />.
    /// </summary>
    public interface IContentInjector
    {
        /// <summary>
        ///     Gets a value indicating whether is this handles loading assets.
        /// </summary>
        bool IsLoader { get; }

        /// <summary>
        ///     Gets a value indicating whether is this handles injecting into loaded assets.
        /// </summary>
        bool IsInjector { get; }

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