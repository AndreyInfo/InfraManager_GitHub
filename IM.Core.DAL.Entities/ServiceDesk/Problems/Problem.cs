using Inframanager;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Collections.Generic;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.Manhours;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System.Linq.Expressions;
using InfraManager.Linq;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk
{
    /// <summary>
    /// Этот класс представляет сущность Проблема
    /// </summary>
    [ObjectClassMapping(ObjectClass.Problem)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Problem_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Problem_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Problem_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]// не трогать
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]// не трогать
    public class Problem :
        IServiceDeskEntity,
        IGloballyIdentifiedEntity,
        ICreateNegotiation,
        IHaveManhours,
        IHaveUserFields,
        ICreateWorkOrderReference,
        IHaveFormValues,
        IHaveUtcModifiedDate,
        IWorkOrderExecutorControl
    {
        #region .ctor

        protected Problem()
        {
        }

        public Problem(Guid typeId)
        {
            IMObjID = Guid.NewGuid();
            TypeID = typeId;
            UtcDateDetected = DateTime.UtcNow;
            UtcDateModified = DateTime.UtcNow;
            UserField1 = string.Empty;
            UserField2 = string.Empty;
            UserField3 = string.Empty;
            UserField4 = string.Empty;
            UserField5 = string.Empty;
            Solution = string.Empty;
            Description = string.Empty;
            Cause = string.Empty;
            Fix = string.Empty;
            HTMLCause = string.Empty;
            HTMLDescription = string.Empty;
            HTMLFix = string.Empty;
            HTMLSolution = string.Empty;
        }

        #endregion

        #region Identification

        /// <summary>
        /// Возвращает идентификатор проблемы
        /// </summary>
        public Guid IMObjID { get; private set; }

        /// <summary>
        /// Возвращает номер проблемы
        /// </summary>
        public int Number { get; private set; }

        #endregion

        #region Properties
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime UtcDateDetected { get; set; }
        public DateTime UtcDatePromised { get; set; }
        public DateTime? UtcDateClosed { get; set; }
        public DateTime? UtcDateSolved { get; set; }
        public DateTime UtcDateModified { get; set; }
        public string Solution { get; set; }
        public string Cause { get; set; }
        public string Fix { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
        public string UserField4 { get; set; }
        public string UserField5 { get; set; }
        public string EntityStateID { get; set; }
        public string EntityStateName { get; set; }
        public Guid? WorkflowSchemeID { get; set; }
        public string WorkflowSchemeIdentifier { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        public string TargetEntityStateID { get; set; }
        public string HTMLSolution { get; set; }
        public string HTMLFix { get; set; }
        public string HTMLDescription { get; set; }
        public string HTMLCause { get; set; }
        public int ManhoursNormInMinutes { get; set; }
        public bool OnWorkOrderExecutorControl { get; set; }
        public byte[] RowVersion { get; set; }

        #endregion

        #region FormValues

        public long? FormValuesID { get; set; }
        public Guid? FormID { get; set; }

        public virtual FormValues FormValues { get; set; }

        #endregion

        #region Urgency

        public Guid? UrgencyId { get; set; }

        /// <summary>
        /// Возвращает срочность
        /// </summary>
        public virtual Urgency Urgency { get; }

        #endregion

        #region Influence

        public Guid? InfluenceId { get; set; }

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

        public Guid TypeID { get; set; }

        /// <summary>
        /// Возвращает тип проблемы
        /// </summary>
        public virtual ProblemType Type { get; }

        #endregion

        #region Problem Cause

        public Guid? ProblemCauseId { get; set; }

        public virtual ProblemCause ProblemCause { get; }

        #endregion

        #region Owner

        public Guid? OwnerID { get; set; }

        public virtual User Owner { get; }

        public static Expression<Func<Problem, string>> OwnerFullName =>
            User.FullNameExpression.Substitute<Problem, User, string>(p => p.Owner);

        public static SpecificationBuilder<Problem, User> UserIsOwner =>
            new SpecificationBuilder<Problem, User>((problem, user) => problem.OwnerID == user.IMObjID);

        #endregion

        #region Dependency objects

        public virtual IEnumerable<ProblemDependency> Dependencies { get; }

        #endregion

        #region Negotiations

        public virtual IEnumerable<ProblemNegotiation> Negotiations { get; }

        #endregion

        #region Call references

        public virtual IEnumerable<CallReference<Problem>> CallReferences { get; }

        #endregion

        #region Notes

        public virtual IEnumerable<Note<Problem>> Notes { get; }

        #endregion

        #region Negotiations

        public Negotiation CreateNegotiation()
        {
            return new ProblemNegotiation(IMObjID);
        }

        #endregion

        #region Manhours

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

        public int ManhoursInMinutes { get; set; }


        #endregion

        #region WorkOrder references

        public string ReferenceName => $"IM-PL-{Number}";

        public WorkOrderReference CreateWorkOrderReference()
        {
            return new WorkOrderReference<Problem>(this);
        }

        public virtual IEnumerable<WorkOrderReference<Problem>> WorkOrderReferences { get; }

        #endregion

        #region Initiator

        public Guid? InitiatorID { get; set; }
        public virtual User Initiator { get; }
        public static Expression<Func<Problem, string>> InitiatorFullName =>
            User.FullNameExpression.Substitute<Problem, User, string>(p => p.Initiator);
        public static SpecificationBuilder<Problem, User> UserIsInitiator =>
            new SpecificationBuilder<Problem, User>((problem, user) => problem.InitiatorID == user.IMObjID);

        #endregion

        #region Queue

        public Guid? QueueID { get; set; }
        public virtual Group Queue { get; }
        public static SpecificationBuilder<Problem, User> UserIsInGroup =>
            new SpecificationBuilder<Problem, User>((problem, user) => problem.Queue != null && problem.Queue.QueueUsers.Any(x => x.UserID == user.IMObjID));

        #endregion

        #region Executor

        public Guid? ExecutorID { get; set; }
        public virtual User Executor { get; }
        public static SpecificationBuilder<Problem, User> UserIsExecutor =>
            new SpecificationBuilder<Problem, User>((problem, user) => problem.ExecutorID == user.IMObjID);
        public static Expression<Func<Problem, string>> ExecutorFullName =>
            User.FullNameExpression.Substitute<Problem, User, string>(problem => problem.Executor);

        #endregion

        #region Service

        public Guid? ServiceID { get; set; }
        public virtual Service Service { get; }

        #endregion

        #region Запросы на изменения

        /// <summary>
        /// Возвращает коллекцию запросов на изменения, ассоциированных с проблемой.
        /// </summary>
        public virtual ICollection<ManyToMany<Problem, ChangeRequest>> ChangeRequests { get; } = new HashSet<ManyToMany<Problem, ChangeRequest>>();

        #endregion
    }
}
