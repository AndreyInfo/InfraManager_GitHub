using System;

namespace InfraManager.BLL.AssetsManagement.Hardware;

/// <summary>
/// Этот класс описывает параметры фильра списка оборудования, используемого клиентом.
/// </summary>
public class ClientsHardwareListFilter : ListFilter
{
    /// <summary>
    /// Возвращает или задает уникальный идентификатор пользователя, использующего оборудование.
    /// </summary>
    public Guid ClientID { get; init; }
}