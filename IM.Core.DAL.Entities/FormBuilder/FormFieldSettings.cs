using System;

namespace InfraManager.DAL.FormBuilder;

/// <summary>
/// Представляет настройку доп. параметра.
/// </summary>
public class FormFieldSettings
{
    /// <summary>
    /// Уникальный идентификатор сущности.
    /// </summary>
    public long ID { get; init; }

    /// <summary>
    /// Уникальный идентификатор пользователя, в контексте которого действует настройка.
    /// </summary>
    public Guid UserID { get; init; }

    /// <summary>
    /// Уникальный идентификатор поля, для которого настройка.
    /// </summary>
    public Guid FieldID { get; init; }

    /// <summary>
    /// Ширина колонки в таблице.
    /// </summary>
    public int Width { get; set; }
}