using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class StandardWorkListFilterExpressions : StandardPredicatesDictionary<WorkOrder>, ISelfRegisteredService<IStandardPredicatesProvider<WorkOrder>>
    {
        public StandardWorkListFilterExpressions()
        {
            #region WorkOrder
            Add( //!На исполнении, мои, не приняты в работу
         StandardFilters.WordOrderMyNotAcceptNotAccomplish,
         userId =>
         w => w.ExecutorID == userId || w.Group.QueueUsers.Any(q => q.GroupID == w.QueueID && q.UserID == userId)
         && w.UtcDateAccepted == null && w.UtcDateAccomplished == null);

            Add(
                //!На исполнении, мои, просроченные
                StandardFilters.WordOrderMyNotAccomplishOverdue,
                    userId =>
                w => w.ExecutorID == userId || w.Group.QueueUsers.Any(q => q.GroupID == w.QueueID && q.UserID == userId)
                && w.UtcDateAccepted != null && w.UtcDateAccomplished == null && w.UtcDatePromised < DateTime.UtcNow);

            Add(
                 //!На исполнении, моих коллег, просроченные
                 StandardFilters.WordOrderCollegNotAccomplishOverdue,
                    userId =>
                w => (w.ExecutorID != userId || w.ExecutorID == null) && !w.Group.QueueUsers.Any(q => q.GroupID == w.QueueID && q.UserID == userId)
                && w.UtcDateAccomplished == null && w.UtcDatePromised < DateTime.UtcNow);

            Add(
                //!Неназначенные
                StandardFilters.WordOrderNoOwner,
                    userId =>
                w => (w.ExecutorID == null || w.AssigneeID == null) && (w.UtcDateAccomplished == null ||
                !(w.EntityStateID == null && w.WorkflowSchemeID == null && w.WorkflowSchemeVersion != null)));

            Add(
                //Все инициированные мною, в работе
                StandardFilters.WordOrderMyInitInWork,
                    userId =>
                w => w.InitiatorID == userId && w.UtcDateAccepted != null && w.UtcDateAccomplished == null);

            Add(
                 //Все инициированные мною, ожидают подтверждения
                 StandardFilters.WordOrderMyInitWait,
                    userId =>
                w => w.InitiatorID == userId && w.UtcDateAccomplished != null && w.WorkflowSchemeID != null);

            Add(
                //Все мои
                StandardFilters.WorkOrderAllmy,
                    userId =>
                w => w.ExecutorID == userId || w.Group.QueueUsers.Any(q => q.GroupID == w.QueueID && q.UserID == userId)
                || w.InitiatorID == userId || w.AssigneeID == userId);

            Add(
                //Все мои на исполнении, в работе
                StandardFilters.WorkOrderAllmyInWorkNotAccomplish,
                    userId =>
                w => w.ExecutorID == userId || w.Group.QueueUsers.Any(q => q.GroupID == w.QueueID && q.UserID == userId)
                && w.UtcDateAccepted != null && w.UtcDateAccomplished == null);

            Add(
                //Все мои на исполнении, ожидают подтверждения
                StandardFilters.WorkOrderAllmyWaitNotAccomplish,
                    userId =>
                w => w.ExecutorID == userId || w.Group.QueueUsers.Any(q => q.GroupID == w.QueueID && q.UserID == userId)
                && w.UtcDateAccomplished != null && w.WorkflowSchemeID != null);

            Add(
                //Все мои, закрытые
                StandardFilters.WorkOrderAllmyClosed,
                    userId =>
                w => (w.ExecutorID == userId || w.Group.QueueUsers.Any(q => q.GroupID == w.QueueID && q.UserID == userId)
                || w.InitiatorID == userId || w.AssigneeID == userId) && w.UtcDateAccomplished != null && w.UtcDateAccepted != null);

            Add(
                 //Все моих коллег, на исполнении
                 StandardFilters.WorkOrderCollegNotAccomplish,
                     userId =>
                w => (w.ExecutorID != userId || w.ExecutorID == null) && !w.Group.QueueUsers.Any(q => q.GroupID == w.QueueID && q.UserID == userId)
                && w.UtcDateAccepted != null && w.UtcDateAccomplished == null);

            Add(
                //Все моих коллег, ожидают подтверждения
                StandardFilters.WorkOrderCollegWait,
                    userId =>
                w => (w.ExecutorID != userId || w.ExecutorID == null) && !w.Group.QueueUsers.Any(q => q.GroupID == w.QueueID && q.UserID == userId)
                && w.UtcDateAccomplished != null && !(w.EntityStateID == null && w.WorkflowSchemeID == null && w.WorkflowSchemeVersion != null));

            Add(
                //Все назначенные мною, в работе
                StandardFilters.WorkOrderAssignByMeInWork,
                    userId =>
                w => w.AssigneeID == userId && w.UtcDateAccepted != null && w.UtcDateAccomplished == null);

            Add(
                //Все назначенные мною, ожидают подтверждения
                StandardFilters.WorkOrderAssignByMeWait,
                    userId =>
                w => w.AssigneeID == userId && w.UtcDateAccomplished != null && w.WorkflowSchemeID != null);
            #endregion

            #region MyTasks
            //!Неназначенные
            Add(
            StandardFilters.MyTaskMyWopNoOwner,
            userId =>
            w => 
                w.ExecutorID == null && w.UtcDateAccomplished == null);
            //!Мои открытые, просроченные
            Add(
            StandardFilters.MyTaskMyWopMyOpenOverdue,
            userId =>
            w =>
               (w.ExecutorID == userId || w.Group.QueueUsers.Any(x => x.UserID == userId))
               && w.UtcDateAccomplished == null
               && DateTime.UtcNow > w.UtcDatePromised);
            //!Открытые моих коллег, просроченные
            Add(
            StandardFilters.MyTaskMyWopCollegOpenOverdue,
            userId =>
            w =>
               (w.ExecutorID != userId || w.ExecutorID == null)
               && !w.Group.QueueUsers.Any(x => x.UserID == userId)
               && DateTime.UtcNow > w.UtcDatePromised
               && w.UtcDateAccomplished == null);
            //Все мои
            Add(
            StandardFilters.MyTaskMyWopAllmy,
            userId =>
            w => w.ExecutorID == userId || w.Group.QueueUsers.Any(x => x.UserID == userId));
            //Все мои, закрыты
            Add(
            StandardFilters.MyTaskMyWopAllmyClose,
            userId =>
            w => w.ExecutorID == userId || w.Group.QueueUsers.Any(x => x.UserID == userId) && w.UtcDateAccomplished != null);
            //Все мои, не закрыты
            Add(
            StandardFilters.MyTaskMyWopAllmyNotClose,
            userId =>
            w => w.ExecutorID == userId || w.Group.QueueUsers.Any(x => x.UserID == userId) && w.UtcDateAccomplished == null);
            //Все моих коллег
            Add(
            StandardFilters.MyTaskMyWopAllColleg,
            userId =>
            w =>
               (w.ExecutorID != userId || w.ExecutorID == null)
               && !w.Group.QueueUsers.Any(x => x.UserID == userId)
            );
            //Все моих коллег, в работе
            Add(
            StandardFilters.MyTaskMyWopAllCollegInWork,
            userId =>
            w =>
               (w.ExecutorID != userId || w.ExecutorID == null)
               && !w.Group.QueueUsers.Any(x => x.UserID == userId)
               && (w.UtcDateAccomplished == null
                   || (w.EntityStateID == null && w.WorkflowSchemeID == null && w.WorkflowSchemeVersion != null)));
            //Все моих коллег, закрытые
            Add(
            StandardFilters.MyTaskMyWopAllCollegClose,
            userId =>
            w =>
               (w.ExecutorID != userId || w.ExecutorID == null)
               && !w.Group.QueueUsers.Any(x => x.UserID == userId)
               && w.UtcDateAccomplished != null);
            #endregion
        }
    }
}
