using AutoMapper;
using InfraManager.BLL.Software.SoftwareModels.ForTable;
using InfraManager.BLL.Software.SoftwareModelTabs.Relations;
using Inframanager.BLL;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Software;
using InfraManager.DAL;
using Inframanager;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL.Software.PackageContents;
using InfraManager.BLL.ColumnMapper;

namespace InfraManager.BLL.Software.SoftwareModelTabs.PackageContents;
public class SoftwareModelPackageContentsBLL : ISoftwareModelPackageContentsBLL, ISelfRegisteredService<ISoftwareModelPackageContentsBLL>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<SoftwareModelPackageContentsBLL> _logger;
    private readonly IValidatePermissions<SoftwareModel> _validatePermissions;
    private readonly ISoftwareModelRelationBLL _softwareModelRelationBLL;
    private readonly IOrderedColumnQueryBuilder<SoftwareModel, SoftwareModelForTable> _orderedColumnQueryBuilder;
    private readonly ISoftwareModelPackageContentsQuery _softwareModelPackageContentsQuery;

    public SoftwareModelPackageContentsBLL(
        IMapper mapper, 
        ICurrentUser currentUser, 
        ILogger<SoftwareModelPackageContentsBLL> logger, 
        IValidatePermissions<SoftwareModel> validatePermissions, 
        ISoftwareModelRelationBLL softwareModelRelationBLL, 
        IOrderedColumnQueryBuilder<SoftwareModel, SoftwareModelForTable> orderedColumnQueryBuilder,
        ISoftwareModelPackageContentsQuery softwareModelPackageContentsQuery
        )
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _logger = logger;
        _validatePermissions = validatePermissions;
        _softwareModelRelationBLL = softwareModelRelationBLL;
        _orderedColumnQueryBuilder = orderedColumnQueryBuilder;
        _softwareModelPackageContentsQuery = softwareModelPackageContentsQuery;
    }

    public async Task<SoftwareModelPackageContentsListItemDetails[]> GetPackageContentsForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = await _softwareModelRelationBLL.GetRelatedSoftwareModelsByIDAndTypeQueryAsync
            (filter.ID, 
            SoftwareModelDependencyType.SoftwarePackage, 
            true, 
            cancellationToken);

        var orderedQuery = await _orderedColumnQueryBuilder.BuildQueryAsync(filter.ViewName, query, cancellationToken);

        var packageContentsItems = await _softwareModelPackageContentsQuery.ExecuteAsync(_mapper.Map<PaggingFilter>(filter), orderedQuery, cancellationToken);

        return _mapper.Map<SoftwareModelPackageContentsListItemDetails[]>(packageContentsItems);
    }
}
