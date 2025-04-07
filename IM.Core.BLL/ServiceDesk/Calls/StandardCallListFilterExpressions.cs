using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class StandardCallListFilterExpressions :
        StandardPredicatesDictionary<Call>,
        ISelfRegisteredService<IStandardPredicatesProvider<Call>>
    {
        #region Standard Filters

        private static readonly Dictionary<string, Func<Guid, Expression<Func<Call, bool>>>> _standardFilters =
            new Dictionary<string, Func<Guid, Expression<Func<Call, bool>>>>
            {
                #region client
                {
                     //Все заявки в работе
                    StandardFilters.CallClientAllMyInWork,
                    userId =>
                        с => (с.ClientID == userId || с.InitiatorID == userId) && с.UtcDateAccomplished == null && с.UtcDateClosed == null

                },

                {
                    //!Заявки в работе, где я клиент
                    StandardFilters.CallClientOnlyMyInWork,
                    userId =>
                        с => с.ClientID == userId && с.UtcDateAccomplished == null && с.UtcDateClosed == null
                },

                {
                    //!Заявки, выполнены и ожидают подтверждения, где я клиент
                    StandardFilters.CallClientOnlyMyWaitClosed,
                    userId =>
                        с => с.ClientID == userId && с.UtcDateAccomplished != null && с.UtcDateClosed == null
                },

                {
                    //!Заявки в работе, где я заявитель
                    StandardFilters.CallClientInWorkIinit,
                    userId =>
                    с => с.InitiatorID == userId && с.UtcDateAccomplished == null && с.UtcDateClosed == null
                },

                {
                    //Закрытые заявки, все
                    StandardFilters.CallClientAllMyClosed,
                    userId =>
                    с => (с.ClientID == userId || с.InitiatorID == userId) && с.UtcDateAccomplished != null && с.UtcDateClosed != null
                },

                {
                    //Заявки в работе, моих коллег
                    StandardFilters.CallClientCollegsInWork,
                    userId =>
                    с => (с.ClientID != userId || с.ClientID == null) && (с.InitiatorID != userId || с.InitiatorID == null) && с.UtcDateAccomplished == null && с.UtcDateClosed == null
                },

                {
                    //Закрытые заявки, моих коллег
                    StandardFilters.CallClientCollegsClosed,
                    userId =>
                    с => (с.ClientID != userId || с.ClientID == null) && (с.InitiatorID != userId || с.InitiatorID == null) && с.UtcDateAccomplished != null && с.UtcDateClosed != null
                },

                {
                    //!Заявки, выполнены и ожидают подтверждения, где я заявитель
                    StandardFilters.CallClientOnlyMyInitedWaitClosed,
                    userId =>
                    с => с.InitiatorID == userId && с.UtcDateAccomplished !=null && с.UtcDateClosed == null
                },
                #endregion

                #region engineer
                {
                    //!Неназначенные 
                    StandardFilters.CallEngineerNoOwner,
                    userId =>
                    c => c.OwnerID == null
                },

                {
                    //Открытые, где я владелец
                    StandardFilters.CallEngineerOpenIOwner,
                    userId =>
                    c => c.OwnerID == userId && c.UtcDateAccomplished == null && c.UtcDateOpened != null

                },
                {
                    //Все мои, в работе
                    StandardFilters.CallEngineerAllMyNotAccomplish,
                    userId =>
                        c => (c.OwnerID == userId
                        || c.ExecutorID == userId
                        || c.Queue.QueueUsers.Any(x => x.UserID == userId)
                        || c.AccomplisherID == userId)
                    && c.UtcDateOpened !=null
                    && c.UtcDateAccomplished == null
                    && c.UtcDateClosed == null
                },
                {

                    //Все моих коллег, в работе
                    StandardFilters.CallEngineerAllCollegNotAccomplish,
                    userId =>
                    c => c.UtcDateOpened != null && c.UtcDateAccomplished == null && c.UtcDateClosed == null &&
                    !(c.OwnerID == userId || c.ExecutorID == userId || (c.QueueID != null  && c.Queue.QueueUsers.Any(x => x.UserID == userId)
                    ) || c.AccomplisherID == userId)
                },
                {
                    //!Все мои, непринятые в работу
                    StandardFilters.CallEngineerMyNotStarted,
                     userId =>
                    c => (c.OwnerID == userId && c.ExecutorID == null && c.QueueID == null
                        || c.ExecutorID == userId 
                        || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userId)
                    )) 
                        && c.UtcDateOpened == null 
                        && c.UtcDateAccomplished == null 
                        && c.UtcDateClosed == null
                },
                {
                    //Все мои, выполнены и ожидают подтверждения
                    StandardFilters.CallEngineerMyWaitClosed,
                    userId =>
                    c => (c.OwnerID == userId || c.ExecutorID == userId || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userId)
                    ) || c.AccomplisherID == userId) && c.UtcDateAccomplished !=null && c.UtcDateClosed == null
                },
                {
                    //Все моих коллег, выполнены и ожидают подтверждения
                     StandardFilters.CallEngineerCollegAccomplishWaitClose,
                     userId =>
                    c => (c.UtcDateClosed == null && c.UtcDateAccomplished != null) &&
                    !(c.OwnerID == userId || c.ExecutorID == userId || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userId)
                    ) || c.AccomplisherID == userId)
                },
                {
                    //Открытые, где я исполнитель
                    StandardFilters.CallEngineerOpenIExecutor,
                    userId =>
                    c => (c.ExecutorID == userId || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userId)
                    )) && c.UtcDateAccomplished == null && c.UtcDateOpened != null
                },
                {         
                    //!Мои открытые, просроченные
                    StandardFilters.CallEngineerMyOpenOverdue,
                    userId =>
                        c => (c.OwnerID == userId
                            || c.ExecutorID == userId
                            || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userId))
                            || c.AccomplisherID == userId)
                        && !c.IsFinished
                        && c.UtcDatePromised < DateTime.UtcNow

                },
                {
                    //!Открытые моих коллег, просроченные 
                    StandardFilters.CallEngineerCollegOpenOverdue,
                    userID =>
                        c => !(
                            c.OwnerID == userID 
                                || c.ExecutorID == userID 
                                || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userID)) 
                                || c.AccomplisherID == userID) 
                        && !c.IsFinished 
                        && c.UtcDatePromised < DateTime.UtcNow
                },

                {
                    //Все мои
                    StandardFilters.CallEngineerAllmy,
                    userId =>
                        c => c.OwnerID == userId
                            || c.ExecutorID == userId
                            || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userId))
                        || c.AccomplisherID == userId
                },
                {
                    //Все мои, закрытые
                    StandardFilters.CallEngineerAllmyClosed,
                    userId =>
                    c => (c.OwnerID == userId || c.ExecutorID == userId || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userId)
                    ) || c.AccomplisherID == userId) && c.UtcDateAccomplished != null && c.UtcDateClosed != null
                 },
                {
                    //Все моих коллег 
                    StandardFilters.CallEngineerAllCollegs,
                    userId =>
                    c => !(c.OwnerID == userId || c.ExecutorID == userId || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userId)
                    ) || c.AccomplisherID == userId)

                },
                {
                    //Все моих коллег, закрытые
                    StandardFilters.CallEngineerAllCollegsClose,
                    userId =>
                    c => !(c.OwnerID == userId || c.ExecutorID == userId || (c.QueueID != null && c.Queue.QueueUsers.Any(x => x.UserID == userId)
                    )|| c.AccomplisherID == userId) && c.UtcDateAccomplished != null && c.UtcDateClosed != null
                },
                #endregion

                #region MyTasks
                {
                    //!Неназначенные
                     StandardFilters.MyTaskMyWopNoOwner,
                     userId =>
                     c =>
                        (c.OwnerID == null
                            || c.ExecutorID == null
                            && c.Queue.QueueUsers.Any(x => x.UserID == userId)
                            || (c.OwnerID == userId && c.QueueID != null && c.ExecutorID == null))
                        && c.UtcDateClosed == null
                        && c.UtcDateAccomplished == null
                },
                {
                    //!Мои открытые, просроченные
                    StandardFilters.MyTaskMyWopMyOpenOverdue,
                    userId =>
                    c =>
                        (c.OwnerID == userId || c.ExecutorID == userId || c.Queue.QueueUsers.Any(x => x.UserID == userId))
                        && c.UtcDateClosed == null
                        && DateTime.UtcNow > c.UtcDatePromised
                },
                {
                    //!Открытые моих коллег, просроченные
                    StandardFilters.MyTaskMyWopCollegOpenOverdue,
                    userId =>
                    c =>
                        (c.OwnerID != userId || c.OwnerID == null)
                        && (c.ExecutorID != userId || c.ExecutorID == null)
                        && !c.Queue.QueueUsers.Any(x => x.UserID == userId)
                        && c.UtcDateClosed == null
                        && DateTime.UtcNow > c.UtcDatePromised
                        && c.UtcDateAccomplished == null
                },
                {
                    //Все мои
                    StandardFilters.MyTaskMyWopAllmy,
                    userId =>
                    c => c.OwnerID == userId || c.ExecutorID == userId || c.Queue.QueueUsers.Any(x => x.UserID == userId)
                },
                {
                    //Все мои, закрыты
                    StandardFilters.MyTaskMyWopAllmyClose,
                    userId =>
                    c => c.OwnerID == userId || c.ExecutorID == userId || c.Queue.QueueUsers.Any(x => x.UserID == userId) && c.UtcDateClosed != null
                },
                {
                    //Все мои, не закрыты
                    StandardFilters.MyTaskMyWopAllmyNotClose,
                    userId =>
                    c => c.OwnerID == userId || c.ExecutorID == userId || c.Queue.QueueUsers.Any(x => x.UserID == userId) && c.UtcDateClosed == null
                },
                {
                    //Все моих коллег
                    StandardFilters.MyTaskMyWopAllColleg,
                    userId =>
                    c =>
                        (c.OwnerID != userId || c.OwnerID == null)
                        && (c.ExecutorID != userId || c.ExecutorID == null)
                        && !c.Queue.QueueUsers.Any(x => x.UserID == userId)
                },
                {
                    //Все моих коллег, в работе
                    StandardFilters.MyTaskMyWopAllCollegInWork,
                    userId =>
                    c =>
                        (c.OwnerID != userId || c.OwnerID == null)
                        && (c.ExecutorID != userId || c.ExecutorID == null)
                        && !c.Queue.QueueUsers.Any(x => x.UserID == userId)
                        && c.UtcDateClosed == null
                        && (c.UtcDateAccomplished == null
                            || (c.EntityStateID == null && c.WorkflowSchemeID == null && c.WorkflowSchemeVersion != null))
                },
                {
                    //Все моих коллег, закрытые
                    StandardFilters.MyTaskMyWopAllCollegClose,
                    userId =>
                    c =>
                        (c.OwnerID != userId || c.OwnerID == null)
                        && (c.ExecutorID != userId || c.ExecutorID == null)
                        && !c.Queue.QueueUsers.Any(x => x.UserID == userId)
                        && c.UtcDateClosed != null
                }
                #endregion
            };

        public StandardCallListFilterExpressions()
        {
            foreach (var keyValue in _standardFilters) //TODO: упростить этот код
            {
                Add(keyValue.Key, keyValue.Value);
            }
        }

        #endregion
    }
}

