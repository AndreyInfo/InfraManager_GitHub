using Inframanager;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.DAL.Highlightings
{
    /// <summary>
    /// Этот класс представляет сущность Значение условий для правила выделения строк в списке
    /// </summary>
    [ObjectClassMapping(ObjectClass.HighlightingConditionValue)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.HighlightingConditionValue_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.HighlightingConditionValue_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.HighlightingConditionValue_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.HighlightingConditionValue_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.HighlightingConditionValue_Properties)]
    public class HighlightingConditionValue
    {
        /// <summary>
        /// Возвращает идентификатор значения условия правила выделения строк в списке
        /// </summary>
        public Guid ID { get; init; }

        /// <summary>
        /// Возвращает идентификатор условия правила выделения строк в списке
        /// </summary>
        public Guid HighlightingConditionID { get; set; }

        public virtual HighlightingCondition HighlightingCondition { get; init; }

        /// <summary>
        /// Возвращает идентификатор сущности Приоритет
        /// </summary>
        public Guid? PriorityID { get; set; }

        public virtual Priority Priority { get; init; }

        /// <summary>
        /// Возвращает идентификатор сущности Срочность
        /// </summary>
        public Guid? UrgencyID { get; set; }

        public virtual Urgency Urgency { get; init; }

        /// <summary>
        /// Возвращает идентификатор сущности Влияние
        /// </summary>
        public Guid? InfluenceID { get; set; }

        public virtual Influence Influence { get; init; }

        /// <summary>
        /// Возвращает идентификатор значения SLA
        /// </summary>
        public Guid? SlaID { get; set; }

        public virtual ServiceLevelAgreement Sla { get; init; }

        /// <summary>
        /// Возвращает первое значение условия
        /// </summary>
        public int? IntValue1 { get; set; }

        /// <summary>
        /// Возвращает второе значение условия
        /// </summary>
        public int? IntValue2 { get; set; }

        /// <summary>
        /// Возвращает второе значение условия
        /// </summary>
        public string? StringValue { get; set; }
    }
}
