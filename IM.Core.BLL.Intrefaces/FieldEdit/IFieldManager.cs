using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.FieldEdit
{
    /// <summary>
    /// Обеспечивает работу со значения атрибутов объектов с использование рефлексии
    /// </summary>
    public interface IFieldManager
    {
        /// <summary>
        /// Устанвливает значение аттрибута в объекте, с провркой конкуреных проблем
        /// </summary>
        /// <param name="obj">объект, в кором меняется значения</param>
        /// <param name="fieldValue">модель, описывающая требуемое измененеи</param>
        /// <returns>Причину ошибки или null в случае успеха</returns>
        BaseError? SetFieldValue(object obj, FieldValueModel fieldValue);
        /// <summary>
        /// Сравнивает значения атрибута двух объектов
        /// </summary>
        /// <param name="oldValue">старый объект</param>
        /// <param name="newValue">новый объект</param>
        /// <param name="fieldName">имя проверяемого атрибута</param>
        /// <returns>Результат сравнеия в виде элемента перечисления</returns>
        FieldCompareResult AreFieldsSame(object oldValue, object newValue, string fieldName);
        /// <summary>
        /// Возвращает представление для занчения заданного поля в объекте
        /// </summary>
        /// <param name="value">объект</param>
        /// <param name="name">имя поля</param>
        /// <returns>строкове представление поля</returns>
        string GetFieldValueLabel(object itemValue, string name);
        /// <summary>
        /// Возвращает занчения заданного поля в объекте
        /// </summary>
        /// <param name="value">объект</param>
        /// <param name="name">имя поля</param>
        /// <returns>объект со значением указанного поля</returns>
        object GetFieldValue (object itemValue, string name);
        /// <summary>
        /// Возвращает атибут описания типа 
        /// </summary>
        /// <param name="objectType">тип, поле котрого нужно описать</param>
        /// <returns></returns>
        EntityCompareAttribute GetEntityAttribute(Type objectType);

        /// <summary>
        /// ВОзвращает аттрибут описание поля
        /// </summary>
        /// <param name="objectType">тип, поле котрого нужно описать</param>
        /// <param name="fieldName">имя описываемого поля</param>
        /// <returns></returns>
        FieldCompareAttribute GetFieldAttribute(Type objectType, string fieldName);
        /// <summary>
        /// ВОзвращает список полей типа с атрибутами описания 
        /// </summary>
        /// <param name="objectType">тип, поля для  котрого нужно описать</param>
        /// <returns></returns>
        Dictionary<string, FieldCompareAttribute> GetFieldAttributes(Type objectType);
    }
}
