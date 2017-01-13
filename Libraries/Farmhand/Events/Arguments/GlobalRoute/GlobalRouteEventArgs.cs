namespace Farmhand.Events.Arguments.GlobalRoute
{
    using System;

    /// <summary>
    ///     Arguments for GlobalRoute.
    /// </summary>
    public class GlobalRouteEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GlobalRouteEventArgs" /> class.
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
        public GlobalRouteEventArgs(string type, string method, object[] parameters)
        {
            this.Type = type;
            this.Method = method;
            this.Parameters = parameters;
        }

        /// <summary>
        ///     Gets the method being invoked.
        /// </summary>
        public string Method { get; }

        /// <summary>
        ///     Gets the parameters to the method.
        /// </summary>
        public object[] Parameters { get; }

        /// <summary>
        ///     Gets the name of the type containing the method.
        /// </summary>
        public string Type { get; }
    }
}