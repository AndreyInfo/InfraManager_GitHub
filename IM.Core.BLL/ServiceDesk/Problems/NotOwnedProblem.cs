using Inframanager;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class NotOwnedProblem : 
        IBuildSpecification<Problem, User>, 
        ISelfRegisteredService<NotOwnedProblem>
    {
        public Specification<Problem> Build(User user)
        {
            return User.HasOperation(OperationID.SD_General_Owner).IsSatisfiedBy(user)
                ? new Specification<Problem>(problem => problem.OwnerID == null)
                : new Specification<Problem>(false);
        }
    }
}
