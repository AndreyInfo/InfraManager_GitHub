using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class StandardChangeRequestListFilterExpressions :
        StandardPredicatesDictionary<ChangeRequest>,
        ISelfRegisteredService<IStandardPredicatesProvider<ChangeRequest>>
    {
        public StandardChangeRequestListFilterExpressions()
        {
            #region MyTasks
            //!Неназначенные
            Add(
            StandardFilters.MyTaskMyWopNoOwner,
            userId =>
            cr =>
                cr.OwnerID == null && cr.UtcDateClosed == null && cr.UtcDateSolved == null);
            //!Мои открытые, просроченные
            Add(
            StandardFilters.MyTaskMyWopMyOpenOverdue,
            userId =>
            cr => cr.OwnerID == userId && cr.UtcDateClosed == null && DateTime.UtcNow > cr.UtcDatePromised);
            //!Открытые моих коллег, просроченные
            Add(
            StandardFilters.MyTaskMyWopCollegOpenOverdue,
            userId =>
            cr => (cr.OwnerID != userId || cr.OwnerID == null) && cr.UtcDateClosed == null && DateTime.UtcNow > cr.UtcDatePromised && cr.UtcDateClosed == null);
            //Все мои
            Add(
            StandardFilters.MyTaskMyWopAllmy,
            userId =>
            cr => cr.OwnerID == userId);
            //Все мои, закрыты
            Add(
            StandardFilters.MyTaskMyWopAllmyClose,
            userId =>
            cr => cr.OwnerID == userId && cr.UtcDateClosed != null);
            //Все мои, не закрыты
            Add(
            StandardFilters.MyTaskMyWopAllmyNotClose,
            userId =>
            cr => cr.OwnerID == userId && cr.UtcDateClosed == null);
            //Все моих коллег
            Add(
            StandardFilters.MyTaskMyWopAllColleg,
            userId =>
            cr => cr.OwnerID != userId || cr.OwnerID == null);
            //Все моих коллег, в работе
            Add(
            StandardFilters.MyTaskMyWopAllCollegInWork,
            userId =>
            cr => (cr.OwnerID != userId || cr.OwnerID == null)
               && cr.UtcDateClosed == null && (cr.UtcDateClosed == null || (cr.EntityStateID == null && cr.WorkflowSchemeID == null && cr.WorkflowSchemeVersion != null)));
            //Все моих коллег, закрытые
            Add(
            StandardFilters.MyTaskMyWopAllCollegClose,
            userId =>
            cr => (cr.OwnerID != userId || cr.OwnerID == null) && cr.UtcDateClosed != null);
            #endregion
        }
    }
}
