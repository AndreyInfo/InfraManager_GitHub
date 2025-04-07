using Inframanager;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Collections.Generic;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.Linq;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс представляет сущность Массовый инцидент
    /// </summary>
    [ObjectClassMapping(ObjectClass.MassIncident)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.MassIncident_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.MassIncident_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.MassIncident_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)] // не трогать
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)] // не трогать
    public class MassIncident : ICreateNegotiation, IWorkflowEntity, ICreateWorkOrderReference, IHaveFormValues
    {
        #region .ctor

        protected MassIncident()
        {
            Description = new Description();
            Solution = new Description();
            Cause = new Description();
        }

        /// <summary>
        /// Создает новый массовый инцидент
        /// </summary>
        /// <param name="informationChannelID">Идентификатор канала приема</param>
        /// <param name="typeID">Тип массового инцидента</param>
        public MassIncident(short informationChannelID, int typeID) : this()
        {
            InformationChannelID = informationChannelID;
            TypeID = typeID;
            UtcCreatedAt = DateTime.UtcNow;
            UtcDateModified = UtcCreatedAt;
            OwnedByUserID = User.NullUserId;            
        }

        #endregion

        #region Идентификация

        /// <summary>
        /// Возвращает идентификатор
        /// </summary>
        public int ID { get; }
        /// <summary>
        /// Возвращает глобальный идентификатор
        /// </summary>
        public Guid IMObjID { get; }
        /// <summary>
        /// Возвращает или задает наименование массового инцидента (оно же Краткое описание)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор версии
        /// </summary>
        public byte[] RowVersion { get; set; }

        #endregion

        #region Описание

        /// <summary>
        /// Возвращает или задает идентификатор канала приема
        /// </summary>
        public short InformationChannelID { get; set; }
        
        public virtual MassIncidentInformationChannel MassIncidentInformationChannel { get; init; }
        /// <summary>
        /// Возвращает или задает идентификатор типа
        /// </summary>
        public int TypeID { get; set; }

        public virtual MassIncidentType Type { get; init; }

        public int OwnedByUserID { get; private set; }

        private User _ownedBy;
        /// <summary>
        /// Возвращает или задает ссылку на владельца
        /// </summary>
        public virtual User OwnedBy
        {
            get => _ownedBy;
            set
            {
                if (_ownedBy?.ID != (value?.ID ?? User.NullUserId))
                {
                    OwnedByUserID = value?.ID ?? User.NullUserId;
                }
                _ownedBy = value;
            }
        }

        public static SpecificationBuilder<MassIncident, User> UserIsOwner =>
            new SpecificationBuilder<MassIncident, User>((massIncident, user) => massIncident.OwnedByUserID == user.ID);

        public static Expression<Func<MassIncident, string>> OwnerFullName =>
            User.FullNameExpression.Substitute<MassIncident, User, string>(x => x.OwnedBy);
        
        /// <summary>
        /// Возвращает или задает идентификатор группы
        /// </summary>
        public Guid ExecutedByGroupID { get; set; }

        private Group _executedByGroup;

        /// <summary>
        /// Возвращает группу
        /// </summary>
        public virtual Group ExecutedByGroup 
        {
            get => _executedByGroup;
            set
            {
                if (_executedByGroup?.IMObjID != (value?.IMObjID ?? Group.NullGroupID))
                {
                    ExecutedByGroupID = value?.IMObjID ?? Group.NullGroupID;
                }

                _executedByGroup = value;
            }
        }

        public static SpecificationBuilder<MassIncident, User> UserIsInGroup =>
            new SpecificationBuilder<MassIncident, User>(
                (massIncident, user) => massIncident.ExecutedByGroup.QueueUsers.Any(x => x.UserID == user.IMObjID));

        public static Specification<MassIncident> ExecutedByUserAndGroup =>
            new Specification<MassIncident>(
                massIncident => massIncident.ExecutedByGroupID != Group.NullGroupID 
                    && massIncident.ExecutedByUserID != User.NullUserId);

        public int ExecutedByUserID { get; private set; }

        private User _executedByUser;

        /// <summary>
        /// Возвращает или задает ссылку на исполнителя
        /// </summary>
        public virtual User ExecutedByUser
        {
            get => _executedByUser;
            set
            {
                if (_executedByUser?.ID != (value?.ID ?? User.NullUserId))
                {
                    ExecutedByUserID = value?.ID ?? User.NullUserId;
                }

                _executedByUser = value;
            }
        }

        public static SpecificationBuilder<MassIncident, User> UserIsExecutor =>
            new SpecificationBuilder<MassIncident, User>((massIncident, user) => massIncident.ExecutedByUserID == user.ID);

        public static Expression<Func<MassIncident, string>> ExecutorFullName =>
            User.FullNameExpression.Substitute<MassIncident, User, string>(x => x.ExecutedByUser);

        /// <summary>
        /// Возвращает полное описание
        /// </summary>
        public Description Description { get; set; }

        /// <summary>
        /// Возвращает или задает категорию технических сбоев
        /// </summary>
        public int? TechnicalFailureCategoryID { get; set; }
        public virtual TechnicalFailureCategory TechnicalFailureCategory { get; init; }

        #endregion

        #region Сервисы

        public Guid ServiceID { get; set; }
        public virtual Service Service { get; }

        /// <summary>
        /// Возвращает или задает идентификатор SLA
        /// </summary>
        public int? OperationalLevelAgreementID { get; set; }
        /// <summary>
        /// Возвращает ссылку на коллекцию сервисов, затронутых массовым инцидентом
        /// </summary>
        public virtual ICollection<ManyToMany<MassIncident, Service>> AffectedServices { get; } =
            new HashSet<ManyToMany<MassIncident, Service>>();

        #endregion

        #region Анализ и решение

        /// <summary>
        /// Возвращает или задает идентификатор приоритета
        /// </summary>
        public Guid PriorityID { get; set; }

        public virtual Priority Priority { get; init; }

        /// <summary>
        /// Возвращает или задает идентификатор критичности
        /// </summary>
        public Guid? CriticalityID { get; set; }

        public virtual Criticality Criticality { get; init; }

        /// <summary>
        /// Возвращает или задает идентификатор причины
        /// </summary>
        public int? CauseID { get; set; }
        public virtual MassIncidentCause MassIncidentCause { get; init; }
        
        /// <summary>
        /// Возвращает или задает причину
        /// </summary>
        public Description Cause { get; set; }
        /// <summary>
        /// Возвращает или задает решение
        /// </summary>
        public Description Solution { get; set; }
        /// <summary>
        /// Возвращает или задает дату требуемого завершения
        /// </summary>
        public DateTime? UtcCloseUntil { get; set; }

        #endregion

        #region История

        /// <summary>
        /// Возвращает дату создания
        /// </summary>
        public DateTime UtcCreatedAt { get; set; }

        private User _createdBy;

        /// <summary>
        /// Возвращает идентификатор инициатора
        /// </summary>
        public int CreatedByUserID { get; private set; }

        /// <summary>
        /// Возвращает инициатора
        /// </summary>
        public virtual User CreatedBy 
        {
            get => _createdBy;
            set
            {
                if (_createdBy?.ID != value.ID)
                {
                    CreatedByUserID = value.ID;
                }

                _createdBy = value;
            }
        }

        public static Expression<Func<MassIncident, string>> ClientFullName =>
            User.FullNameExpression.Substitute<MassIncident, User, string>(x => x.CreatedBy);

        public static SpecificationBuilder<MassIncident, User> UserIsCreator =>
            new SpecificationBuilder<MassIncident, User>((massIncident, user) => massIncident.CreatedByUserID == user.ID);

        /// <summary>
        /// Возвращает дату последнего изменения
        /// </summary>
        public DateTime UtcDateModified { get; set; }

        /// <summary>
        /// Возвращает дату выполнения массового инцидента
        /// </summary>
        public DateTime? UtcDateAccomplished { get; set; }

        /// <summary>
        /// Возвращает дату завершения массового инцидента
        /// </summary>
        public DateTime? UtcDateClosed { get; set; }

        #endregion

        #region Рабочая процедура

        /// <summary>
        /// Возвращает или задает дату открытия
        /// </summary>
        public DateTime? UtcOpenedAt { get; set; }
        /// <summary>
        /// Возвращает или задает дату регистрации
        /// </summary>
        public DateTime? UtcRegisteredAt { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор состояния рабочей процедуры
        /// </summary>
        public string EntityStateID { get; set; }
        /// <summary>
        /// Возвращает или задает наименование состояния рабочей процедуры
        /// </summary>
        public string EntityStateName { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор версии схемы рабочей процедуры
        /// </summary>
        public Guid? WorkflowSchemeID { get; set; }
        /// <summary>
        /// Возвращает или задает версию рабочей процедуры
        /// </summary>
        public string WorkflowSchemeVersion { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор рабочей процедуры
        /// </summary>
        public string WorkflowSchemeIdentifier { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор состояния перехода
        /// </summary>
        public string TargetEntityStateID { get; set; }

        #endregion

        #region Заявки

        /// <summary>
        /// Возвращает колелкцию заявок, ассоциированных с массовым инцидентом
        /// </summary>
        public virtual ICollection<ManyToMany<MassIncident, Call>> Calls { get; } = new HashSet<ManyToMany<MassIncident, Call>>();

        #endregion

        #region Проблемы

        /// <summary>
        /// Возвращает ссылку на коллекцию проблем, ассоциированных с массовым инцидентом
        /// </summary>
        public virtual ICollection<ManyToMany<MassIncident, Problem>> Problems { get; } = new HashSet<ManyToMany<MassIncident, Problem>>();

        #endregion

        #region Запросы на изменения

        /// <summary>
        /// Возвращает ссылку на коллекцию запросов на изменения, ассоциированных с массовым инцидентом
        /// </summary>
        public virtual ICollection<ManyToMany<MassIncident, ChangeRequest>> ChangeRequests { get; } =
            new HashSet<ManyToMany<MassIncident, ChangeRequest>>();

        #endregion

        #region Согласования

        public Negotiation CreateNegotiation()
        {
            return new MassiveIncidentNegotiation(IMObjID);
        }

        #endregion

        #region Referenced WorkOrders

        public string ReferenceName => $"IM-MI-{ID}";
        public WorkOrderReference CreateWorkOrderReference()
        {
            return new WorkOrderReference<MassIncident>(this);
        }

        #endregion

        #region FormValues

        public long? FormValuesID { get; set; }
        public Guid? FormID { get; set; }

        public virtual FormValues FormValues { get; set; }

        #endregion
    }
}
