using System;

namespace InfraManager.BLL.FormBuilder.Contracts;

/// <summary>
/// Объектное представление для <see cref="FormBuilderFormTabFieldDetails.SpecialFields"/>. 
/// </summary>
public class FormBuilderFieldSpecialFields
{
    /// <summary>
    /// Возвращает минимальное возможное значение для поля.
    /// Для текстовых полей используется для ограничения длины.
    /// </summary>
    public decimal? MinValue { get; init; }

    /// <summary>
    /// Возвращает максимально возможное значение для поля.
    /// Для текстовых полей используется для ограничения длины.
    /// </summary>
    public decimal? MaxValue { get; init; }

    /// <summary>
    /// Возвращает уникальный идентификатор подразделения для фильтрации значений полей по подразделению.
    /// Например поля с типом <see cref="InfraManager.DAL.FormBuilder.FieldTypes.Subdivision"/>.
    /// </summary>
    public Guid? SubdivisionID { get; init; }

    /// <summary>
    /// Возвращает уникальный идентификатор организации для фильтрации значений полей по организации.
    /// Например поля с типом <see cref="InfraManager.DAL.FormBuilder.FieldTypes.Subdivision"/>.
    /// </summary>
    public Guid? OrganizationID { get; init; }
    
    /// <summary>
    /// Возвращает список возможных значений для полей с типом <see cref="InfraManager.DAL.FormBuilder.FieldTypes.EnumComboBox"/> и <see cref="InfraManager.DAL.FormBuilder.FieldTypes.EnumRadioButton"/>.
    /// </summary>
    public FormBuilderFieldListValue[] List { get; init; } = Array.Empty<FormBuilderFieldListValue>();
}