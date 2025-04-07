using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.BLL.Extensions;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.Users;
using Microsoft.Extensions.Logging;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using Inframanager;
using InfraManager.DAL.ServiceDesk.WorkOrders.Templates;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

internal class WorkOrderTemplateBLL : IWorkOrderTemplateBLL, ISelfRegisteredService<IWorkOrderTemplateBLL>
{
    private readonly IReadonlyRepository<WorkOrderTemplateFolder> _folderListQuery;
    private readonly IRepository<WorkOrderTemplate> _workOrderTemplateRepository;
    private readonly IFinder<WorkOrderTemplateFolder> _folderFinder;
    private readonly IWorkOrderTemplateDataProvider _dataProvider;//TODO избавиться от DataProvider
    private readonly IClassIconBLL _classIconBLL;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IUserBLL _userBLL;
    private readonly ILogger<WorkOrderTemplate> _logger;
    private readonly IValidatePermissions<WorkOrderTemplate> _validatePermissions;
    private readonly IClientSideFilterer<WorkOrderTemplateDetails, WorkOrderTemplateListItem> _guidePaggingFacadeBLL;
    //TODO отрсовывать на фронте
    private readonly NodeWorkOrderTemplateTree _rootNode;

    public WorkOrderTemplateBLL(IReadonlyRepository<WorkOrderTemplateFolder> folderListQuery
                                , IRepository<WorkOrderTemplate> workOrderTemplateRepository
                                , IFinder<WorkOrderTemplateFolder> folderFinder
                                , IWorkOrderTemplateDataProvider dataProvider
                                , IClassIconBLL classIconBLL
                                , IMapper mapper
                                , IUnitOfWork unitOfWork
                                , ILocalizeText localization
                                , ICurrentUser currentUser
                                , IUserBLL userBLL
                                , ILogger<WorkOrderTemplate> logger
                                , IValidatePermissions<WorkOrderTemplate> validatePermissions
                                , IClientSideFilterer<WorkOrderTemplateDetails, WorkOrderTemplateListItem> guidePaggingFacadeBLL)
    {
        _folderListQuery = folderListQuery;
        _workOrderTemplateRepository = workOrderTemplateRepository;
        _folderFinder = folderFinder;
        _dataProvider = dataProvider;
        _classIconBLL = classIconBLL;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _rootNode = new()
        {
            ID = Guid.Empty,
            Name = localization.Localize(nameof(Resources.RootNameWorkOrderTemplateTree)),
            HasChild = true,
            ParentId = null,
        };
        _currentUser = currentUser;
        _userBLL = userBLL;
        _logger = logger;
        _validatePermissions = validatePermissions;
        _guidePaggingFacadeBLL = guidePaggingFacadeBLL;
    }

    #region 
    public async Task<NodeWorkOrderTemplateTree[]> GetTreeAsync(Guid? parentId, bool isMainFather, CancellationToken cancellationToken)
    {
        var result = new List<NodeWorkOrderTemplateTree>();
        if (isMainFather)
        {
            result.Add(_rootNode);
        }
        else
        {
            result.AddRange(await GetSubFolder(parentId, cancellationToken));
        }

        return result.ToArray();
    }


    private async Task<NodeWorkOrderTemplateTree[]> GetSubFolder(Guid? parentId, CancellationToken cancellationToken)
    {
        var result = new List<NodeWorkOrderTemplateTree>();
        if (parentId.HasValue)
        {
            var folder = await _folderFinder.FindAsync(parentId.Value, cancellationToken);
            if (folder is null)
                return null;
        }

        result.AddRange(await ExecuteGetSubFolderAsync(parentId));
        return result.ToArray();
    }

    private async Task<NodeWorkOrderTemplateTree[]> ExecuteGetSubFolderAsync(Guid? parentId)
    {
        var subFolders = await _folderListQuery.ToArrayAsync(c => c.ParentID == parentId);

        var result = _mapper.Map<NodeWorkOrderTemplateTree[]>(subFolders);

        return InitializateIconName(result, ObjectClass.WorkOrderTemplateFolder);
    }
    private NodeWorkOrderTemplateTree[] InitializateIconName(NodeWorkOrderTemplateTree[] models, ObjectClass classID)
    {
        var iconFolder = _classIconBLL.GetIconNameByClassID(classID);
        models.ForEach(c => c.IconName = iconFolder);
        return models.ToArray();
    }
    #endregion


    public async Task<NodeWorkOrderTemplateTree[]> GetPathItemByIDAsync(Guid id, ObjectClass classID, CancellationToken cancellationToken)
    {
        var result = new List<NodeWorkOrderTemplateTree>();
        if (classID == ObjectClass.WorkOrderTemplate)
        {
            var template = await _workOrderTemplateRepository.FirstOrDefaultAsync(c=> c.ID == id, cancellationToken) ??
                           throw new ObjectNotFoundException<Guid>(id, ObjectClass.WorkOrderTemplate);

            // Обязательно учитывать порядок, иначе фронт будет получать неправильные данные
            result.Add(_mapper.Map<NodeWorkOrderTemplateTree>(template));
            var folders = await _dataProvider.GetPathFolders(template.FolderID.Value); // TODO: Исправить потенциальный NULL Reference
            result.AddRange(_mapper.Map<NodeWorkOrderTemplateTree[]>(folders));
        }
        else if (classID == ObjectClass.WorkOrderTemplateFolder)
        {
            var folder = await _folderFinder.FindAsync(id, cancellationToken) ??
                         throw new ObjectNotFoundException<Guid>(id, ObjectClass.WorkOrderTemplateFolder);

            var folders = await _dataProvider.GetPathFolders(id);
            result.AddRange(_mapper.Map<NodeWorkOrderTemplateTree[]>(folders));
        }


        result.Add(_rootNode);
        return result.ToArray();

    }

    public async Task<WorkOrderTemplateDetails[]> GetDataForTableAsync(WorkOrderTemplateFilter filter, CancellationToken cancellationToken)
    {
        var templates = await GetTemplatesFromFolderAndSubFoldersAsync(filter.FolderID, cancellationToken);

        var result = _mapper.Map<WorkOrderTemplateDetails[]>(templates);
        
        return await _guidePaggingFacadeBLL.GetPaggingAsync(result
            , filter
            , c => c.Name.ToLower().Contains(filter.SearchString.ToLower())
            , cancellationToken);
    }

    private async Task<WorkOrderTemplate[]> GetTemplatesFromFolderAndSubFoldersAsync(Guid folderID, CancellationToken cancellationToken)
    {
        var folder = await _folderListQuery.WithMany(c => c.SubFolder)
                                           .WithMany(c => c.Templates)
                                                .ThenWith(c=> c.Initiator)
                                           .WithMany(c => c.Templates)
                                                .ThenWith(c=> c.Priority)
                                           .FirstOrDefaultAsync(c => c.ID == folderID, cancellationToken);

        var result = new List<WorkOrderTemplate>();
        var folders = new List<WorkOrderTemplateFolder>
        {
            folder
        };

        while (folders.Any())
        {
            folder = folders.First();
            result.AddRange(folder.Templates);
            folders.AddRange(folder.SubFolder);
            folders.Remove(folder);
        }

        return result.ToArray();
    }

    public async Task<bool> HasUsedTemplateAsync(Guid folderID, CancellationToken cancellationToken = default)
    {
        var folder = await _folderFinder.FindAsync(folderID, cancellationToken) ??
                     throw new ObjectNotFoundException<Guid>(folderID, ObjectClass.WorkOrderTemplateFolder);

        var root = await _folderListQuery.WithMany(c => c.SubFolder)
            .WithMany(c => c.Templates)
            .FirstOrDefaultAsync(c => c.ID == folderID, cancellationToken);

        var result = HasTemplatesIntoSubFoldersShort(root);

        return result;
    }

    private bool HasTemplatesIntoSubFoldersShort(WorkOrderTemplateFolder root)
    {
        var parents = new List<WorkOrderTemplateFolder>()
        {
            root
        };
        while (parents.Count != 0 && root is not null)
        {
            root = parents.FirstOrDefault();
            if (root.Templates.Any())
                return true;

            if (root is not null)
                parents.Remove(root);


            foreach (var child in root.SubFolder)
            {
                parents.Add(child);
            }
        }

        return false;
    }

    public async Task<WorkOrderTemplateDetails> UpdateAsync(WorkOrderTemplateDetails model, Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

        var entity = await _workOrderTemplateRepository.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.WorkOrderTemplate);

        _mapper.Map(model, entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return model;
    }

    public async Task<WorkOrderTemplateDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var entity = await _workOrderTemplateRepository.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.WorkOrderTemplate);

        var result = _mapper.Map<WorkOrderTemplateDetails>(entity);
        InitFlagsAndType(result, entity.ExecutorAssignmentType);
        return result;
    }

    /// <summary>
    /// Инициализация ExecutorAssignmentType 
    /// и флаги: FlagCalendarWorkSchedule, FlagServiceResponsible, FlagTOZ, FlagTTZ
    /// </summary>
    /// <param name="model">Модель где инициализируется флаги и поля</param>
    /// <param name="executorAssignmentType">номер типа, хранимый в базе</param>
    private void InitFlagsAndType(WorkOrderTemplateDetails model, ExecutorAssignmentType executorAssignmentType)
    {
        model.ExecutorAssignmentType = executorAssignmentType & ~ExecutorAssignmentType.Flags; // Получаем режим(Фиксированный сотрудник,Фиксированная группа и тд)
        var flags = executorAssignmentType & ExecutorAssignmentType.Flags;

        model.FlagTTZ = (flags & ExecutorAssignmentType.FlagTTZ) == ExecutorAssignmentType.FlagTTZ;
        model.FlagTOZ = (flags & ExecutorAssignmentType.FlagTOZ) == ExecutorAssignmentType.FlagTOZ;
        model.FlagServiceResponsible = (flags & ExecutorAssignmentType.FlagServiceResponsible) == ExecutorAssignmentType.FlagServiceResponsible;
        model.FlagCalendarWorkSchedule = (flags & ExecutorAssignmentType.FlagCalendarWorkSchedule) == ExecutorAssignmentType.FlagCalendarWorkSchedule;
    }

    public async Task<Guid> AddAsAsync(WorkOrderTemplateDetails model, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request insertAs {nameof(WorkOrderTemplate)}");

        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.InsertAs, cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} has permission to insertAs {nameof(WorkOrderTemplate)}");

        var entity = _mapper.Map<WorkOrderTemplate>(model);

        _workOrderTemplateRepository.Insert(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish insertedAs new {nameof(WorkOrderTemplate)}");

        return entity.ID;
    }
}
