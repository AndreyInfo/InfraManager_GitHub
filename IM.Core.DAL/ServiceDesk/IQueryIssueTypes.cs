using System.Linq;

namespace InfraManager.DAL.ServiceDesk
{
    internal interface IQueryIssueTypes
    {
        IQueryable<IssueType> Query();
    }
}
