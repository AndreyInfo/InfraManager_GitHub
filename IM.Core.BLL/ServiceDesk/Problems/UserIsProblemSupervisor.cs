using InfraManager.BLL.AccessManagement;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk
{
    internal class UserIsProblemSupervisor : UserIsSupervisor<Problem>,
        ISelfRegisteredService<UserIsSupervisor<Problem>>
    {
        public UserIsProblemSupervisor(ISupervisorQuery<Problem> query, IUserAccessBLL access)
            : base(OperationID.Problem_ShowProblemsForITSubdivisionInWeb, query, access)
        {
        }
    }
}
