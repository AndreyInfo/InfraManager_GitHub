using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.AccessManagement.AccessPermissions;
using InfraManager.BLL.Dashboards.ForTable;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Dashboards;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Dashboards;

internal class DashboardBLL :
    IDashboardBLL,
    ISelfRegisteredService<IDashboardBLL>
{
    private readonly IReadonlyRepository<Dashboard> _readOnlyDashboardRepository;
    private readonly IReadonlyRepository<DashboardFolder> _readOnlyDashboardFolderRepository;
    private readonly IRepository<DashboardDevEx> _dashboardDERepository;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly IMapper _mapper;
    private readonly IDashboardTreeQuery _dashboardTreeQuery;
    private readonly IDashboardTreeFolderQuery _dashboardTreeFolderQuery;
    private readonly ICurrentUser _currentUser;
    private readonly IUserAccessBLL _userAccess;
    private readonly IMemoryCache _cache;
    private readonly IGuidePaggingFacade<Dashboard, DashboardsForTable> _paggingFacade;
    private readonly IValidatePermissions<Dashboard> _validatePermissionsDashboard;
    private readonly ILogger<DashboardBLL> _logger;
    private readonly IAccessPermissionObjectForUserQuery _accessPermissionObjectForUserQuery;
    private readonly IAccessPermissionBLL _accessPermissionBLL;
    private readonly IInsertEntityBLL<Dashboard, DashboardData> _insertDashboardEntityBLL;
    private readonly IGetEntityBLL<Guid, Dashboard, DashboardDetails> _getDashboardEntityBLL;
    private readonly IModifyEntityBLL<Guid, Dashboard, DashboardData, DashboardDetails> _modifyDashboardEntityBLL;
    private readonly IRemoveEntityBLL<Guid, Dashboard> _removeDashboardEntityBLL;

    private readonly string _dbType;


    private const string C_CACHE_TreeList = nameof(C_CACHE_TreeList);
    private const int C_CACHE_Timeout_Sec = 5;
    private const string C_CONNECTION_MSSQL = "\"IM\"";
    private const string C_CONNECTION_PG = "\"IM_PG\"";
    public DashboardBLL(
    IUnitOfWork saveChangesCommand,
    ICurrentUser currentUser,
    IInsertEntityBLL<Dashboard, DashboardData> insertDashboardEntityBLL,
    IModifyEntityBLL<Guid, Dashboard, DashboardData, DashboardDetails> modifyDashboardEntityBLL,
    IRemoveEntityBLL<Guid, Dashboard> removeDashboardEntityBLL,
    IGetEntityBLL<Guid, Dashboard, DashboardDetails> getDashboardEntityBLL,
    IMapper mapper,
    IRepository<DashboardDevEx> dashboardDERepository,
    IDashboardTreeQuery dashboardTreeQuery,
    IDashboardTreeFolderQuery dashboardTreeFolderQuery,
    IUserAccessBLL userAccess,
    IMemoryCache cache,
    IGuidePaggingFacade<Dashboard, DashboardsForTable> paggingFacade,
    IValidatePermissions<Dashboard> validatePermissionsDashboard,
    ILogger<DashboardBLL> logger,
    IReadonlyRepository<Dashboard> readOnlyDashboardRepository,
    IReadonlyRepository<DashboardFolder> readOnlyDashboardFolderRepository,
    IConfiguration configuration,
    IAccessPermissionObjectForUserQuery accessPermissionObjectForUserQuery,
    IAccessPermissionBLL accessPermissionBLL
        )
    {
        _mapper = mapper;
        _dashboardDERepository = dashboardDERepository;
        _saveChangesCommand = saveChangesCommand;
        _dashboardTreeQuery = dashboardTreeQuery;
        _dashboardTreeFolderQuery = dashboardTreeFolderQuery;
        _currentUser = currentUser;
        _userAccess = userAccess;
        _cache = cache;
        _insertDashboardEntityBLL = insertDashboardEntityBLL;
        _getDashboardEntityBLL = getDashboardEntityBLL;
        _modifyDashboardEntityBLL = modifyDashboardEntityBLL;
        _removeDashboardEntityBLL = removeDashboardEntityBLL;
        _paggingFacade = paggingFacade;
        _validatePermissionsDashboard = validatePermissionsDashboard;
        _logger = logger;
        _readOnlyDashboardRepository = readOnlyDashboardRepository;
        _readOnlyDashboardFolderRepository = readOnlyDashboardFolderRepository;
        _accessPermissionObjectForUserQuery = accessPermissionObjectForUserQuery;
        _accessPermissionBLL = accessPermissionBLL;
        _dbType = configuration["dbType"];
    }

    public async Task<DashboardFullDetails> InsertAsync(DashboardFullData data, CancellationToken cancellationToken)
    {
        var dashboardData = _mapper.Map<DashboardData>(data);

        var dashboardEntity = await _insertDashboardEntityBLL.CreateAsync(dashboardData, cancellationToken);

        if (string.IsNullOrEmpty(data.Data))
        {
            data.Data =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "\r\n<Dashboard>\r\n" +
                "  <Title Text=\"New Dashboard\" />\r\n" +
                "</Dashboard>";
        }
        var dashboardDE = new DashboardDevEx(dashboardEntity.ID, data.Data);
        _dashboardDERepository.Insert(dashboardDE);

        await _saveChangesCommand.SaveAsync(cancellationToken);

        return _mapper.Map<DashboardFullDetails>(dashboardEntity, opt => opt.AfterMap((src, dest) => dest.Data = dashboardDE.Data));
    }

    public async Task<DashboardFullDetails> AllDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        var details = await _getDashboardEntityBLL.DetailsAsync(id, cancellationToken);
        var data = await _dashboardDERepository.FirstOrDefaultAsync(x => x.DashboardID == id, cancellationToken);
        return _mapper.Map<DashboardFullDetails>(details, opt => opt.AfterMap((src, dest) => dest.Data = data.Data));
    }

    public async Task<DashboardFullDetails> UpdateDashboardAsync(Guid id, DashboardFullData data, CancellationToken cancellationToken)
    {
        var dashboardData = _mapper.Map<DashboardData>(data);
        
        var dashboardDE = await _dashboardDERepository.FirstOrDefaultAsync(x => x.DashboardID == id, cancellationToken);
        dashboardDE.Data = data.Data;
        
        var dashboard = await _modifyDashboardEntityBLL.ModifyAsync(id, dashboardData, cancellationToken);
        
        await _saveChangesCommand.SaveAsync(cancellationToken);

        return _mapper.Map<DashboardFullDetails>(dashboard, opt => opt.AfterMap((src, dest) => dest.Data = dashboardDE.Data));
    }

    public async Task UpdateDashboardData(Guid id, string data, CancellationToken cancellationToken = default)
    {
        var dashboard = await _dashboardDERepository.FirstOrDefaultAsync(x => x.DashboardID == id, cancellationToken);
        if (dashboard != null)
        {
            dashboard.Data = data;
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _removeDashboardEntityBLL.RemoveAsync(id, cancellationToken);
        await _saveChangesCommand.SaveAsync(cancellationToken);
    }
    
    public async Task<DashboardsForTableDetails[]> GetDetailsArrayAsync(DashboardListFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissionsDashboard.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = _readOnlyDashboardRepository.Query().Where(x => x.DashboardFolderID == filter.FolderID);
        var isAdmin = await _userAccess.HasAdminRoleAsync(_currentUser.UserId, cancellationToken);
        if (filter.ByAccess && !isAdmin)
        {
            var accessObjects = await _accessPermissionObjectForUserQuery.ExecuteAsync(_currentUser.UserId
                , ObjectClass.DevExpressDashboard
                , cancellationToken);
            if(accessObjects.Any())
                query = query.Where( x => accessObjects.Select(c => c.ObjectID).Contains(x.ID ));
        }


        var dashboards = await _paggingFacade.GetPaggingAsync(
                filter,
                query,
                x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
                cancellationToken: cancellationToken);

        DashboardsForTableDetails[] resultReports;
        if (filter.FolderID is not null)
        {
            var folder =
                await _readOnlyDashboardFolderRepository.FirstOrDefaultAsync(x => x.ID == filter.FolderID,
                    cancellationToken);

            var folderWithParents = _mapper.Map<DashboardFolderWithParents>(folder);
            folderWithParents.BuildParents();

            resultReports = _mapper.Map<DashboardsForTableDetails[]>(dashboards);
            foreach (var report in resultReports)
            {
                report.StringFolder = GetPath(folderWithParents);
            }
        }
        else
        {
            resultReports = _mapper.Map<DashboardsForTableDetails[]>(dashboards);
        }

        return resultReports;
    }

    private string GetDBType()
    {
        if (_dbType == "pg")
        {
            return C_CONNECTION_PG;
        }

        if (_dbType == "mssql" || _dbType == "ms")
        {
            return C_CONNECTION_MSSQL;
        }

        throw new InvalidObjectException($"Database engine type {_dbType} is not supported or specified");
    }

    private string GetPath(DashboardFolderWithParents dashboardFolderWithParents)
    {
        var pathStringBuilder = new StringBuilder();
        foreach (var parent in dashboardFolderWithParents.Parents.Reverse())
        {
            pathStringBuilder.Append("/" + parent.Name);
        }

        pathStringBuilder.Append("/" + dashboardFolderWithParents.Name);
        return pathStringBuilder.ToString();
    }

    public async Task<IEnumerable<DashboardTreeItemDetails>> GetTreeListAsync(Guid? parentFolderID, Guid userID, CancellationToken cancellationToken = default)
    {
        if (!await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken))
            throw new AccessDeniedException($"User Id={_currentUser.UserId} Don't have rights");
        var result = await _cache.GetOrCreateAsync(
            C_CACHE_TreeList, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(C_CACHE_Timeout_Sec);

                var list = new List<DashboardTreeResultItem>();
                list.AddRange(await _dashboardTreeFolderQuery.ExecuteAsync(_currentUser.UserId, cancellationToken));
                list.AddRange(await _dashboardTreeQuery
                        .ExecuteAsync(ObjectClass.DevExpressDashboard, _currentUser.UserId, cancellationToken));
                
                DashboardTreeResultItem[] filtered;
                if (!await _userAccess.HasAdminRoleAsync(_currentUser.UserId))
                {
                    var permissions = await EvaluatePermissionsAsync(list, cancellationToken);
                    filtered = ApplyByPermissionsFiltering(list, permissions
                        ,new[] { AccessPermissionRight.HasPropertiesPermissions }).ToArray();
                }
                else filtered = list.ToArray();
                return _mapper.Map<DashboardTreeItemDetails[]>(filtered);
            });
        return result.Where(f => f.ParentID == parentFolderID).ToList();
    }

    private IEnumerable<DashboardTreeResultItem> ApplyByPermissionsFiltering(
        IEnumerable<DashboardTreeResultItem> dashboardTreeResultItems
        , IEnumerable<AccessPermissionDetails> permissions
        , AccessPermissionRight[] rights)
    {
        foreach (var r in rights)
        {
            permissions = permissions.Where(GetPredicateFor(r));
        }
        var filtered = permissions.ToArray();
        foreach (var item in dashboardTreeResultItems)
        {
            if (!item.IsDashboard && item.HasChilds) yield return item;
            if (filtered.Where(p => p.ObjectId == item.ID).Any()) yield return item;
        }
    }

    private enum AccessPermissionRight 
    { 
        HasPropertiesPermissions 
        , HasAddPermissions 
        , HasDeletePermissions 
        , HasUpdatePermissions 
        , HasAccessManagePermissions 
    }

    private Func<AccessPermissionDetails, bool> GetPredicateFor(AccessPermissionRight right
        = AccessPermissionRight.HasPropertiesPermissions) =>
        right switch
        {
            AccessPermissionRight.HasPropertiesPermissions => x => x.Rights.HasPropertiesPermissions,
            AccessPermissionRight.HasUpdatePermissions => x => x.Rights.HasUpdatePermissions,
            AccessPermissionRight.HasAddPermissions => x => x.Rights.HasAddPermissions,
            AccessPermissionRight.HasAccessManagePermissions => x => x.Rights.HasAccessManagePermissions,
            AccessPermissionRight.HasDeletePermissions => x => x.Rights.HasDeletePermissions,            
            _ => x => true
        };

    private async Task<List<AccessPermissionDetails>> EvaluatePermissionsAsync(List<DashboardTreeResultItem> list, CancellationToken token = default)
    {
        var permissions = new List<AccessPermissionDetails>();
        foreach (var item in list)
        {
            var permission = await _accessPermissionBLL
            .GetDataTableAsync(new BaseFilterWithClassIDAndID<Guid>
            {
                ClassID = ObjectClass.DevExpressDashboard,
                ObjectID = item.ID,
                CountRecords = 40,
                ViewName = "AccessPermissionColumns",
                StartRecordIndex = 0,
                SearchString = string.Empty
            }, token);
            permissions.AddRange(permission);
        }
        return permissions;
    }
}
