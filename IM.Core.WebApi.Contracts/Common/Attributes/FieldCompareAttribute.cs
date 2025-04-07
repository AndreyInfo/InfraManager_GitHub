using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes
{
    /// <summary>
    /// Задает Параметры сравнения и формирования описания события изменения для поля.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FieldCompareAttribute : Attribute
    {
        public FieldCompareAttribute(string userFieldName)
        {
            UserFieldName = userFieldName;
        }

        public FieldCompareAttribute(string userFieldName, int nameOrder)
        {
            UserFieldName = userFieldName;
            NameOrder = nameOrder;
        }
        /// <summary>
        /// Наименование поля для записи в историю
        /// </summary>
        public string UserFieldName { get; private set; }
        
        /// <summary>
        /// Признак имени - значение этого поля используется для обозначения объекта в событии. 
        /// В полном представлении используются все поля с установленным номером в порядке убывания.
        /// значение 1 - у основного названия (краткого)
        /// </summary>
        public int? NameOrder { get; private set; }
        
        /// <summary>
        /// Представление null значения в событии
        /// </summary>
        public string NullValueLabel { get; set; }
        
        /// <summary>
        /// Формат представления поля
        /// </summary>
        public string Format { get; set; }
        
        /// <summary>
        /// Наименование атрибта значения поля в JSON-сериализованном виде (передаваемое в SetField)
        /// </summary>
        public string SetFieldProperty { get; set; }
        
        /// <summary>
        /// Наименование атрибута, идентифицирующего элемент спискового поля.
        /// </summary>
        public string ListIDField { get; set; }

        /// <summary>
        /// Флаг использования особого метода работы с данными поля
        /// </summary>
        public bool IsUseCustomValues { get; set; }
    }
}
