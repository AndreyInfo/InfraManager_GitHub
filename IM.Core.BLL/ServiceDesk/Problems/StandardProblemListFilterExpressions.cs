using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class StandardProblemListFilterExpressions :
        StandardPredicatesDictionary<Problem>,
        ISelfRegisteredService<IStandardPredicatesProvider<Problem>>
    {
        public StandardProblemListFilterExpressions()
        {
            #region Problem
            Add( //!Мои открытые просроченные
                StandardFilters.ProblemMyOpenOverdue,
                userId =>
                        p => p.OwnerID == userId
                            && p.UtcDateSolved == null
                        && p.UtcDateClosed == null
                        && DateTime.UtcNow > p.UtcDatePromised);
            Add(//!Неназначенные 
                StandardFilters.ProblemNoOwner,
                userId => p => p.OwnerID == null);
            Add(//!Просроченные моих коллег
                StandardFilters.ProblemColleagueOverdue,
                userId =>
                    p => p.OwnerID != userId
                        && p.UtcDateSolved == null
                        && p.UtcDateClosed == null
                        && DateTime.UtcNow > p.UtcDatePromised);
            Add(//Все мои 
                StandardFilters.ProblemAllMy,
                userId => p => p.OwnerID == userId);
            Add(//Все мои, закрытые
                StandardFilters.ProblemAllMyClosed,
                userId =>
                  p => p.OwnerID == userId
                    && p.UtcDateSolved != null
                    && p.UtcDateClosed != null);
            Add(//Все мои, открытые
                StandardFilters.ProblemAllMyOpened,
                userId =>
                    p => p.OwnerID == userId
                        && p.UtcDateSolved == null
                        && p.UtcDateClosed == null);
            Add(//Все мои, решенные, ожидают подтверждения
                StandardFilters.ProblemAllMySolvedAndWaiting,
                userId =>
                    p => p.OwnerID == userId
                        && p.UtcDateSolved != null
                        && p.UtcDateClosed == null);
            Add(//Все моих коллег, закрытые
                StandardFilters.ProblemColleagueClosed,
                userId =>
                    p => p.OwnerID != userId
                        && p.UtcDateSolved != null
                        && p.UtcDateClosed != null);
            Add(//Все моих коллег, открытые
                StandardFilters.ProblemColleagueOpened,
                userId =>
                    p => p.OwnerID != userId
                        && p.UtcDateSolved == null
                        && p.UtcDateClosed == null);
            Add(//Все моих коллег, решенные, ожидают подтверждения
                StandardFilters.ProblemColleagueSolvedAndWaiting,
                userId =>
                    p => p.OwnerID != userId
                        && p.UtcDateSolved != null
                        && p.UtcDateClosed == null);
            #endregion

            #region MyTasks
            //!Неназначенные
            Add(
            StandardFilters.MyTaskMyWopNoOwner,
            userId =>
            p => 
                p.OwnerID == null && p.UtcDateClosed == null && p.UtcDateSolved == null);
            //!Мои открытые, просроченные
            Add(
            StandardFilters.MyTaskMyWopMyOpenOverdue,
            userId =>
            p => p.OwnerID == userId && p.UtcDateClosed == null && DateTime.UtcNow > p.UtcDatePromised);
            //!Открытые моих коллег, просроченные
            Add(
            StandardFilters.MyTaskMyWopCollegOpenOverdue,
            userId =>
            p => (p.OwnerID != userId || p.OwnerID == null) && p.UtcDateClosed == null && DateTime.UtcNow > p.UtcDatePromised && p.UtcDateClosed == null);
            //Все мои
            Add(
            StandardFilters.MyTaskMyWopAllmy,
            userId =>
            p => p.OwnerID == userId);
            //Все мои, закрыты
            Add(
            StandardFilters.MyTaskMyWopAllmyClose,
            userId =>
            p => p.OwnerID == userId && p.UtcDateClosed != null);
            //Все мои, не закрыты
            Add(
            StandardFilters.MyTaskMyWopAllmyNotClose,
            userId =>
            p => p.OwnerID == userId && p.UtcDateClosed == null);
            //Все моих коллег
            Add(
            StandardFilters.MyTaskMyWopAllColleg,
            userId =>
            p => p.OwnerID != userId || p.OwnerID == null);
            //Все моих коллег, в работе
            Add(
            StandardFilters.MyTaskMyWopAllCollegInWork,
            userId =>
            p => (p.OwnerID != userId || p.OwnerID == null)
               && p.UtcDateClosed == null && (p.UtcDateClosed == null || (p.EntityStateID == null && p.WorkflowSchemeID == null && p.WorkflowSchemeVersion != null)));
            //Все моих коллег, закрытые
            Add(
            StandardFilters.MyTaskMyWopAllCollegClose,
            userId =>
            p => (p.OwnerID != userId || p.OwnerID == null) && p.UtcDateClosed != null);
            #endregion
        }
    }
}
