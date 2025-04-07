using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Models
{
    /// <summary>
    /// Универсальная модель, описаавыющая зименение поля сущности.
    /// </summary>
    public sealed class SetFieldRequest : ObjectIDModel
    {
        /// <summary>
        /// ИЗменяемое значение в объекте
        /// </summary>
        public FieldValueModel FieldValue { get; set; }
        /// <summary>
        /// Дополнитльные параметры изменения... ???
        /// </summary>
        public String[] Params { get; set; }
    }
}
