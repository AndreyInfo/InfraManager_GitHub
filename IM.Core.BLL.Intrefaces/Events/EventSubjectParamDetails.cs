using System;

namespace InfraManager.BLL.Events
{
    public class EventSubjectParamDetails
    {
        /// <summary>
        ///     Иденитфикатор
        /// </summary>
        public Guid ID { get; init; }

        /// <summary>
        ///     Изменяемое поле
        /// </summary>
        public string ParamName { get; init; }

        /// <summary>
        ///     Старое значение поля (или null)
        /// </summary>
        public string ParamOldValue { get; init; }

        /// <summary>
        ///     Новое значение поля
        /// </summary>
        public string ParamNewValue { get; init; }
    }
}
