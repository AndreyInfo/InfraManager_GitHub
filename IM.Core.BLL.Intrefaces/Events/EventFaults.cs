using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Events
{
    /// <summary>
    /// Ошибки формирования события
    /// </summary>
    public enum EventFaults
    {
        /// <summary>
        /// Общая ошибка
        /// </summary>
        GeneralError,
        /// <summary>
        /// Типы сравниваемых объектов отличаются
        /// </summary>
        ObjectTypeDiffers,
        /// <summary>
        /// Объекты идентичны, события изменения нет
        /// </summary>
        ObjectsAreSame,
        /// <summary>
        /// Не передан объект для сравнения
        /// </summary>
        ObjectIsNull,
        /// <summary>
        /// В классе объекта не определны атрибуты сравнения.
        /// </summary>
        ComparableAttrubutesNotSet,
    }
}
