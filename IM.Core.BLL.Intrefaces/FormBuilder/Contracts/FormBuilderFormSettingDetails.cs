using System;

namespace InfraManager.BLL.FormBuilder.Contracts;

/// <summary>
/// Представляет выходной контракт настроек одной формы доп. параметров.
/// </summary>
public class FormBuilderFormSettingDetails
{
    /// <summary>
    /// Возвращает массив настроек полей формы.
    /// </summary>
    public FormBuilderFormFieldSettingDetails[] FieldSettings { get; init; } = Array.Empty<FormBuilderFormFieldSettingDetails>();
}