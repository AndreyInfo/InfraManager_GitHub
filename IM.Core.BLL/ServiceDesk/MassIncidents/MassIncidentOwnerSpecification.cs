using Inframanager;
using InfraManager.BLL.AccessManagement;
using InfraManager.DAL;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentOwnerSpecification : Specification<User>,
        ISelfRegisteredService<MassIncidentOwnerSpecification>
    {
        private IUserAccessBLL _userAccessBll;

        private static OperationID[] MassIncidentOwnerOperations = 
            new[]
            {
                OperationID.MassIncident_BeOwner,
                OperationID.SD_General_Administrator
            };

        public MassIncidentOwnerSpecification(IUserAccessBLL userAccessBll)
            : base(
                  user => user.UserRoles
                    .SelectMany(r => r.Role.Operations)
                    .Any(x => MassIncidentOwnerOperations.Contains(x.OperationID)))
        {
            _userAccessBll = userAccessBll;
        }

        // TODO: Добавить проверку на наличие загруженных операций у пользователя, если загружены, то base.GetFunc
        // иначе оставляем оптимизированную реализацию
        protected override Func<User, bool> GetFunc()
        {
            return u => User.SystemUserIds.Contains(u.ID) || MassIncidentOwnerOperations
                .Any(operationID => _userAccessBll.UserHasOperation(u.IMObjID, operationID)); // вместо базовой реализации используем оптимизированный аналог
        }
    }
}
