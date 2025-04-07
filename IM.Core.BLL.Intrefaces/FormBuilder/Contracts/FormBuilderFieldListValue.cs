using System;

namespace InfraManager.BLL.FormBuilder.Contracts;

/// <summary>
/// Представляет элемент списка для полей с типом <see cref="InfraManager.DAL.FormBuilder.FieldTypes.EnumComboBox"/> и <see cref="InfraManager.DAL.FormBuilder.FieldTypes.EnumRadioButton"/>.
/// </summary>
public class FormBuilderFieldListValue
{
    /// <summary>
    /// Уникальный идентификатор элемента списка.
    /// </summary>
    public Guid? ID { get; init; }

    /// <summary>
    /// Отображаемой значение элемента списка.
    /// </summary>
    public string Value { get; init; }
}