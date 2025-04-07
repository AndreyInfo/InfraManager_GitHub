using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

/// <summary>
/// Внутренний интерфейся, для работы с линиями поддержки у сервисов, элементов и услуг сервиса
/// интерфейся был создан, что бы использовать DI
/// </summary>
public interface ISupportLineBLL
{
    /// <summary>
    /// При наследование элемента или услуги сервиса от сервиса
    /// присваеиват те же линии поддержки, что и у сервиса, объекту
    /// </summary>
    /// <param name="objetID"></param>
    /// <param name="serviceID"></param>
    /// <param name="classID"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CopySupportLineFromServiceAsync(Guid objetID, Guid serviceID, ObjectClass classID, CancellationToken cancellationToken);

    /// <summary>
    /// Получение моделей линий поддержки для отображения на фронту
    /// </summary>
    /// <param name="obcjectID"></param>
    /// <param name="objectClassID"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SupportLineResponsibleDetails[]> GetResponsibleObjectLineAsync(Guid obcjectID, ObjectClass objectClassID, CancellationToken cancellationToken);

    /// <summary>
    /// Изменение списка линий поддержки
    /// Одинаковые не трогает, те которые отсутсвуют удаляет, новые добвляет
    /// </summary>
    /// <param name="models"></param>
    /// <param name="objecID"></param>
    /// <param name="objectClassID"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveAsync(IEnumerable<SupportLineResponsibleDetails> models, Guid objecID, ObjectClass objectClassID, CancellationToken cancellationToken = default);
}
