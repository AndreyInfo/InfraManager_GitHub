using Inframanager;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems.Events
{
    internal class AllProblemsReportUserSpecificationBuilder : ReportUserSpecificationBuilder<Problem>,
        ISelfRegisteredService<AllProblemsReportUserSpecificationBuilder>
    {
        public AllProblemsReportUserSpecificationBuilder(
            IFindEntityByGlobalIdentifier<User> userFinder,
            ServiceDeskObjectAccessIsNotRestricted accessNotRestricted,
            UserIsSupervisor<Problem> userIsSupervisor, // Видеть проблемы ИТ-сотрудников 
            NotOwnedProblem notOwnedProblems)
            : base(           
                  SpecificationBuilder<Problem, User>.Any(
                    Problem.UserIsOwner,
                    Problem.UserIsInitiator,
                    Problem.UserIsExecutor,
                    Problem.UserIsInGroup,
                    notOwnedProblems,
                    userIsSupervisor),
                  userFinder,
                  accessNotRestricted)
        {
        }
    }
}
