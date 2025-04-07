using System;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Предоставляет свойство доступа к доп. параметрам объекта.
/// </summary>
public interface IHaveFormValues
{
    /// <summary>
    /// Возвращает или задает доп. параметры объекта.
    /// </summary>
    FormValues FormValues { get; set; }

    /// <summary>
    /// Получает или задает уникальный идентификатор набора значений формы. 
    /// </summary>
    long? FormValuesID { get; set; }

    /// <summary>
    /// Возвращает или задает уникальный идентификатор шаблона формы доп. параметров объекта.
    /// </summary>
    Guid? FormID { get; set; }
}