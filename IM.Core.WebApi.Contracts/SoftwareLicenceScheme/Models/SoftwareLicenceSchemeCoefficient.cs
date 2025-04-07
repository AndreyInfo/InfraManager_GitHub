using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    /// <summary>
    /// информация о коэффициенте типа процессора 
    /// </summary>
    public class SoftwareLicenceSchemeCoefficient
    {
        /// <summary>
        /// Идентификатор типа процессора
        /// </summary>
        [FieldCompare("Идентификатор Типа процессора", SetFieldProperty = "")]
        public Guid ProcessorTypeID { get; set; }
        /// <summary>
        /// Коэффициент для данного типа процессора
        /// </summary>
        [FieldCompare("Коэффициент", SetFieldProperty = "")]
        public int Coefficient { get; set; }
    }
}
