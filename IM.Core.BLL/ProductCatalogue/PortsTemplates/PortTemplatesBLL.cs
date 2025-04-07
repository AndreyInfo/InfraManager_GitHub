using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.PortTemplates;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL.AccessManagement;

namespace InfraManager.BLL.ProductCatalogue.PortsTemplatess;

public class PortTemplatesBLL :
    StandardBLL<PortTemplatesKey,PortTemplate,PortTemplatesData, PortTemplatesDetails, PortTemplatesFilter>,
    IPortTemplatesBLL, 
    ISelfRegisteredService<IPortTemplatesBLL>
{
    private readonly IMapper _mapper;
    private readonly IValidatePermissions<PortTemplate> _validatePermissions;
    private readonly IGuidePaggingFacade<PortTemplate, PortTemplateColumns> _guidePaggingFacade;
    private readonly IBuildEntityQuery<PortTemplate, PortTemplatesDetails, PortTemplatesFilter> _buildEntityQuery;

    public PortTemplatesBLL(
          IRepository<PortTemplate> repository
        , IMapper mapper
        , ILogger<PortTemplatesBLL> logger
        , IValidatePermissions<PortTemplate> validatePermissions
        , IGuidePaggingFacade<PortTemplate, PortTemplateColumns> guidePaggingFacade
        , IBuildEntityQuery<PortTemplate, PortTemplatesDetails, PortTemplatesFilter> buildEntityQuery
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<PortTemplatesDetails, PortTemplate> detailsBuilder
        , IInsertEntityBLL<PortTemplate, PortTemplatesData> insertEntityBLL
        , IModifyEntityBLL<PortTemplatesKey, PortTemplate, PortTemplatesData, PortTemplatesDetails> modifyEntityBLL
        , IRemoveEntityBLL<PortTemplatesKey, PortTemplate> removeEntityBLL
        , IGetEntityBLL<PortTemplatesKey, PortTemplate, PortTemplatesDetails> detailsBLL
        , IGetEntityArrayBLL<PortTemplatesKey, PortTemplate, PortTemplatesDetails, PortTemplatesFilter> detailsArrayBLL)
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
        _mapper = mapper;
        _guidePaggingFacade = guidePaggingFacade;
        _buildEntityQuery = buildEntityQuery;
        _validatePermissions = validatePermissions;
    }

    public async Task<PortTemplatesDetails[]> GetListAsync(PortTemplatesFilter filter, CancellationToken token)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, token);

        var portTemplates = await _guidePaggingFacade.GetPaggingAsync(filter
            , _buildEntityQuery.Query(filter)
            , null
            , token);

        return _mapper.Map<PortTemplatesDetails[]>(portTemplates);
    }
}