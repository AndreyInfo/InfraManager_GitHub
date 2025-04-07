using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL.OrganizationStructure.JobTitles;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL.AccessManagement;

namespace InfraManager.BLL.OrganizationStructure;

internal class JobTitleBLL :
    StandardBLL<int, JobTitle, JobTitleData, JobTitleDetails, JobTitleListFilter>
    , IJobTitleBLL
    , ISelfRegisteredService<IJobTitleBLL>
{

    private readonly IMapper _mapper;
    private readonly IValidatePermissions<JobTitle> _validatePermissions;
    private readonly IGuidePaggingFacade<JobTitle, JobTitleColumns> _guidePaggingFacade;
    public JobTitleBLL(
        IRepository<JobTitle> repository,
        ILogger<JobTitleBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<JobTitleDetails, JobTitle> detailsBuilder,
        IInsertEntityBLL<JobTitle, JobTitleData> insertEntityBLL,
        IModifyEntityBLL<int, JobTitle, JobTitleData, JobTitleDetails> modifyEntityBLL,
        IRemoveEntityBLL<int, JobTitle> removeEntityBLL,
        IGetEntityBLL<int, JobTitle, JobTitleDetails> detailsBLL,
        IGetEntityArrayBLL<int, JobTitle, JobTitleDetails, JobTitleListFilter> detailsArrayBLL,
        IMapper mapper,
        IValidatePermissions<JobTitle> validatePermissions,
        IGuidePaggingFacade<JobTitle, JobTitleColumns> guidePaggingFacade)
        : base(
            repository,
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
        _mapper = mapper;
        _validatePermissions = validatePermissions;
        _guidePaggingFacade = guidePaggingFacade;
    }

    public async Task<JobTitleDetails[]> GetPaggingAsync(BaseFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var entities = await _guidePaggingFacade.GetPaggingAsync(filter
            , Repository.Query()
            , c => c.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<JobTitleDetails[]>(entities);
    }
}
