using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    internal class ObjectSummaryBLL<TEntity> : IObjectSummaryBLL where TEntity : class
    {
        private readonly IObjectSummaryInfoQuery<TEntity> _queryObjectStateInfo;
        private readonly IValidateObjectPermissions<Guid, TEntity> _objectAccessValidator;
        private readonly ICurrentUser _currentUser;
        
        public ObjectSummaryBLL(
            IObjectSummaryInfoQuery<TEntity> queryObjectStateInfo,
            IValidateObjectPermissions<Guid, TEntity> objectAccessValidator,
            ICurrentUser currentUser)
        {
            _queryObjectStateInfo = queryObjectStateInfo;
            _objectAccessValidator = objectAccessValidator;
            _currentUser = currentUser;
        }

        public async Task<ObjectSummaryInfo> GetObjectSummaryAsync(Guid objectID, CancellationToken cancellationToken = default)
        {
            if (await _objectAccessValidator.ObjectIsAvailableAsync(_currentUser.UserId, objectID, cancellationToken))
            {
                return await _queryObjectStateInfo.ExecuteAsync(objectID, _currentUser.UserId, cancellationToken);
            }

            throw new AccessDeniedException($"Get summary of {typeof(TEntity).Name} (IMObjID = objectID)");
        }
    }
}
