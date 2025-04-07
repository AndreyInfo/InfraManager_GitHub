using Inframanager;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Collections.Generic;
using InfraManager.DAL.ServiceDesk.Manhours;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System.Linq.Expressions;
using InfraManager.Linq;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    [ObjectClassMapping(ObjectClass.ChangeRequest)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.ChangeRequest_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.ChangeRequest_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.ChangeRequest_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]// не трогать
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]// не трогать
    public class ChangeRequest : 
        IServiceDeskEntity, 
        IGloballyIdentifiedEntity, 
        ICreateNegotiation, 
        IHaveManhours,
        ICreateWorkOrderReference,
        IHaveUtcModifiedDate
    {
        #region .ctor
        protected ChangeRequest()
        {
        }

        public ChangeRequest(Guid changeRequestTypeID)
        {
            IMObjID = Guid.NewGuid();
            RFCTypeID = changeRequestTypeID;
            UtcDateDetected = DateTime.UtcNow;
            UtcDateModified = UtcDateDetected;
        }

        #endregion .ctor
        /// <summary>
        /// Возвращает идентификатор rfc
        /// </summary>

        #region Properties
        public Guid IMObjID { get; private set; }

        /// <summary>
        /// Возвращает номер rfc
        /// </summary>
        public int Number { get; private set; }

        public string ServiceName { get; set; }
        public string Summary { get; set; }
        public DateTime UtcDateDetected { get; set; }
        public DateTime? UtcDatePromised { get; set; }
        public DateTime? UtcDateClosed { get; set; }
        public DateTime? UtcDateSolved { get; set; }
        public DateTime UtcDateModified { get; set; }
        public DateTime? UtcDateStarted { get; set; }
        public string HTMLDescription { get; set; }
        public string Description { get; set; }
        public string Target { get; set; }
        public byte[] RowVersion { get; set; }
        public Guid? ReasonObjectID { get; set; }
        public ObjectClass? ReasonObjectClassID { get; set; }
        public int? FundingAmount { get; set; }
        public virtual IEnumerable<ManhoursWork> Manhours { get; }

        public void OnManhoursWorkAdded()
        {
            UtcDateModified = DateTime.UtcNow;
        }

        public void IncrementTotalManhours(int value)
        {
            ManhoursInMinutes += value;
            UtcDateModified = DateTime.UtcNow;
        }

        public void DecrementTotalManhours(int value)
        {
            ManhoursInMinutes -= value;
            UtcDateModified = DateTime.UtcNow;
        }

        public int ManhoursNormInMinutes { get; set; }
        public string EntityStateID { get; set; }
        public string EntityStateName { get; set; }
        public Guid? WorkflowSchemeID { get; set; }
        public string WorkflowSchemeIdentifier { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        public bool OnWorkOrderExecutorControl { get; set; }
        public bool InRealization { get; set; }
        public Guid? RealizationDocumentID { get; set; }
        public Guid? RollbackDocumentID { get; set; }
        public string TargetEntityStateID { get; set; }

        #endregion

        #region FormValues

        public long? FormValuesID { get; set; }
        public Guid? FormID { get; set; }

        public virtual FormValues FormValues { get; set; }
        #endregion

        #region Urgency

        public Guid? UrgencyID { get; set; }

        /// <summary>
        /// Возвращает срочность
        /// </summary>
        public virtual Urgency Urgency { get; }

        #endregion

        #region Influence

        public Guid? InfluenceID { get; set; }

        /// <summary>
        /// Возвращает или задает влияние
        /// </summary>
        public virtual Influence Influence { get; }

        #endregion

        #region Priority

        public Guid PriorityID { get; set; }

        /// <summary>
        /// Возвращает приоритет
        /// </summary>
        public virtual Priority Priority { get; }

        #endregion

        #region Type

        public Guid RFCTypeID { get; set; }

        /// <summary>
        /// Возвращает тип
        /// </summary>
        public virtual ChangeRequestType Type { get; }

        #endregion

        #region Category

        public Guid? CategoryID { get; set; }

        /// <summary>
        /// Возвращает категорию
        /// </summary>
        public virtual ChangeRequestCategory Category { get; }

        #endregion

        #region Service

        public Guid? ServiceID { get; set; }

        /// <summary>
        /// Возвращает сервис
        /// </summary>
        public virtual Service Service { get; }

        #endregion

        #region Group

        public Guid? QueueID { get; set; }

        /// <summary>
        /// Возвращает группу
        /// </summary>
        public virtual Group Group { get; }

        #endregion

        #region Initiator

        public Guid? InitiatorID { get; set; }

        /// <summary>
        /// Возвращает инициатора
        /// </summary>
        public virtual User Initiator { get; }

        #endregion

        #region Owner

        public Guid? OwnerID { get; set; }

        /// <summary>
        /// Возвращает владельца
        /// </summary>
        public virtual User Owner { get; }

        public int ManhoursInMinutes { get; set; }

        public static Expression<Func<ChangeRequest, string>> OwnerFullName =>
            User.FullNameExpression.Substitute<ChangeRequest, User, string>(rfc => rfc.Owner);

        #endregion

        #region Negotiation

        public Negotiation CreateNegotiation()
        {
            return new ChangeRequestNegotiation(IMObjID);
        }

        #endregion

        public string ReferenceName => $"IM-RL-{Number}";

        public WorkOrderReference CreateWorkOrderReference()
        {
            return new WorkOrderReference<ChangeRequest>(this);
        }
        
        public virtual IEnumerable<WorkOrderReference<ChangeRequest>> WorkOrderReferences { get; }
    }
}