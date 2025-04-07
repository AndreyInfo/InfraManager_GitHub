using System.ComponentModel;

namespace InfraManager.ComponentModel
{
    public interface ICancelClose
    {
        event CancelEventHandler CancelClose;
    }
}
