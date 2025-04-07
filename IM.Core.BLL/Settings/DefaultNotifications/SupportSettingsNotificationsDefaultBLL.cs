using System;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.DAL.Notification;
using Model = InfraManager.DAL.Notification;
using InfraManager.DAL;
using System.Collections.Generic;
using System.Linq;
using InfraManager.BLL.Localization;
using Microsoft.Extensions.Logging;
using AutoMapper;
using InfraManager.BLL.AccessManagement;

namespace InfraManager.BLL.Settings.DefaultNotifications;

internal sealed class SupportSettingsNotificationsDefaultBLL : ISupportSettingsNotificationsDefaultBLL
    , ISelfRegisteredService<ISupportSettingsNotificationsDefaultBLL>
{
    private readonly IReadonlyRepository<Model.Notification> _notifications;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizeEnum<BusinessRole> _localizer;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<SupportSettingsNotificationsDefaultBLL> _logger;
    private readonly IMapper _mapper;
    private readonly IUserAccessBLL _userAccessBLL;
    public SupportSettingsNotificationsDefaultBLL(IReadonlyRepository<Model.Notification> notifications
        , IUnitOfWork unitOfWork
        , ILocalizeEnum<BusinessRole> localizer
        , ICurrentUser currentUser
        , ILogger<SupportSettingsNotificationsDefaultBLL> logger
        , IMapper mapper
        , IUserAccessBLL userAccessBLL)
    {
        _notifications = notifications;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
        _currentUser = currentUser;
        _logger = logger;
        _mapper = mapper;
        _userAccessBLL = userAccessBLL;
    }

    private BusinessRole[] GetBusinessRoles() => Enum.GetValues<BusinessRole>()
                                                     .Where(c => c != BusinessRole.None)
                                                     .ToArray();

    public async Task<NodeNotificationDefaultDetails<ObjectClass>[]> GetClassNodesAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request get first level with classes from the default notification tree");
        await _userAccessBLL.ThrowIfNoAdminAsync(_currentUser.UserId, _logger, cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} start  get first level with classes from the default notification tree");
        var notification = await _notifications.With(c=> c.Class)
                                               .ToArrayAsync(cancellationToken);

        var businessRoles = GetBusinessRoles();
        var notificationsGroupBuClassID = notification.GroupBy(c => c.ClassID).ToArray();

        var result = new List<NodeNotificationDefaultDetails<ObjectClass>>(notificationsGroupBuClassID.Length);
        foreach (var item in notificationsGroupBuClassID)
        {
            var nodesBusinessRolesLevel = await ConvertBusinessRoleNodesAsync(item.ToArray(), item.Key, cancellationToken);
            var node = _mapper.Map<NodeNotificationDefaultDetails<ObjectClass>>((item.Key
                                                                                 , item.First().Class.Name
                                                                                 , nodesBusinessRolesLevel));

            result.Add(node);
        }

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish get first level with classes from the default notification tree");
        return result.ToArray();
    }


    #region Дерево Бизнесс Роли
    public async Task<NodeNotificationDefaultDetails<BusinessRole>[]> GetBusinessRoleNodesAsync(ObjectClass classID, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request get second level with business roles from the default notification tree");
        await _userAccessBLL.ThrowIfNoAdminAsync(_currentUser.UserId, _logger, cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} start get second level with business roles from the default notification tree");
        
        var notifications = await _notifications.ToArrayAsync(c => c.ClassID == classID 
                                                                    && c.AvailableBusinessRole != BusinessRole.None
                                                                    , cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} received notification by classID = '{classID}'");

        return await ConvertBusinessRoleNodesAsync(notifications, classID, cancellationToken);
    }

    private async Task<NodeNotificationDefaultDetails<BusinessRole>[]> ConvertBusinessRoleNodesAsync(Model.Notification[] notifications
                                                                                              , ObjectClass classID
                                                                                              , CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} start convert notification to node from the default notification tree(level business role)");
        var businessRoles = GetBusinessRolesFromNotifications(notifications);
        var result = new List<NodeNotificationDefaultDetails<BusinessRole>>(businessRoles.Length);

        foreach (var role in businessRoles)
        {
            var notificationsByRole = notifications.Where(c => c.AvailableBusinessRole.HasFlag(role))
                                                   .ToArray();

            var item = _mapper.Map<NodeNotificationDefaultDetails<BusinessRole>>((role
                                                                                 , classID
                                                                                 , notificationsByRole
                                                                                 , await _localizer.LocalizeAsync(role, cancellationToken)));
            result.Add(item);
        }

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish convert notification to node from the default notification tree(level business role)");
        return result.ToArray();
    }

    private BusinessRole[] GetBusinessRolesFromNotifications(Model.Notification[] notifications)
    {
        var businessRoles = GetBusinessRoles();

        var result = new Queue<BusinessRole>();
        foreach (var role in businessRoles)
        {
            if (notifications.Any(c => c.AvailableBusinessRole.HasFlag(role)))
                result.Enqueue(role);
        }

        return result.ToArray();
    }
    #endregion


    #region Получение дерева Оповещения
    public async Task<NodeNotificationDefaultDetails<Guid>[]> GetNotificationNodesAsync(ObjectClass classID, BusinessRole businessRole, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request get third level with notification from the default notification tree");
        await _userAccessBLL.ThrowIfNoAdminAsync(_currentUser.UserId, _logger, cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} start get third level with notification from the default notification tree");
        var notifications = await _notifications.ToArrayAsync(c => c.ClassID == classID
                                                                  && c.AvailableBusinessRole != BusinessRole.None
                                                                  && c.AvailableBusinessRole.HasFlag(businessRole), cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} received notification by classID = '{classID}' and BusinessRole = '{businessRole}'");

        return ConvertNotificationToNodeDetails(notifications, businessRole);
    }

    private NodeNotificationDefaultDetails<Guid>[] ConvertNotificationToNodeDetails(Model.Notification[] models, BusinessRole businessRole)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} start convert notification to node from the default notification tree(level notification)");
        var result = new Queue<NodeNotificationDefaultDetails<Guid>>(models.Length);
        foreach (var item in models)
        {
            var element = _mapper.Map<NodeNotificationDefaultDetails<Guid>>((item, businessRole));

            result.Enqueue(element);
        }
        _logger.LogTrace($"UserID = {_currentUser.UserId} finish convert notification to node from the default notification tree(level notification)");
        //TODO убрать после того как будет решена проблема на фронте с пересчетом
        return result.OrderByDescending(c=> c.IsSelectFull).ToArray();
    }
    #endregion

    public async Task ToDefaultBuisnessRoleAsync(Guid notificationID, BusinessRole businessRole, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request add business role to notification");
        await _userAccessBLL.ThrowIfNoAdminAsync(_currentUser.UserId, _logger, cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} start add business role to notification");

        var entity = await _notifications.FirstOrDefaultAsync(c => c.ID == notificationID, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(notificationID, ObjectClass.Notification);

        entity.DefaultBusinessRole |= businessRole;

        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} finish add business role to notification");
    }

    public async Task ToNoDefaultBuisnessRoleAsync(Guid notificationID, BusinessRole businessRole, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request delete business role to notification");
        await _userAccessBLL.ThrowIfNoAdminAsync(_currentUser.UserId, _logger, cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} start delete business role to notification");

        var entity = await _notifications.FirstOrDefaultAsync(c => c.ID == notificationID, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(notificationID, ObjectClass.Notification);

        entity.DefaultBusinessRole &= ~businessRole;

        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} finish delete business role to notification");
    }
}
