using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    //TODO: Выпилить когда все ПК объектов SD будут исправлены (станут int)
    [Obsolete]
    internal class GloballyIdentifierMassIncidentAccessValidatorWrapper : 
        IValidateObjectPermissions<Guid, MassIncident>,
        ISelfRegisteredService<IValidateObjectPermissions<Guid, MassIncident>>
    {
        private readonly IFindEntityByGlobalIdentifier<MassIncident> _finder;
        private readonly IValidateObjectPermissions<int, MassIncident> _originalValidator;

        public GloballyIdentifierMassIncidentAccessValidatorWrapper(
            IFindEntityByGlobalIdentifier<MassIncident> finder,
            IValidateObjectPermissions<int, MassIncident> originalValidator)
        {
            _finder = finder;
            _originalValidator = originalValidator;
        }

        public IEnumerable<Expression<Func<MassIncident, bool>>> ObjectIsAvailable(Guid userID) =>
            _originalValidator.ObjectIsAvailable(userID);

        public async Task<bool> ObjectIsAvailableAsync(Guid userID, Guid id, CancellationToken cancellationToken = default) =>
            await _originalValidator.ObjectIsAvailableAsync(
                userID,
                (await _finder.FindAsync(id, cancellationToken))?.ID ?? default,
                cancellationToken);
    }
}
