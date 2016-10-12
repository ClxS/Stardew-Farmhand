using System;
using System.Collections.Generic;

namespace Farmhand.API.Pulse
{
    [Obsolete("This utility is experimental and may be subject to change in a later version and will break mod compatibility.")]
    public interface IPulsableObject
    {
        T Get<T>(string propertyName);
        void Set<TReturn>(string propertyName);

        TReturn Call<TReturn>(string methodName);
        TReturn Call<TReturn, TParam1>(string methodName, TParam1 param1);
        TReturn Call<TReturn, TParam1, TParam2>(string methodName, TParam1 param1, TParam2 param2);
        TReturn Call<TReturn, TParam1, TParam2, TParam3>(string methodName, TParam1 param1, TParam2 param2, TParam3 param3);

        IEnumerable<string> GetPropertyNames();
        IEnumerable<string> GetMethodNames();
    }
}
