using AutoMapper;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Settings;
//TODO переделать на новый InfraManager.DAL.Settings.BusinessRole
using BusinessRole = InfraManager.Core.BusinessRole;
//TODO убрать использовать локализацию
using FriendlyNameAttribute = InfraManager.Core.FriendlyNameAttribute;
using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.Extensions.Logging;
using NotifictionDAL = InfraManager.DAL.Notification.Notification;
using NotifictionUser = InfraManager.DAL.Notification.NotificationUser;

namespace InfraManager.BLL.Notification
{
    internal class NotificationBLL : INotificationBLL, ISelfRegisteredService<INotificationBLL>
    {
        private readonly IFinder<NotifictionDAL> _finder;
        private readonly IRepository<NotifictionDAL> _repository;
        private readonly IRepository<NotifictionUser> _notificationUserRepository;
        private readonly IClassIM _classIM;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IUserAccessBLL _userAccessBLL;
        private readonly IGuidePaggingFacade<NotifictionDAL, NotificationsListItem> _paggingFacade;
        private readonly IValidatePermissions<NotifictionDAL> _validatePermissions;
        private readonly ILogger<NotifictionDAL> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IServiceMapper<ObjectClass, INotificationProvider> _serviceMapping;
        
        private const int roleIsNull = 0;


        public NotificationBLL(
            IFinder<NotifictionDAL> finder,
            IMapper mapper,
            IClassIM classIM,
            IRepository<NotifictionDAL> repository,
            IUnitOfWork saveChangesCommand,
            IUserAccessBLL userAccessBLL,
            IRepository<NotifictionUser> notificationUserRepository,
            IGuidePaggingFacade<NotifictionDAL, NotificationsListItem> paggingFacade,
            IValidatePermissions<NotifictionDAL> validatePermissions,
            ILogger<NotifictionDAL> logger,
            ICurrentUser currentUser,
            IServiceMapper<ObjectClass, INotificationProvider> serviceMapping)
        {
            _finder = finder;
            _mapper = mapper;
            _classIM = classIM;
            _repository = repository;
            _saveChangesCommand = saveChangesCommand;
            _userAccessBLL = userAccessBLL;
            _notificationUserRepository = notificationUserRepository;
            _paggingFacade = paggingFacade;
            _validatePermissions = validatePermissions;
            _logger = logger;
            _currentUser = currentUser;
            _serviceMapping = serviceMapping;
        }

        public async Task<NotificationData[]> GetNotificationsAsync(BaseFilter filter, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray,
                cancellationToken);

            var result = await _paggingFacade.GetPaggingAsync(
                filter,
                _repository.Query(),
                x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
                cancellationToken);
            
            _logger.LogInformation($"User with id = {_currentUser.UserId} Got {result.Length} notifications");
            return _mapper.Map<NotificationData[]>(result);
        }

        public async Task<NotificationDetails> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails,
                cancellationToken);
            
            var data = await _finder.FindAsync(id, cancellationToken) ??
                       throw new ObjectNotFoundException($"Notification (ID = {id})");

            var notificationDetails = _mapper.Map<NotificationDetails>(data);

            foreach(var notificationRecipient in notificationDetails.NotificationRecipient)
            {
                if (notificationRecipient.Type != RecipientType.Email)
                {
                    notificationRecipient.BusinessRoleID =
                        (int)Enum.Parse(typeof(BusinessRole), notificationRecipient.Name);
                    notificationRecipient.Name = GetName((BusinessRole)notificationRecipient.BusinessRoleID);
                }
            }

            var availableRoles = GetRolesByClass(notificationDetails.ClassID);
            foreach (var role in availableRoles)
            {
                if ((role.Key & notificationDetails.AvailableBusinessRole) == role.Key)
                {
                    notificationDetails.SelectedRoles.Add(role.Key, role.Value);
                }
            }

            _logger.LogInformation(
                $"User with id = {_currentUser.UserId} Got notifications with id = {notificationDetails.ID}");
            
            return notificationDetails;
        }

        public async Task<InframanagerObjectClassData[]> GetClassesAsync(CancellationToken cancellationToken = default)
        {
            var ids = new List<ObjectClass> 
            {
                ObjectClass.Call,
                ObjectClass.Problem,
                ObjectClass.ChangeRequest,
                ObjectClass.WorkOrder,
                ObjectClass.Negotiation,
                ObjectClass.CustomController,
                ObjectClass.Substitution,
                ObjectClass.MassIncident
            };

            return await _classIM.GetClassesByIDsAsync(ids, cancellationToken);
        }

        public Dictionary<int, string> GetRolesByClass(ObjectClass classID)
        {
            return _serviceMapping.Map(classID).GetRoles();
        }

        //TODO убрать после переноса enum BusinessRole в IM.Core.BLL.Interfaces
        private string GetName(Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    var attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(FriendlyNameAttribute)) as FriendlyNameAttribute;
                    if (attr != null)
                    {
                        return attr.Name;
                    }
                }
            }
            return null;
        }

        public async Task<Guid> AddNotificationAsync(NotificationDetails notificationData, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert,
                cancellationToken);
            
            var notification = new NotifictionDAL();
            if (notificationData.SelectedRoles != null)
            {
                foreach (var role in notificationData.SelectedRoles)
                {
                    notificationData.AvailableBusinessRole |= role.Key;
                }
            }
            var entity = _mapper.Map(notificationData, notification);
            _repository.Insert(entity);

            await _saveChangesCommand.SaveAsync(cancellationToken);

            _logger.LogInformation(
                $"User with id = {_currentUser.UserId} successfully created new  notifications with ID = {entity.ID}");
                
            return entity.ID;
        }
        
        public async Task<Guid> UpdateNotificationAsync(NotificationDetails notificationData, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update,
                cancellationToken);
            
            notificationData.AvailableBusinessRole = 0;
            foreach (var role in notificationData.SelectedRoles)
            {
                notificationData.AvailableBusinessRole |= role.Key;
            }
            var foundEntity = await _finder.FindAsync(notificationData.ID, cancellationToken);
            
            _mapper.Map(notificationData, foundEntity);
            await _saveChangesCommand.SaveAsync(cancellationToken);

            _logger.LogInformation(
                $"User with id = {_currentUser.UserId} successfully updated new notifications with ID = {foundEntity.ID}");
            
            return foundEntity.ID;
        }

        public async Task DeleteNotificationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete,
                cancellationToken);
            
            var foundEntity = await _finder.FindAsync(id, cancellationToken);
            _repository.Delete(foundEntity);
            
            await _saveChangesCommand.SaveAsync(cancellationToken);

            _logger.LogInformation(
                $"User with id = {_currentUser.UserId} successfully deleted new notifications with ID = {foundEntity.ID}");
        }
        

        public ParameterTemplate[] GetAvailableParameterList(ObjectClass objectClass)
        {
            return _serviceMapping.Map(objectClass).GetAvailableParameterList();
        }

        #region Оповещения по умолчанию
        public async Task<DefaultNotificationTreeItem[]> GetDefaultNotificationTreeAsync(NotificationTreeLevel level, int parentId, ObjectClass? classId, CancellationToken cancellationToken = default)
        {
            var result = new List<DefaultNotificationTreeItem>();
            switch (level)
            {
                case NotificationTreeLevel.Object:
                    {
                        var classes = await GetClassesAsync(cancellationToken);
                        foreach (var item in classes)
                        {
                            var defaultNotificationTreeItem = new DefaultNotificationTreeItem() { Level = level, ID = (int)item.ID, Name = item.Name, ParentId = parentId, ClassId = item.ID };
                            result.Add(defaultNotificationTreeItem);
                        }
                        break;
                    }
                case NotificationTreeLevel.Role:
                    {
                        var roles = GetRolesByClass((ObjectClass)parentId);
                        foreach (var role in roles)
                        {
                            var defaultNotificationTreeItem = new DefaultNotificationTreeItem() { Level = level, ID = role.Key, Name = role.Value, ParentId = parentId, ClassId = (ObjectClass)parentId };
                            result.Add(defaultNotificationTreeItem);
                        }
                        break;
                    }
                case NotificationTreeLevel.Notification:
                    {
                        var notifications = await _repository.ToArrayAsync(p => p.ClassID == classId, cancellationToken);
                        var defaultNotifications = _mapper.Map<DefaultNotificationData[]>(notifications);
                        foreach (var notification in defaultNotifications)
                        {
                            if ((parentId & notification.AvailableBusinessRole) == parentId)
                            {
                                var defaultNotificationTreeItem = new DefaultNotificationTreeItem() { Level = level, NotificationID = notification.ID, Name = notification.Name, ParentId = parentId };
                                if ((parentId & notification.DefaultBusinessRole) == parentId)
                                {
                                    defaultNotificationTreeItem.Selected = true;
                                }
                                result.Add(defaultNotificationTreeItem);
                            }
                        }
                        break;
                    }
            }
            return result.ToArray();
        }
        private async Task<NotificationDetails[]> GetNotificationsDetailsAsync(List<NotificationSaveData> notificationSaveDataList, CancellationToken cancellationToken = default)
        {
            var ids = notificationSaveDataList.Select(p => p.ID);
            var result = await _repository.ToArrayAsync(p => ids.Contains(p.ID), cancellationToken);
            return _mapper.Map<NotificationDetails[]>(result);
        }
        
        public async Task SaveDefaultNotificationsAsync(List<NotificationSaveData> notificationSaveDataList, CancellationToken cancellationToken = default)
        {
            var notifications = await GetNotificationsDetailsAsync(notificationSaveDataList, cancellationToken);
            foreach (var notificationDetail in notifications)
            {
                if (notificationSaveDataList.Any(p => p.ID == notificationDetail.ID))
                {
                    var availableRoles = GetRolesByClass(notificationDetail.ClassID);
                    var defaultBusinessRole = 0;
                    foreach (var role in availableRoles)
                    {
                        var notificationRoleState = notificationSaveDataList
                                .FirstOrDefault(p => p.ID == notificationDetail.ID && p.Role == role.Key)?.Checked;
                        if (notificationRoleState != false)
                        {
                            defaultBusinessRole |= role.Key;
                        }
                    }
                    var entity = await _finder.FindAsync(notificationDetail.ID, cancellationToken);
                    entity.DefaultBusinessRole = (DAL.Notification.BusinessRole)defaultBusinessRole;
                }
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }
        #endregion

        #region Оповещения пользователя
        /// <summary>
        /// Возвращает роли доступные пользователю
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //TODO отрефакторить к #@$%#@!%
        private async Task<Dictionary<int, string>> GetBusinessRoleForNotificationsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var businessRoles = new Dictionary<int, string>();

            businessRoles.Add((int)BusinessRole.CallClient, GetName(BusinessRole.CallClient));
            businessRoles.Add((int)BusinessRole.CallInitiator, GetName(BusinessRole.CallInitiator));
            businessRoles.Add((int)BusinessRole.ControllerParticipant, GetName(BusinessRole.ControllerParticipant));
            businessRoles.Add((int)BusinessRole.RFCInitiator, GetName(BusinessRole.RFCInitiator));
            businessRoles.Add((int)BusinessRole.DeputyUser, GetName(BusinessRole.DeputyUser));
            businessRoles.Add((int)BusinessRole.ReplacedUser, GetName(BusinessRole.ReplacedUser));
            businessRoles.Add((int)BusinessRole.ProblemInitiator, GetName(BusinessRole.ProblemInitiator));
            businessRoles.Add((int)BusinessRole.MassIncidentInitiator, GetName(BusinessRole.MassIncidentInitiator));
            
            var operations = (await _userAccessBLL.GrantedOperationsAsync(userId, cancellationToken)).ToList();
            
            if (operations.Contains(OperationID.MassIncident_BeOwner))
            {
                if (!businessRoles.ContainsKey((int)BusinessRole.MassIncidentOwner))
                {
                    businessRoles.Add((int)BusinessRole.MassIncidentOwner,
                        GetName(BusinessRole.MassIncidentOwner));
                }
            }
            
            if (operations.Contains(OperationID.SD_General_Owner))
            {
                if (!businessRoles.ContainsKey((int)BusinessRole.CallAccomplisher)) 
                    businessRoles.Add((int)BusinessRole.CallAccomplisher, GetName(BusinessRole.CallAccomplisher));
                if (!businessRoles.ContainsKey((int)BusinessRole.WorkOrderInitiator)) 
                    businessRoles.Add((int)BusinessRole.WorkOrderInitiator, GetName(BusinessRole.WorkOrderInitiator));
                if (!businessRoles.ContainsKey((int)BusinessRole.WorkOrderAssignor)) 
                    businessRoles.Add((int)BusinessRole.WorkOrderAssignor, GetName(BusinessRole.WorkOrderAssignor));
                if (!businessRoles.ContainsKey((int)BusinessRole.ProblemOwner)) 
                    businessRoles.Add((int)BusinessRole.ProblemOwner, GetName(BusinessRole.ProblemOwner));
                if (!businessRoles.ContainsKey((int)BusinessRole.RFCOwner)) 
                    businessRoles.Add((int)BusinessRole.RFCOwner, GetName(BusinessRole.RFCOwner));
                if (!businessRoles.ContainsKey((int)BusinessRole.CallOwner)) 
                    businessRoles.Add((int)BusinessRole.CallOwner, GetName(BusinessRole.CallOwner));
            }
            if (operations.Contains(OperationID.SD_General_Executor))
            {
                if (!businessRoles.ContainsKey((int)BusinessRole.MassIncidentExecutor))
                    businessRoles.Add((int)BusinessRole.MassIncidentExecutor, GetName(BusinessRole.MassIncidentExecutor));
                if (!businessRoles.ContainsKey((int)BusinessRole.ProblemExecutor))
                    businessRoles.Add((int)BusinessRole.ProblemExecutor, GetName(BusinessRole.ProblemExecutor));
                if (!businessRoles.ContainsKey((int)BusinessRole.CallExecutor))
                    businessRoles.Add((int)BusinessRole.CallExecutor, GetName(BusinessRole.CallExecutor));
                if (!businessRoles.ContainsKey((int)BusinessRole.CallAccomplisher))
                    businessRoles.Add((int)BusinessRole.CallAccomplisher, GetName(BusinessRole.CallAccomplisher));
                if (!businessRoles.ContainsKey((int)BusinessRole.WorkOrderInitiator))
                    businessRoles.Add((int)BusinessRole.WorkOrderInitiator, GetName(BusinessRole.WorkOrderInitiator));
                if (!businessRoles.ContainsKey((int)BusinessRole.WorkOrderAssignor))
                    businessRoles.Add((int)BusinessRole.WorkOrderAssignor, GetName(BusinessRole.WorkOrderAssignor));
                if (!businessRoles.ContainsKey((int)BusinessRole.WorkOrderExecutor))
                    businessRoles.Add((int)BusinessRole.WorkOrderExecutor, GetName(BusinessRole.WorkOrderExecutor));
            }

            if (operations.Contains(OperationID.SD_General_Administrator))
            {
                if (!businessRoles.ContainsKey((int)BusinessRole.MassIncidentExecutor))
                    businessRoles.Add((int)BusinessRole.MassIncidentExecutor, GetName(BusinessRole.MassIncidentExecutor));
                if (!businessRoles.ContainsKey((int)BusinessRole.MassIncidentOwner)) 
                    businessRoles.Add((int)BusinessRole.MassIncidentOwner, GetName(BusinessRole.MassIncidentOwner));
                if (!businessRoles.ContainsKey((int)BusinessRole.ProblemExecutor))
                    businessRoles.Add((int)BusinessRole.ProblemExecutor, GetName(BusinessRole.ProblemExecutor));
                if (!businessRoles.ContainsKey((int)BusinessRole.SDAdministrator))
                    businessRoles.Add((int)BusinessRole.SDAdministrator, GetName(BusinessRole.SDAdministrator));
                if (!businessRoles.ContainsKey((int)BusinessRole.CallOwner))
                    businessRoles.Add((int)BusinessRole.CallOwner, GetName(BusinessRole.CallOwner));
                if (!businessRoles.ContainsKey((int)BusinessRole.CallExecutor))
                    businessRoles.Add((int)BusinessRole.CallExecutor, GetName(BusinessRole.CallExecutor));
                if (!businessRoles.ContainsKey((int)BusinessRole.CallAccomplisher))
                    businessRoles.Add((int)BusinessRole.CallAccomplisher, GetName(BusinessRole.CallAccomplisher));
                if (!businessRoles.ContainsKey((int)BusinessRole.WorkOrderInitiator))
                    businessRoles.Add((int)BusinessRole.WorkOrderInitiator, GetName(BusinessRole.WorkOrderInitiator));
                if (!businessRoles.ContainsKey((int)BusinessRole.WorkOrderAssignor))
                    businessRoles.Add((int)BusinessRole.WorkOrderAssignor, GetName(BusinessRole.WorkOrderAssignor));
                if (!businessRoles.ContainsKey((int)BusinessRole.WorkOrderExecutor))
                    businessRoles.Add((int)BusinessRole.WorkOrderExecutor, GetName(BusinessRole.WorkOrderExecutor));
                if (!businessRoles.ContainsKey((int)BusinessRole.ProblemOwner))
                    businessRoles.Add((int)BusinessRole.ProblemOwner, GetName(BusinessRole.ProblemOwner));
            }
            if (operations.Contains(OperationID.SD_General_VotingUser))
            {
                if (!businessRoles.ContainsKey((int)BusinessRole.NegotiationParticipant)) businessRoles.Add((int)BusinessRole.NegotiationParticipant, GetName(BusinessRole.NegotiationParticipant));
            }
            return businessRoles;
        }

        /// <summary>
        /// Возвращает роли, доступные для объекта по отношению к пользователю
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        private Dictionary<int, string> GetResultRoles(int parentID, Dictionary<int, string> userRoles)
        {
            var objectRoles = GetRolesByClass((ObjectClass)parentID);
            var resultRoles = new Dictionary<int, string>();
            foreach (var role in objectRoles)
            {
                if (userRoles.ContainsKey(role.Key))
                {
                    resultRoles.Add(role.Key, role.Value);
                }
            }
            return resultRoles;
        }

        private async Task<DefaultNotificationTreeItem[]> GetUserNotificationsByClassAsync(int parentID, ObjectClass classID, Guid userID, NotificationTreeLevel level, CancellationToken cancellationToken = default)
        {
            var result = new List<DefaultNotificationTreeItem>();
            //получаем оповещения для класса
            var notifications = await _repository.ToArrayAsync(p => p.ClassID == classID, cancellationToken);
            var defaultNotifications = _mapper.Map<DefaultNotificationData[]>(notifications);
            foreach (var notification in defaultNotifications)
            {
                if ((parentID & notification.AvailableBusinessRole) == parentID) //если у оповещения в доступных ролях такая роль есть
                {
                    var defaultNotificationTreeItem = new DefaultNotificationTreeItem() { Level = level, NotificationID = notification.ID, Name = notification.Name, ParentId = parentID };
                    //проверяем есть ли у пользователя оповещения такая роль

                    var notificationUser = await _notificationUserRepository.FirstOrDefaultAsync(p => p.UserID == userID && p.NotificationID == notification.ID, cancellationToken);
                    if ((parentID & notificationUser?.BusinessRole) == parentID)
                    {
                        defaultNotificationTreeItem.Selected = true;
                    }
                    result.Add(defaultNotificationTreeItem);
                }
            }
            return result.ToArray();
        }
        private void CheckSelected(List<DefaultNotificationTreeItem> allClassNotifications, DefaultNotificationTreeItem defaultNotificationTreeItem)
        {
            if (allClassNotifications.FirstOrDefault(p => p.Selected) != null && allClassNotifications.FirstOrDefault(p => !p.Selected) != null)
            {
                defaultNotificationTreeItem.PartSelected = true;
                defaultNotificationTreeItem.Selected = false;
            }
            else
            {
                defaultNotificationTreeItem.PartSelected = false;
                if (allClassNotifications.FirstOrDefault(p => !p.Selected) == null && allClassNotifications.Count != 0)
                {
                    defaultNotificationTreeItem.Selected = true;
                }
            }
        }
        public async Task<DefaultNotificationTreeItem[]> GetUserNotificationTreeAsync(Guid userID, NotificationTreeLevel level, int parentID, ObjectClass? classID, CancellationToken cancellationToken = default)
        {
            var defaultNotificationTreeItemList = new List<DefaultNotificationTreeItem>();
            var userRoles = await GetBusinessRoleForNotificationsAsync(userID, cancellationToken);
            switch (level)
            {
                case NotificationTreeLevel.Object:
                    {
                        var classes = await GetClassesAsync(cancellationToken);
                        foreach (var item in classes)
                        {
                            var resultRoles = GetResultRoles((int)item.ID, userRoles);
                            if (resultRoles.Any())
                            {
                                var defaultNotificationTreeItem = _mapper.Map<DefaultNotificationTreeItem>((level, parentID, true, true, (int)item.ID, item.Name));

                                var allClassNotifications = new List<DefaultNotificationTreeItem>();
                                foreach (var role in resultRoles)
                                {
                                    var notifications = await GetUserNotificationsByClassAsync(role.Key, item.ID, userID, level, cancellationToken);
                                    allClassNotifications.AddRange(notifications);
                                }
                                if (allClassNotifications.Count > 0)
                                {
                                    CheckSelected(allClassNotifications, defaultNotificationTreeItem);
                                    defaultNotificationTreeItemList.Add(defaultNotificationTreeItem);
                                }
                            }
                        }
                        break;
                    }
                case NotificationTreeLevel.Role:
                    {
                        var resultRoles = GetResultRoles(parentID, userRoles);
                        foreach (var role in resultRoles)
                        {
                            var defaultNotificationTreeItem = _mapper.Map<DefaultNotificationTreeItem>((level, parentID, true, true, role.Key, role.Value));
                            var notifications = await GetUserNotificationsByClassAsync(role.Key, (ObjectClass)parentID, userID, level, cancellationToken);
                            if (notifications != null)
                            {
                                defaultNotificationTreeItem.HasChild = notifications.Any();
                                CheckSelected(notifications.ToList(), defaultNotificationTreeItem);
                                defaultNotificationTreeItemList.Add(defaultNotificationTreeItem);
                            }
                        }
                        break;
                    }
                case NotificationTreeLevel.Notification:
                    {
                        return await GetUserNotificationsByClassAsync(parentID, classID.Value, userID, level, cancellationToken);
                    }
            }
            return defaultNotificationTreeItemList.ToArray();
        }

        public async Task SaveNotificationUserAsync(Guid userId, List<NotificationSaveData> notificationSaveDataList, CancellationToken cancellationToken = default)
        {
            foreach (var notificationSaveDataGroup in notificationSaveDataList.GroupBy(p => p.ID))
            {
                var businessRole = 0;
                var notificationUser = _notificationUserRepository.FirstOrDefault(p => p.UserID == userId && p.NotificationID == notificationSaveDataGroup.Key);
                if (notificationUser == null)
                {
                    foreach (var notificationSaveData in notificationSaveDataGroup)
                    {
                        if (notificationSaveData.Checked)
                        {
                            businessRole |= notificationSaveData.Role;
                        }
                    }
                    notificationUser = new NotifictionUser() { NotificationID = notificationSaveDataGroup.Key, UserID = userId, BusinessRole = businessRole };
                    _notificationUserRepository.Insert(notificationUser);
                }
                else
                {
                    businessRole = notificationUser.BusinessRole;
                    foreach (var notificationSaveData in notificationSaveDataGroup)
                    {
                        if (Enum.IsDefined(typeof(BusinessRole), notificationSaveData.Role))
                        {
                            if (notificationSaveData.Checked)
                            {
                                businessRole |= notificationSaveData.Role;
                            }
                            else
                            {
                                if (businessRole != roleIsNull)
                                {
                                    businessRole ^= notificationSaveData.Role;
                                }
                            }
                        }
                    }
                    notificationUser.BusinessRole = businessRole;
                }
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        

        public async Task BulkSaveUserNotificationsAsync(Guid userID, ObjectClass classID, int? roleID, bool isChecked, CancellationToken cancellationToken = default)
        {
            var saveDataList = new List<NotificationSaveData>();
            if(!roleID.HasValue)
            {
                var userRoles = await GetBusinessRoleForNotificationsAsync(userID, cancellationToken);
                var resultRoles = GetResultRoles((int)classID, userRoles);
                foreach (var role in resultRoles)
                {
                    await NotificationsToSaveAsync(role.Key, classID, userID, NotificationTreeLevel.Object, saveDataList, isChecked, cancellationToken);
                }
            }
            else
            {
                await NotificationsToSaveAsync(roleID.Value, classID, userID, NotificationTreeLevel.Role, saveDataList, isChecked, cancellationToken);
            }
            await SaveNotificationUserAsync(userID, saveDataList, cancellationToken);
        }

        private async Task<List<NotificationSaveData>> NotificationsToSaveAsync(int roleID, ObjectClass classID, Guid userID, NotificationTreeLevel level, 
            List<NotificationSaveData> saveDataList, bool isChecked, CancellationToken cancellationToken)
        {
            var notifications = await GetUserNotificationsByClassAsync(roleID, classID, userID, level, cancellationToken);
            foreach (var notification in notifications)
            {
                var notificationSaveData = new NotificationSaveData() { Checked = isChecked, ID = notification.NotificationID, Role = roleID };
                saveDataList.Add(notificationSaveData);
            }
            return saveDataList;
        }

        public string GetNameByID(Guid notificationID)
            => _repository.FirstOrDefault(x=>x.ID == notificationID)?.Name;

       
        #endregion
    }
}
