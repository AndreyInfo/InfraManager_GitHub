using System;
using InfraManager.DAL;

namespace InfraManager.BLL.AssetsManagement.Hardware;

/// <summary>
/// Этот класс описывает фильтр списка оборудования.
/// </summary>
public class AssetSearchListFilter : ListFilter
{
    /// <summary>
    /// Возвращает или задает список идентификаторов типов оборудования.
    /// </summary>
    public Guid[] TypesID { get; init; }

    /// <summary>
    /// Возвращает или задает список идентификаторов моделей оборудования.
    /// </summary>
    public Guid[] ModelsID { get; init; }

    /// <summary>
    /// Возвращает или задает класс местоположения.
    /// </summary>
    public ObjectClass? LocationClassID { get; init; }

    /// <summary>
    /// Возвращает или задает уникальный идентификатор местоположения.
    /// </summary>
    public Guid? LocationID { get; init; }

    /// <summary>
    /// Возвращает или задает тип фильтрации по элементам оргструктуры.
    /// Определяет по какому полю будет выполняться фильтрация по выбранным элементам оргструктуры.
    /// </summary>
    public UserTreeSettings.FiltrationFieldEnum OrgStructureFilterType { get; init; }

    /// <summary>
    /// Возвращает или задает класс выбранного элемента оргструктуры.
    /// </summary>
    public ObjectClass? OrgStructureObjectClassID { get; init; }

    /// <summary>
    /// Возвращает или задает уникальный идентификатор выбранного элемента оргструктуры.
    /// </summary>
    public Guid? OrgStructureObjectID { get; init; }

    /// <summary>
    /// Возвращает или задает текст для поиска в списке оборудования.
    /// </summary>
    public string SearchText { get; init; }
}