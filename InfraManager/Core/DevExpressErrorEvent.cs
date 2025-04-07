using System;

namespace InfraManager.Core
{
    public static class DevExpressErrorEvent
    {
        public static event EventHandler MainEvent;

        private static bool Triggered = false;

        public static void FireMainEvent(object sender, System.EventArgs e)
        {
            var evt = MainEvent;
            if (evt != null && !Triggered)
            {
                Triggered = true;
                evt(sender, e);
            }
        }

        public static void Refresh()
        {
            Triggered = false;
        }
    }
}
