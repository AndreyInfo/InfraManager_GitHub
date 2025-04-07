using System;

namespace InfraManager.Web.Models
{
    /// <summary>
    /// Универсальная модель, описывающая замену поля сущности.
    /// </summary>
    public sealed class SetFieldRequestUI
    {
        /// <summary>
        /// Идентификатор объекта, для котрого производится изменение
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Идентификатор класса объекта.
        /// </summary>
        public int ObjClassID { get; set; }

        /// <summary>
        /// используется для точного указания класса объекта, когда ObjClassID может соответствовать нескольким классам
        /// </summary>
        public int ClassID { get; set; }

        /// <summary>
        /// Имя поля, значение которого меняется
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Исхоное значение, кторое пользователь правил
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// новое значение, введенное пользователем
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// Флаг обхода проврки конкуренции по старому значению
        /// </summary>
        public bool ReplaceAnyway { get; set; }
    }
}