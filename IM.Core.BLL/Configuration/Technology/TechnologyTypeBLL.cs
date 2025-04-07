using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Configuration.Technologies;
using InfraManager.BLL.Technologies;
using InfraManager.DAL;
using InfraManager.DAL.Configuration;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Configuration.Technology;

internal class TechnologyTypeBLL :
    StandardBLL<int, TechnologyType, TechnologyTypeData, TechnologyTypeDetails, TechnologyTypeFilter>
    , ITechnologyTypeBLL
    , ISelfRegisteredService<ITechnologyTypeBLL>
{
    public TechnologyTypeBLL(IRepository<TechnologyType> repository
        , ILogger<TechnologyTypeBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<TechnologyTypeDetails, TechnologyType> detailsBuilder
        , IInsertEntityBLL<TechnologyType, TechnologyTypeData> insertEntityBLL
        , IModifyEntityBLL<int, TechnologyType, TechnologyTypeData, TechnologyTypeDetails> modifyEntityBLL
        , IRemoveEntityBLL<int, TechnologyType> removeEntityBLL
        , IGetEntityBLL<int, TechnologyType, TechnologyTypeDetails> detailsBLL
        , IGetEntityArrayBLL<int, TechnologyType, TechnologyTypeDetails, TechnologyTypeFilter> detailsArrayBLL)
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
