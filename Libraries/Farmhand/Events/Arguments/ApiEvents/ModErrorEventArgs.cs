namespace Farmhand.Events.Arguments.ApiEvents
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     Arguments for ModError.
    /// </summary>
    public class ModErrorEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ModErrorEventArgs" /> class.
        /// </summary>
        /// <param name="assembly">
        ///     The assembly which threw an exception.
        /// </param>
        /// <param name="ex">
        ///     The exception thrown.
        /// </param>
        public ModErrorEventArgs(Assembly assembly, Exception ex)
        {
            this.Assembly = assembly;
            this.Exception = ex;
        }

        /// <summary>
        ///     Gets the assembly which threw an exception.
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        ///     Gets the exception thrown.
        /// </summary>
        public Exception Exception { get; }
    }
}