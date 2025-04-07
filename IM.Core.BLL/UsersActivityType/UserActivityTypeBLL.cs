using System;
using Inframanager.BLL;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.UsersActivityType;

internal class UserActivityTypeBLL :
    StandardBLL<Guid, UserActivityType, UserActivityTypeData, UserActivityTypeDetails, UserActivityTypeFilter>,
    IUserActivityTypeBLL, ISelfRegisteredService<IUserActivityTypeBLL>
{
    public UserActivityTypeBLL(
        IRepository<UserActivityType> repository,
        ILogger<UserActivityTypeBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<UserActivityTypeDetails, UserActivityType> detailsBuilder,
        IInsertEntityBLL<UserActivityType, UserActivityTypeData> insertEntityBLL,
        IModifyEntityBLL<Guid, UserActivityType, UserActivityTypeData, UserActivityTypeDetails> modifyEntityBLL,
        IRemoveEntityBLL<Guid, UserActivityType> removeEntityBLL,
        IGetEntityBLL<Guid, UserActivityType, UserActivityTypeDetails> detailsBLL,
        IGetEntityArrayBLL<Guid, UserActivityType, UserActivityTypeDetails, UserActivityTypeFilter> detailsArrayBLL)
        : base(repository,
            logger,
            unitOfWork,
            currentUser,
            detailsBuilder,
            insertEntityBLL,
            modifyEntityBLL,
            removeEntityBLL,
            detailsBLL,
            detailsArrayBLL)
    {
    }
}