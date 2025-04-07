using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk;

/// <summary>
/// Представляет запрос получения шаблона формы доп. параметров.
/// </summary>
public class GetFormBuilderRequest
{
    /// <summary>
    /// Класс объекта, для которого получить шаблон формы.
    /// </summary>
    public ObjectClass ClassID { get; init; }

    /// <summary>
    /// Уникальный идентификатор объекта, для которого получить шаблон формы.
    /// </summary>
    public Guid ObjectID { get; init; }
}