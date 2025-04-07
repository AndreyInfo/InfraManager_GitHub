using Inframanager;
using System;

namespace InfraManager.DAL.ServiceDesk
{
    public interface IBuildObjectIsUnderControlSpecification<T> : IBuildSpecification<T, Guid>
        where T : IGloballyIdentifiedEntity
    {
    }
}
