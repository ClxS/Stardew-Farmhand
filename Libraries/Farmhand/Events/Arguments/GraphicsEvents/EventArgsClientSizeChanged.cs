using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments.GraphicsEvents
{
    public class EventArgsClientSizeChanged : EventArgs
    {
        public EventArgsClientSizeChanged(int width, int height) 
        { 
            NewWidth = width;
            NewHeight = height;
        }
        public int NewWidth { get; set; }
        public int NewHeight { get; set; }
    }
}
