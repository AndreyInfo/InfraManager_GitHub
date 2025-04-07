using System;

namespace InfraManager.ComponentModel
{
    public delegate void CallPickUpEventHandler(ITelephonyConnector sender, string fromNumber, string toNumber);

    public interface ITelephonyConnector : IDisposable
    {
        bool CallTo(string fromNumber, string toNumber);

        event CallPickUpEventHandler CallPickUp;
        event EventHandler SDKFatalError;
    }
}
