namespace Farmhand.Events.Arguments.GlobalRoute
{
    /// <summary>
    ///     Arguments for GlobalRouteCancellable.
    /// </summary>
    public sealed class GlobalRouteCancellableEventArgs : GlobalRouteEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GlobalRouteCancellableEventArgs" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        /// <param name="parameters">
        ///     The parameters.
        /// </param>
        public GlobalRouteCancellableEventArgs(string type, string method, object[] parameters)
            : base(type, method, parameters)
        {
            this.Cancel = false;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the method is cancelled.
        /// </summary>
        public bool Cancel { get; set; }
    }
}