namespace Farmhand.Logging
{
    /// <summary>
    ///     Enumeration of different message types.
    /// </summary>
    public enum LogEntryType
    {
        /// <summary>
        ///     Messages which which display some useful information.
        /// </summary>
        Info,

        /// <summary>
        ///     Messages which are mostly spam.
        /// </summary>
        Verbose,

        /// <summary>
        ///     Messages which indicate success.
        /// </summary>
        Success,

        /// <summary>
        ///     Messages which indicate a warning occurred.
        /// </summary>
        Warning,

        /// <summary>
        ///     Messages which indicate an error occurred.
        /// </summary>
        Error,

        /// <summary>
        ///     Messages used to display developer comments.
        /// </summary>
        Comment
    }
}