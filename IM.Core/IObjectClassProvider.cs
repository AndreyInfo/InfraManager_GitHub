using InfraManager;

namespace Inframanager
{
    public interface IObjectClassProvider<T>
    {
        ObjectClass GetObjectClass();
    }
}
