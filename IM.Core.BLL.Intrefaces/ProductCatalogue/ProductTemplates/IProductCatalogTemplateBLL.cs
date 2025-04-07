using InfraManager.BLL.ProductCatalogue.Classes;
using InfraManager.DAL.ProductCatalogue;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductTemplates;

public interface IProductCatalogTemplateBLL
{
    /// <summary>
    /// Возвращает идентификаторы указанных шаблонов вместе с подчиненными
    /// </summary>
    /// <param name="templates">Идентификаторы шаблонов для поиска</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Шаблоны для поиска и подчиненные шаблоны, входящие в поддеревья</returns>
    Task<HashSet<ProductTemplate>> GetSubTemplatesAsync(IEnumerable<ProductTemplate> templates, CancellationToken cancellationToken);

    /// <summary>
    /// Получение узлов дерева шаблонов продуктов по фильтру
    /// </summary>
    /// <param name="filter">фильтр получение</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>модели шаблонов</returns>
    Task<ProductTemplateInfo[]> GetNodesAsync(ProductTemplateTreeFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение шаблона по идентификатору
    /// </summary>
    /// <param name="id">идентификатор шаблона</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>модель шаблоны</returns>
    Task<ProductTemplateInfo> GetByID(ProductTemplate id, CancellationToken cancellationToken);
}