using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Pulse
{
    [Obsolete("This utility is experimental and may be subject to change in a later version - breaking mod compatibility.")]
    public class PulseEventArgs : EventArgs
    {
        public Dictionary<string, object> Data;
        public PulseEventArgs(Dictionary<string, object> data)
        {
            Data = data;
        }
    }
}
