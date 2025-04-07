using Inframanager;
using InfraManager.DAL;
using System;

namespace InfraManager.BLL.ServiceDesk
{
    internal class ReportUserSpecificationBuilder<T> : IBuildSpecification<T, Guid>
    {
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
        private readonly ServiceDeskObjectAccessIsNotRestricted _accessNotRestricted;
        private readonly IBuildSpecification<T, User> _specificationBuilder;

        public ReportUserSpecificationBuilder(
            IBuildSpecification<T, User> specificationBuilder,
            IFindEntityByGlobalIdentifier<User> userFinder,             
            ServiceDeskObjectAccessIsNotRestricted accessNotRestricted)
        {
            _userFinder = userFinder;
            _specificationBuilder = specificationBuilder;
            _accessNotRestricted = accessNotRestricted; 
        }

        public Specification<T> Build(Guid userID)
        {
            var user = _userFinder
                .WithMany(u => u.UserRoles)
                    .ThenWith(ur => ur.Role)
                        .ThenWithMany(r => r.Operations)
                .Find(userID);
            return _accessNotRestricted.IsSatisfiedBy(user) ? new Specification<T>(true) : _specificationBuilder.Build(user);
        }
    }
}
