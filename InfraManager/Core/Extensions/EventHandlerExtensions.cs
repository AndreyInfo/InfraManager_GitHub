using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class EventHandlerExtensions
    {
        public static void Raise(this EventHandler eventHandler, object sender)
        {
            Raise(eventHandler, sender, System.EventArgs.Empty);
        }

        public static void Raise(this EventHandler eventHandler, object sender, System.EventArgs args)
        {
            if (eventHandler == null || sender == null || args == null)
                return;

            eventHandler(sender, args);
        }
    }
}
