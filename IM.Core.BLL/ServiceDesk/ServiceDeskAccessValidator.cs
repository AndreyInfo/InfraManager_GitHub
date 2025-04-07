using Inframanager;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    internal class ServiceDeskAccessValidator<T> : SpecificationPermissionsValidator<Guid, T>
        where T : class, IGloballyIdentifiedEntity
    {
        private readonly IFinder<T> _finder;
        private readonly ISettingsBLL _settings;
        private readonly IConvertSettingValue<bool> _converter;

        public ServiceDeskAccessValidator(
            IFinder<T> finder,
            IObjectClassProvider<T> classProvider,
            IBuildAccessIsGrantedSpecification<T> accessIsGranted,
            IBuildUserInNegotiationSpecification<T> userInNegotiation,
            ISettingsBLL settings,
            IConvertSettingValue<bool> converter,
            IFindEntityByGlobalIdentifier<User> userFinder) 
            : base(
                  userFinder, 
                  classProvider,
                  SpecificationBuilder<T, User>.Any(
                      accessIsGranted,
                      userInNegotiation))
        {
            _finder = finder;
            _settings = settings;
            _converter = converter;
        }

        protected override async Task<T> FindEntityAsync(Guid key, CancellationToken cancellationToken = default)
        {
            return await _finder.FindAsync(key, cancellationToken);
        }

        protected override IFindEntityByGlobalIdentifier<User> Include(IFindEntityByGlobalIdentifier<User> userFinder)
        {
            return userFinder;
        }

        protected override bool NotRestricted(User user)
        {
            return false;
        }
    }
}
