using Inframanager;
using InfraManager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk
{
    internal class UserIsSupervisor<T> : IBuildSpecification<T, User>
        where T : IGloballyIdentifiedEntity
    {
        private readonly OperationID _supervisorOperationID;
        private readonly ISupervisorQuery<T> _query; //TODO: Перейти от Query к композитной спецификации
        private readonly IUserAccessBLL _access;

        public UserIsSupervisor(
            OperationID supervisorOperationID, 
            ISupervisorQuery<T> query, 
            IUserAccessBLL access)
        {
            _query = query;
            _access = access;
            _supervisorOperationID = supervisorOperationID;
        }

        public Specification<T> Build(User filterBy)
        {
            if (!_access.UserHasOperation(filterBy.IMObjID, _supervisorOperationID))
            {
                return new Specification<T>(x => false);
            }

            var availableObjects = _query.Query(filterBy);
            return new Specification<T>(
                obj => availableObjects.Any(x => x.IMObjID == obj.IMObjID));
        }
    }
}
