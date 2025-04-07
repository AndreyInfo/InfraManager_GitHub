using System;

namespace InfraManager.BLL.FormBuilder.Contracts;

/// <summary>
/// Представляет входной контракт для обновления настройки одного поля формы доп. параметров.
/// </summary>
public class FormBuilderFormFieldSettingData
{
    /// <summary>
    /// Возвращает уникальный идентификатор поля, для которого эта настройка.
    /// </summary>
    public Guid? FieldID { get; init; }

    /// <summary>
    /// Возвращает ширину поля.
    /// </summary>
    public int Width { get; init; }
}