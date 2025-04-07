using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallPermissionsValidator : SpecificationPermissionsValidator<Guid, Call>,
        ISelfRegisteredService<IValidateObjectPermissions<Guid, Call>>
    {
        private readonly IUserAccessBLL _userAccess;
        private readonly IFinder<Call> _finder;

        public CallPermissionsValidator(
            IFinder<Call> finder,
            IObjectClassProvider<Call> classProvider,
            IFindEntityByGlobalIdentifier<User> userFinder,            
            IUserAccessBLL userAccess,
            IBuildAccessIsGrantedSpecification<Call> legacyCallAccessIsGranted,
            IBuildUserInNegotiationSpecification<Call> userInCallNegotiation) 
            : base(
                  userFinder,
                  classProvider,
                  SpecificationBuilder<Call, User>.Any(
                    legacyCallAccessIsGranted,
                    userInCallNegotiation))
        {
            _finder = finder;
            _userAccess = userAccess;
        }

        protected override async Task<Call> FindEntityAsync(Guid key, CancellationToken cancellationToken = default)
        {
            return await _finder.FindAsync(key, cancellationToken);
        }

        protected override IFindEntityByGlobalIdentifier<User> Include(IFindEntityByGlobalIdentifier<User> userFinder)
        {
            return userFinder;
        }

        protected override bool NotRestricted(User user)
        {
            return _userAccess.UserHasOperation(user.IMObjID, OperationID.Call_ViewAll);
        }
    }
}
