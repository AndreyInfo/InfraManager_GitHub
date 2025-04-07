using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Models
{
    /// <summary>
    /// Универсальная модлеь, описывающая результат изменеия сущности
    /// </summary>
    public sealed class SetFieldResult : ObjectIDModel
    {
        public SetFieldResult(ObjectIDModel objectIDModel = null) : base(objectIDModel)
        {

        }
        /// <summary>
        /// Текущее значение
        /// </summary>
        public object CurrentObjectValue { get; set; }
        /// <summary>
        /// Допонительное сообщение по исполнению
        /// </summary>
        public string Message { get; set; }
    }
}
