using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

/// <summary>
/// Query, которая возвращает список сервисов которые заключенны с OLA с фильтрацией
/// </summary>
public interface IOperationalLevelAgreementServiceQuery
{
    /// <summary>
    /// возвращает список сервисов которые заключенны с OLA с фильтрацией
    /// </summary>
    /// <param name="query">Query с фильтрацией</param>
    /// <param name="take">Сколько записей взять</param>
    /// <param name="skip">Сколько записей пропустить</param>
    /// <param name="ascending">Сортировка, если нужна</param>
    /// <param name="predicate">Поле, по которому сортировать</param>
    /// <param name="search">Строка поиска</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<OperationalLevelAgreementServiceListItem[]> ExecuteAsync(
        IExecutableQuery<ManyToMany<OperationalLevelAgreement, Service>> query,
        int take,
        int skip,
        bool ascending,
        Expression<Func<OperationalLevelAgreementServiceListItem, object>> predicate,
        string search,
        CancellationToken cancellationToken = default);
}