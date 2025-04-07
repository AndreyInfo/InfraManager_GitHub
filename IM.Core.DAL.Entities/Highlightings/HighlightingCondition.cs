using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Highlightings
{
    /// <summary>
    /// Этот класс представляет сущность Условие для правила выделения строк в списке
    /// </summary>
    [ObjectClassMapping(ObjectClass.HighlightingCondition)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.HighlightingCondition_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.HighlightingCondition_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.HighlightingCondition_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.HighlightingCondition_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.HighlightingCondition_Properties)]
    public class HighlightingCondition
    {
        protected HighlightingCondition()
        {
        }

        /// <summary>
        /// Возвращает идентификатор условия правила выделения строк в списке
        /// </summary>
        public Guid ID { get; init; }

        /// <summary>
        /// Возвращает идентификатор правила выделения строк в списке
        /// </summary>
        public Guid HighlightingID { get; init; }

        public virtual Highlighting Highlighting { get; init; }

        /// <summary>
        /// Возвращает наименование справочника для условия правила выделения строк в списке
        /// </summary>
        public ObjectClass? DirectoryParameter { get; init; }

        /// <summary>
        /// Возвращает наименование временного параметра для условия правила выделения строк в списке
        /// </summary>
        public HighlightingParameterEnum? EnumParameter { get; init; }

        /// <summary>
        /// Возвращает условие для правила выделения строк в списке
        /// </summary>
        public ConditionEnum Condition { get; init; }

        /// <summary>
        /// Возвращает значения цвета фона для правила выделения строк в списке
        /// </summary>
        public string BackgroundColor { get; init; }

        /// <summary>
        /// Возвращает значения цвета шрифта для правила выделения строк в списке
        /// </summary>
        public string FontColor { get; init; }

        public virtual ICollection<HighlightingConditionValue> HighlightingConditionValue { get; set; }
    }
}
