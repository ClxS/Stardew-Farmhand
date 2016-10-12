namespace Farmhand.Events.Arguments.GlobalRoute
{
    public class EventArgsGlobalRouteReturnable : EventArgsGlobalRoute
    {
        public EventArgsGlobalRouteReturnable(string type, string method, object[] parameters, object output) : base(type, method, parameters)
        {
            _output = output;
        }

        private bool _cancel;
        public bool Cancel
        {
            get
            {
                return _cancel;
            }
        }

        private object _output;
        public object Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
                _cancel = true;
            }
        }
    }
}
