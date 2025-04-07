using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

public interface IWorkOrderTemplateBLL
{
    /// <summary>
    /// Получение дерева состоящие из папок шаблонов и самих шаблонов
    /// Папки могут быть вложенные
    /// Последний уровень сами шаблоны
    /// </summary>
    /// <param name="parentId">идентификатор родительского элемента</param>
    /// <param name="isRoot">Получение корневого элемента</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Дочерние узлы элемента с идентификатром parentId</returns>
    Task<NodeWorkOrderTemplateTree[]> GetTreeAsync(Guid? parentId, bool isRoot, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение пути элемента в дереве
    /// От запрошиваемого до корневого
    /// </summary>
    /// <param name="id">идентификатор элемента</param>
    /// <param name="classID">параметр для распознания типа элемента(папки или шаблон)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Ветка дерева до запрашиваемого элемента</returns>
    Task<NodeWorkOrderTemplateTree[]> GetPathItemByIDAsync(Guid id, ObjectClass classID, CancellationToken cancellationToken);

    /// <summary>
    /// Получени массива данные с возможностью поиска и скролинга
    /// Если folderId, то получаем шаблоны из конкретной папки
    /// Если нет, то берем данные из общего массива
    /// </summary>
    /// <param name="filter">фильтр для поиска, скролинга и определния из какой папки брать данные</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массива шаблонов</returns>
    Task<WorkOrderTemplateDetails[]> GetDataForTableAsync(WorkOrderTemplateFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Проверяет используется ли шаблон
    /// </summary>
    /// <param name="folderID"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> HasUsedTemplateAsync(Guid folderID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение по индентификатору
    /// Производятся дополнительные вычисления, для рассчета доп полей
    /// </summary>
    /// <param name="id">идентификатор получаемой модели</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модель шаблона задания</returns>
    Task<WorkOrderTemplateDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Создает шаблон задания по аналогии
    /// </summary>
    /// <param name="model">шаблон задания</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> AddAsAsync(WorkOrderTemplateDetails model, CancellationToken cancellationToken);
}
