using InfraManager.BLL.Notification;
using InfraManager.BLL.Settings;
using InfraManager.DAL.Events;
using InfraManager.Services;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.UI.Web.Controllers
{
    /// <summary>
    /// Оповещения
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : BaseApiController
    {
        private readonly INotificationBLL _notificationBLL;

        public NotificationController(
            INotificationBLL notificationBLL)
        {
            _notificationBLL = notificationBLL;
        }


        [HttpGet]
        public async Task<NotificationData[]> GetNotificationsAsync([FromQuery] BaseFilter filter,
            CancellationToken cancellationToken = default)
        {
            return await _notificationBLL.GetNotificationsAsync(filter, cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<NotificationDetails> GetNotificationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _notificationBLL.FindAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<Guid> AddNotificationAsync([FromBody] NotificationDetails model, CancellationToken cancellationToken = default)
        {
            return await _notificationBLL.AddNotificationAsync(model, cancellationToken);
        }

        [HttpPut]
        public async Task<Guid> UpdateNotificationAsync([FromBody] NotificationDetails model, CancellationToken cancellationToken = default)
        {
            return await _notificationBLL.UpdateNotificationAsync(model, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteNotificationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _notificationBLL.DeleteNotificationAsync(id, cancellationToken);
        }

        [HttpGet("classes")]
        public async Task<InframanagerObjectClassData[]> GetClassesAsync(CancellationToken cancellationToken = default)
        {
            return await _notificationBLL.GetClassesAsync(cancellationToken);
        }

        [HttpGet("rolesByClass")]
        public Dictionary<int, string> GetRolesByClass(ObjectClass id)
        {
            return _notificationBLL.GetRolesByClass(id);
        }
        

        [HttpGet("parameters")]
        public ParameterTemplate[] GetAvailableParameterList(ObjectClass objectClass)
        {
            return _notificationBLL.GetAvailableParameterList(objectClass);
        }

        [HttpGet("defaultNotifications")]
        public async Task<DefaultNotificationTreeItem[]> DefaultNotificationsAsync(NotificationTreeLevel level, int parentId, ObjectClass? classId, CancellationToken cancellationToken = default)
        {
            return await _notificationBLL.GetDefaultNotificationTreeAsync(level, parentId, classId, cancellationToken);
        }

        [HttpPut("defaultNotifications")]
        public async Task SaveDefaultNotificationsAsync(List<NotificationSaveData> notificationSaveDataList, CancellationToken cancellationToken = default)
        {
            await _notificationBLL.SaveDefaultNotificationsAsync(notificationSaveDataList, cancellationToken);
        }

        [HttpGet("userNotifications")]
        public async Task<DefaultNotificationTreeItem[]> GetUserNotificationTreeAsync(Guid userId, NotificationTreeLevel level, int parentId, ObjectClass? classId, CancellationToken cancellationToken = default)
        {
            return await _notificationBLL.GetUserNotificationTreeAsync(userId, level, parentId, classId, cancellationToken);
        }

        [HttpPut("userNotifications")]
        public async Task SaveNotificationUserAsync(Guid userId, List<NotificationSaveData> notificationSaveDataList, CancellationToken cancellationToken = default)
        {
            await _notificationBLL.SaveNotificationUserAsync(userId, notificationSaveDataList, cancellationToken);
        }

        [HttpPut("userNotifications/bulk")]
        public async Task BulkSaveUserNotificationsAsync(Guid userID, ObjectClass classID, int? roleID, bool isChecked, CancellationToken cancellationToken = default)
        {
            await _notificationBLL.BulkSaveUserNotificationsAsync(userID, classID, roleID, isChecked, cancellationToken);
        }
    }
}
