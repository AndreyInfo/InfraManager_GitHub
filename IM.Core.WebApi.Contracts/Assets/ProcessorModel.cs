using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Assets
{
    /// <summary>
    /// Модель объекта "Модель процессора"
    /// </summary>
    public sealed class ProcessorModelModel
    {
        /// <summary>
        /// Идентификатор модели
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Наименование типа
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Наименование модели
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// Производитель
        /// </summary>
        public string ManufactorName { get; set; }
        /// <summary>
        /// Количесво ядер
        /// </summary>
        public string Cores { get; set; }
    }
}
