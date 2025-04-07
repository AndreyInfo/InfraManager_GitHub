using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Models
{
    public sealed class FieldValueModel
    {
        /// <summary>
        /// Имя поля, значение которого меняется
        /// </summary>
        public String Field { get; set; }
        /// <summary>
        /// Исходное значение, которое пользователь правил
        /// </summary>
        public String OldValue { get; set; }
        /// <summary>
        /// новое значение, введенное пользователем
        /// </summary>
        public String NewValue { get; set; }
        /// <summary>
        /// Флаг обхода проверки конкуренции по старому значению
        /// </summary>
        public bool ReplaceAnyway { get; set; }
    }
}
