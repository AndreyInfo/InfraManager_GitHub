using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Highlightings
{
    /// <summary>
    /// Этот класс представляет сущность Правило выделения строк в списке
    /// </summary>
    [ObjectClassMapping(ObjectClass.Highlighting)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Highlighting_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Highlighting_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Highlighting_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Highlighting_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Highlighting_Properties)]
    public class Highlighting
    {

        protected Highlighting()
        {

        }

        /// <summary>
        /// Возвращает идентификатор правила выделения строк в списке
        /// </summary>
        public Guid ID { get; init; }

        /// <summary>
        /// Возвращает название правила выделения строк в списке
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Возвращает номер последовательности правила выделения строк в списке
        /// </summary>
        public int Sequence { get; init; }

        public virtual ICollection<HighlightingCondition> HighlightingCondition { get; init; }
    }
}
