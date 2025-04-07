using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes
{
    /// <summary>
    /// Для использования при необходимост кастомно сериализации/десириализации полей объекта
    /// </summary>
    public interface ICustomValueManager
    {
        /// <summary>
        /// Установка значения поля объекта по указанномк строковому предвталению.
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="value"></param>
        void SetField(string filedName, string value);
        /// <summary>
        /// Проврка совпадения текущих значений поля
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="value"></param>
        bool CheckSameFieldValue(string filedName, string value);
    }
}
