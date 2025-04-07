using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;

namespace InfraManager.BLL.ServiceDesk.Solutions;

internal sealed class SolutionBLL
    : StandardBLL<Guid, Solution, SolutionData, SolutionDetails, SolutionFilter>
    , ISolutionBLL
    , ISelfRegisteredService<ISolutionBLL>
{
    public SolutionBLL(IRepository<Solution> repository
        , ILogger<SolutionBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<SolutionDetails, Solution> detailsBuilder
        , IInsertEntityBLL<Solution, SolutionData> insertEntityBLL
        , IModifyEntityBLL<Guid, Solution, SolutionData, SolutionDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, Solution> removeEntityBLL, IGetEntityBLL<Guid, Solution, SolutionDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, Solution, SolutionDetails, SolutionFilter> detailsArrayBLL)
        : base(repository
            , logger
            , unitOfWork
            , currentUser
            , detailsBuilder
            , insertEntityBLL
            , modifyEntityBLL
            , removeEntityBLL
            , detailsBLL
            , detailsArrayBLL)
    {
    }
}
