using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class ProblemSupervisorQuery : ISupervisorQuery<Problem>, ISelfRegisteredService<ISupervisorQuery<Problem>>
    {
        private readonly DbSet<Problem> _problems;

        public ProblemSupervisorQuery(DbSet<Problem> problems, DbSet<User> users)
        {
            _problems = problems;
        }

        public IQueryable<Problem> Query(User user)
        {
            var subdivisionID = user.SubdivisionID;
            return _problems.Where(
                problem => Subdivision.SubdivisionIsSibling(subdivisionID, problem.Initiator.SubdivisionID)
                    || Subdivision.SubdivisionIsSibling(subdivisionID, problem.Owner.SubdivisionID)
                    || Subdivision.SubdivisionIsSibling(subdivisionID, problem.Executor.SubdivisionID));
        }
    }
}
