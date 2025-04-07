using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL;

/// <summary>
/// Аналог GuidePaggingFacade
/// Используется только в тех случаях, 
/// когда невозможно получить все данные для пагинации и фильтрации одним запросом к БД
/// Например получение всех листиков
/// Желательно неиспользовать
/// </summary>
public interface IClientSideFilterer<TDetails, TTable>
{
    Task<TDetails[]> GetPaggingAsync(IEnumerable<TDetails> source,
            BaseFilter filter,
            Func<TDetails, bool> searchPredicate = null,
            CancellationToken cancellationToken = default);
}
