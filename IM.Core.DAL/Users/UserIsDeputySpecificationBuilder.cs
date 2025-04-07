using Inframanager;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.Users
{
    internal class UserIsDeputySpecificationBuilder : IBuildUserIsDeputySpecification, 
        ISelfRegisteredService<IBuildUserIsDeputySpecification>
    {
        private readonly DbSet<DeputyUser> _deputyUsers;

        public UserIsDeputySpecificationBuilder(DbSet<DeputyUser> deputyUsers)
        {
            _deputyUsers = deputyUsers;
        }

        public Specification<IEnumerable<User>> Build(Guid userID)
        {
            return new Specification<IEnumerable<User>>(
                users => users.Any(u => _deputyUsers.Any(DeputyUser.UserIsDeputy(u.IMObjID, userID))));
        }

        Specification<Guid> IBuildSpecification<Guid, Guid>.Build(Guid deputyUserID)
        {
            return new Specification<Guid>(userID => _deputyUsers.Any(DeputyUser.UserIsDeputy(userID, deputyUserID)));
        }
    }
}
