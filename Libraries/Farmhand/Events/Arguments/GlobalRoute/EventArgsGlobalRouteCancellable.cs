namespace Farmhand.Events.Arguments.GlobalRoute
{
    public sealed class EventArgsGlobalRouteCancellable : EventArgsGlobalRoute
    {
        public EventArgsGlobalRouteCancellable(string type, string method, object[] parameters)
            : base(type, method, parameters)
        {
            Type = type;
            Method = method;
            Parameters = parameters;
            Cancel = false;
        }

        public bool Cancel { get; set; }
    }
}
