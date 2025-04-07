using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class RfcTypeBLL : 
        StandardBLL<Guid, ChangeRequestType, RfcTypeModel, RfcTypeDetailsModel, LookupListFilter>,
        IRfcTypeBLL,
        ISelfRegisteredService<IRfcTypeBLL>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RfcTypeBLL> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IGuidePaggingFacade<ChangeRequestType, ChangeRequestListItem> _paggingFacade;
        private readonly IValidatePermissions<ChangeRequestType> _validate;

        private Guid _currentUserID => _currentUser.UserId;

        public RfcTypeBLL(
            IRepository<ChangeRequestType> repository,
            IMapper mapper, 
            ICurrentUser currentUser,
            ILogger<RfcTypeBLL> logger,
            IGuidePaggingFacade<ChangeRequestType, ChangeRequestListItem> paggingFacade,
            IValidatePermissions<ChangeRequestType> validate, 
            IInsertEntityBLL<ChangeRequestType, RfcTypeModel> insertEntityBLL,
            IBuildObject<RfcTypeDetailsModel, ChangeRequestType> detailsBuilder,
            IModifyEntityBLL<Guid, ChangeRequestType, RfcTypeModel, RfcTypeDetailsModel> modifyEntityBLL,
            IRemoveEntityBLL<Guid, ChangeRequestType> removeEntityBLL,
            IGetEntityBLL<Guid, ChangeRequestType, RfcTypeDetailsModel> detailsBLL,
            IGetEntityArrayBLL<Guid, ChangeRequestType, RfcTypeDetailsModel, LookupListFilter> detailsArrayBLL,
            IUnitOfWork saveChanges)
            : base(
                repository,
                logger,
                saveChanges,
                currentUser,
                detailsBuilder,
                insertEntityBLL,
                modifyEntityBLL,
                removeEntityBLL,
                detailsBLL,
                detailsArrayBLL)
        {

            _currentUser = currentUser;
            _mapper = mapper;
            _logger = logger;
            _paggingFacade = paggingFacade;
            _validate = validate;
        }
        
        public async Task<RfcTypeDetailsModel[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken = default)
        {
            if (!await _validate.UserHasPermissionAsync(_currentUserID, ObjectAction.ViewDetailsArray, cancellationToken))
            {
                _logger.LogWarning($"user with id = {_currentUserID} dont have permissions to Get list array of RFC types");
                throw new AccessDeniedException("Array RFC Type list");
            }
            
            var result = await _paggingFacade.GetPaggingAsync(
                filter,
                Repository.Query(),
                x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
                cancellationToken: cancellationToken);
            
            _logger.LogTrace($"user with id = {_currentUserID} got {result.Length} RFC Types");
            
            return _mapper.Map<RfcTypeDetailsModel[]>(result);
        }
    }
}
