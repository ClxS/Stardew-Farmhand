namespace Farmhand.Events.Arguments.GlobalRoute
{
    /// <summary>
    ///     Arguments for GlobalRouteReturnable.
    /// </summary>
    public class GlobalRouteReturnableEventArgs : GlobalRouteEventArgs
    {
        private object output;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GlobalRouteReturnableEventArgs" /> class.
        /// </summary>
        /// <param name="type">
        ///     The name of the type containing the method.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        /// <param name="parameters">
        ///     The parameters.
        /// </param>
        /// <param name="output">
        ///     The output.
        /// </param>
        public GlobalRouteReturnableEventArgs(string type, string method, object[] parameters, object output)
            : base(type, method, parameters)
        {
            this.output = output;
        }

        /// <summary>
        ///     Gets a value indicating whether to cancel the method and use our output value.
        /// </summary>
        public bool Cancel { get; private set; }

        /// <summary>
        ///     Gets or sets the output to use in the method.
        /// </summary>
        public object Output
        {
            get
            {
                return this.output;
            }

            set
            {
                this.output = value;
                this.Cancel = true;
            }
        }
    }
}