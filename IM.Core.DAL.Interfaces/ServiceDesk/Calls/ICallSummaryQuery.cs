using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls;

public interface ICallSummaryQuery
{
    /// <summary>
    /// Получение данных с пагинацией и сортировкой
    /// </summary>
    /// <param name="query">запрос с условием</param>
    /// <param name="filter">фильтр для пагинации</param>
    /// <param name="propertySort">свойство для сортировка</param>
    /// <param name="mappedValues">поля для вложенных сортировок</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модели кратких описаний заявок</returns>
    public Task<CallSummaryModelItem[]> ExecuteAsync(IExecutableQuery<CallSummary> query, PaggingFilter filter, Sort propertySort, string[] mappedValues, CancellationToken cancellationToken);
}