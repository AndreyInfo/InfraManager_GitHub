using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class StandardMassIncidentListFilterExpressions :
        StandardPredicatesDictionary<MassIncident>,
        ISelfRegisteredService<IStandardPredicatesProvider<MassIncident>>
    {
        #region Standard Filters

        private static readonly Dictionary<string, Func<Guid, Expression<Func<MassIncident, bool>>>> _standardFilters =
            new Dictionary<string, Func<Guid, Expression<Func<MassIncident, bool>>>>
            {
                {
                    //!Все мои, непринятые в работу
                    StandardFilters.MassIncidentMyNotStarted,
                    userID =>
                        massIncident => (
                            (massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID)
                                && massIncident.ExecutedByUserID == User.NullUserId
                                && !massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == massIncident.OwnedBy.IMObjID))
                            || (massIncident.OwnedByUserID == User.NullUserId 
                                && massIncident.ExecutedByGroupID == Group.NullGroupID))
                        && massIncident.UtcDateAccomplished == null
                        && massIncident.UtcDateClosed == null
                },
                {
                    //!Мои открытые, просроченные
                    StandardFilters.MassIncidentMyOpenedOverdue,
                    userID =>
                        massIncident =>
                            (massIncident.OwnedBy.IMObjID == userID || massIncident.ExecutedByUser.IMObjID == userID || massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID))
                            && massIncident.UtcOpenedAt != null
                            && massIncident.UtcDateAccomplished == null
                            && massIncident.UtcDateClosed == null
                            && DateTime.UtcNow > massIncident.UtcCloseUntil
                },
                {
                    //!Неназначенные
                     StandardFilters.MassIncidentUnassigned,
                     userID => massIncident => massIncident.OwnedByUserID == User.NullUserId
                },
                {
                    //!Открытые моих коллег, просроченные
                    StandardFilters.MassIncidentOthersOverdue,
                    userID =>
                    massIncident =>
                        !(massIncident.OwnedBy.IMObjID == userID
                            || massIncident.ExecutedByUser.IMObjID == userID
                            || massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID))
                        && DateTime.UtcNow > massIncident.UtcCloseUntil
                        && massIncident.UtcDateAccomplished == null 
                        && massIncident.UtcOpenedAt != null 
                        && massIncident.UtcDateClosed == null
                },
                {
                    //Все мои
                    StandardFilters.MassIncidentMy,
                    userID =>
                        massIncident => massIncident.OwnedBy.IMObjID == userID
                            || massIncident.ExecutedByUser.IMObjID == userID
                            || massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID)
                },
                {
                    //Все мои, в работе
                    StandardFilters.MassIncidentMyInWork,
                    userID =>
                        massIncident => (massIncident.OwnedBy.IMObjID == userID
                                || massIncident.ExecutedByUser.IMObjID == userID
                                || massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID))
                            && massIncident.UtcOpenedAt != null
                            && massIncident.UtcDateAccomplished == null
                            && massIncident.UtcDateClosed == null
                },
                {
                    //Все мои, выполнены и ожидают подтверждения
                    StandardFilters.MassIncidentMyCompletedWaitingForConfirmation,
                    userID =>
                        massIncident => (massIncident.OwnedBy.IMObjID == userID
                                || massIncident.ExecutedByUser.IMObjID == userID
                                || massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID))
                            && massIncident.UtcDateAccomplished !=null
                            && massIncident.UtcDateClosed == null
                },
                {
                    //Все мои, закрыты
                    StandardFilters.MassIncidentMyClosed,
                    userID =>
                        massIncident => (massIncident.OwnedBy.IMObjID == userID
                                || massIncident.ExecutedByUser.IMObjID == userID
                                || massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID))
                            && massIncident.UtcDateClosed != null
                            && massIncident.UtcDateAccomplished != null
                },
                {
                    //Все моих коллег
                    StandardFilters.MassIncidentNotMy,
                    userID =>
                        massIncident =>
                            massIncident.OwnedBy.IMObjID != userID
                            && massIncident.ExecutedByUser.IMObjID != userID
                            && !massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID)
                },
                {
                    //Все моих коллег, в работе
                    StandardFilters.MassIncidentNotMyInWork,
                    userID =>
                        massIncident => massIncident.UtcOpenedAt != null
                            && massIncident.UtcDateAccomplished == null
                            && massIncident.UtcDateClosed == null
                            && massIncident.OwnedBy.IMObjID != userID
                            && massIncident.ExecutedByUser.IMObjID != userID
                            && !massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID)
                },
                {
                    //Все моих коллег, выполнены и ожидают подтверждения
                    StandardFilters.MassIncidentNotMyWaitingForConfirmation,
                    userID =>
                        massIncident => massIncident.UtcDateAccomplished !=null
                            && massIncident.UtcDateClosed == null
                            && massIncident.OwnedBy.IMObjID != userID
                            && massIncident.ExecutedByUser.IMObjID != userID
                            && !massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID)
                },
                {
                    //Все моих коллег, закрытые
                    StandardFilters.MassIncidentNotMyClosed,
                    userID =>
                        massIncident => massIncident.UtcDateAccomplished !=null
                            && massIncident.UtcDateClosed != null
                            && massIncident.OwnedBy.IMObjID != userID
                            && massIncident.ExecutedByUser.IMObjID != userID
                            && !massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID)
                },
                {
                    //Открытые, где я владелец
                    StandardFilters.MassIncidentOwnedOpened,
                    userID =>
                        massIncident => massIncident.OwnedBy.IMObjID == userID
                            && massIncident.UtcDateAccomplished == null
                            && massIncident.UtcOpenedAt != null
                },
                {
                    //Открытые, где я исполнитель
                    StandardFilters.MassIncidentExecutedOpened,
                    userID =>
                        massIncident => (massIncident.ExecutedByUser.IMObjID == userID 
                                || massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == userID))
                            && massIncident.UtcDateAccomplished == null
                            && massIncident.UtcOpenedAt != null
                }
            };

        public StandardMassIncidentListFilterExpressions()
        {
            foreach (var keyValue in _standardFilters) //TODO: упростить этот код
            {
                Add(keyValue.Key, keyValue.Value);
            }
        }

        #endregion
    }
}

