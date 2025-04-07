using System;

namespace InfraManager.BLL.FormBuilder.Contracts;

/// <summary>
/// Представляет входной контракт для обновления настроек формы под. параметров в контексте пользователя.
/// </summary>
public class FormBuilderFormSettingData
{
    /// <summary>
    /// Возвращает массив настроек для каждого поля.
    /// </summary>
    public FormBuilderFormFieldSettingData[] FieldSettings { get; init; } = Array.Empty<FormBuilderFormFieldSettingData>();
}