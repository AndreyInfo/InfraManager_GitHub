using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ProblemPredicateBuilders : 
        FilterBuildersAggregate<Problem, ProblemListItem>, 
        ISelfRegisteredService<IAggregatePredicateBuilders<Problem, ProblemListItem>>
    {
        public ProblemPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(ProblemListItem.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(ProblemListItem.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(ProblemListItem.Number), x => x.Number);
            AddPredicateBuilder(nameof(ProblemListItem.UrgencyName), x => x.Urgency.ID);
            AddPredicateBuilder(nameof(ProblemListItem.InfluenceName), x => x.Influence.ID);
            AddPredicateBuilder(nameof(ProblemListItem.UtcDateDetected), x => x.UtcDateDetected);
            AddPredicateBuilder(nameof(ProblemListItem.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(ProblemListItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(ProblemListItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(ProblemListItem.UtcDateSolved), x => x.UtcDateSolved);
            AddPredicateBuilder(nameof(ProblemListItem.Summary), x => x.Summary);
            AddPredicateBuilder(nameof(ProblemListItem.Description), x => DbFunctions.CastAsString(x.Description));
            AddPredicateBuilder(nameof(ProblemListItem.Cause), x => DbFunctions.CastAsString(x.Cause));
            AddPredicateBuilder(nameof(ProblemListItem.ProblemCauseName), x => x.ProblemCause.Name);
            AddPredicateBuilder(nameof(ProblemListItem.Fix), x => DbFunctions.CastAsString(x.Fix));
            AddPredicateBuilder(nameof(ProblemListItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(ProblemListItem.Solution), x => DbFunctions.CastAsString(x.Solution));
            AddPredicateBuilder(nameof(ProblemListItem.UserField1), x => x.UserField1);
            AddPredicateBuilder(nameof(ProblemListItem.UserField2), x => x.UserField2);
            AddPredicateBuilder(nameof(ProblemListItem.UserField3), x => x.UserField3);
            AddPredicateBuilder(nameof(ProblemListItem.UserField4), x => x.UserField4);
            AddPredicateBuilder(nameof(ProblemListItem.UserField5), x => x.UserField5);
            AddPredicateBuilder(nameof(ProblemListItem.InitiatorFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(ProblemListItem.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(ProblemListItem.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(ProblemListItem.ServiceName), x => x.Service.ID);
        }
    }
}
