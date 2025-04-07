using InfraManager.BLL.AccessManagement;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk
{
    internal class UserIsMassIncidentSupervisor : UserIsSupervisor<MassIncident>,
        ISelfRegisteredService<UserIsSupervisor<MassIncident>>
    {
        public UserIsMassIncidentSupervisor(ISupervisorQuery<MassIncident> query, IUserAccessBLL access)
            : base(OperationID.MassIncident_Supervisor, query, access)
        {
        }
    }
}
