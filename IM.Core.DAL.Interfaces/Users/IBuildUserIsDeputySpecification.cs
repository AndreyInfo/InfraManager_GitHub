using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Users
{
    public interface IBuildUserIsDeputySpecification : 
        IBuildSpecification<IEnumerable<User>, Guid>,
        IBuildSpecification<Guid, Guid>
    {
    }
}
