using InfraManager.BLL.ServiceDesk.Calls;
using Inframanager.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraManager.DAL.OrganizationStructure;
using System.Threading;
using AutoMapper;
using InfraManager.BLL.Notification;

namespace InfraManager.BLL.OrganizationStructure
{
    internal class DeputyUserModifier : IModifyObject<DeputyUser, DeputyUserData>,
        ISelfRegisteredService<IModifyObject<DeputyUser, DeputyUserData>>
    {
        private readonly IMapper _mapper;
        private readonly INotificationSenderBLL _notificationSenderBLL;

        public DeputyUserModifier(IMapper mapper, INotificationSenderBLL notificationSenderBLL)
        {
            _mapper = mapper;
            _notificationSenderBLL = notificationSenderBLL;
        }

        public async Task ModifyAsync(DeputyUser entity, DeputyUserData data, CancellationToken cancellationToken = default)
        {
            if (entity.ChildUserId != data.ChildUserID)
                await _notificationSenderBLL.SendSeparateNotificationsAsync(SystemSettings.DeleteSubstitution,
                    new InframanagerObject(entity.IMObjID, ObjectClass.Substitution), cancellationToken);
            
            _mapper.Map(data, entity);

        }

        public void SetModifiedDate(DeputyUser entity)
        {
        }
    }
}
