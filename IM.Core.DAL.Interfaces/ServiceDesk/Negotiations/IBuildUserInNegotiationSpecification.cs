using Inframanager;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public interface IBuildUserInNegotiationSpecification<T> : IBuildSpecification<T, User>
        where T : IGloballyIdentifiedEntity
    {
    }
}
