using System;

using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ServiceCatalogue
{
    /// <summary>
    /// Представляет расширенный фильтр для SLA, содержащий отображение SLA по уровням.
    /// </summary>
    public sealed class SLAFilter : BaseFilter
    {
        /// <summary>
        /// Включить собственные SLA
        /// </summary>
        public bool IncludeOwn { get; init; }

        /// <summary>
        /// Включить SLA верхних уровней.
        /// </summary>
        public bool IncludeTopLevels { get; init; }

        /// <summary>
        /// Включить SLA вложенных уровней.
        /// </summary>
        public bool IncludeNestedLevels { get; init; }
        
        /// <summary>
        /// Включать ли SLA которые заключены с пользователем или его подразделением
        /// </summary>
        public Guid? UserID { get; init; }

        /// <summary>
        /// Выбранный элемент от которого будем возвращать SLA.
        /// </summary>
        public ConcludedDetails SelectedItem => new() { ClassID = ClassID, ObjectID = ObjectID };

        public ObjectClass ClassID { get; init; }

        public Guid ObjectID { get; init; }
    }
}
