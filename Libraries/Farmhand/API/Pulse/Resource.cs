using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Pulse
{
    [Obsolete("This utility is experimental and may be subject to change in a later version - breaking mod compatibility.")]
    public class Resource
    {
        private dynamic _Value;
        private Type _Type;
        public bool Set<T>(T value)
        {
            if (_Value != null)
                return false;
            _Value = value;
            _Type = typeof(T);
            return true;
        }
        public dynamic Get()
        {
            return _Value;
        }
        public Type Type()
        {
            return _Type;
        }
    }
}
