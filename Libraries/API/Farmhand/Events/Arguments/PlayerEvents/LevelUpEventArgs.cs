namespace Farmhand.Events.Arguments.PlayerEvents
{
    using System;

    using Farmhand.API.Player;

    /// <summary>
    ///     Arguments for LevelUp.
    /// </summary>
    public class LevelUpEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelUpEventArgs" /> class.
        /// </summary>
        /// <param name="which">
        ///     The which.
        /// </param>
        /// <param name="newLevel">
        ///     The new level.
        /// </param>
        /// <param name="oldLevel">
        ///     The old level.
        /// </param>
        public LevelUpEventArgs(int which, int newLevel, int oldLevel)
        {
            this.Which = (Skill)which;
            this.NewLevel = newLevel;
            this.OldLevel = oldLevel;
        }

        /// <summary>
        ///     Gets the which skill leveled up.
        /// </summary>
        public Skill Which { get; }

        /// <summary>
        ///     Gets the new level.
        /// </summary>
        public int NewLevel { get; }

        /// <summary>
        ///     Gets the old level.
        /// </summary>
        public int OldLevel { get; }
    }
}