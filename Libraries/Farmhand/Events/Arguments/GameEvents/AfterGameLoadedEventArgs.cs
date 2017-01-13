namespace Farmhand.Events.Arguments.GameEvents
{
    using System;

    /// <summary>
    ///     Arguments for AfterGameLoaded.
    /// </summary>
    public class AfterGameLoadedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AfterGameLoadedEventArgs" /> class.
        /// </summary>
        /// <param name="loadedGame">
        ///     The loaded game.
        /// </param>
        public AfterGameLoadedEventArgs(bool loadedGame = false)
        {
            this.LoadedGame = loadedGame;
        }

        /// <summary>
        ///     Gets a value indicating whether loaded game.
        /// </summary>
        public bool LoadedGame { get; }
    }
}