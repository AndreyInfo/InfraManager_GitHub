using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure.JobTitles;

/// <summary>
/// Бизнес логика с сущностью Должность
/// </summary>
public interface IJobTitleBLL
{
    /// <summary>
    /// Получение долнжности по Guid id
    /// </summary>
    /// <param name="id">идентификатор должности</param>
    /// <param name="cancellationToken"></param>
    Task<JobTitleDetails> DetailsAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Получение списка Должностей, по фильтру
    /// </summary>
    /// <param name="filterBy">Условия выборки данных</param>\
    /// <param name="pageBy">Условия сортировки и постраничного вывода данных</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных должностей, удовлетворяющих условию выборки</returns>
    Task<JobTitleDetails[]> GetDetailsPageAsync(JobTitleListFilter filterBy, ClientPageFilter<JobTitle> pageBy, CancellationToken cancellationToken);
    /// <summary>
    /// Получение выборки данных должностей
    /// </summary>
    /// <param name="filterBy">Условия выборки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных должностей, удовлетворяющих условиям выборки</returns>
    Task<JobTitleDetails[]> GetDetailsArrayAsync(JobTitleListFilter filterBy, CancellationToken cancellationToken);
    /// <summary>
    /// Создает новую должность
    /// </summary>
    /// <param name="data">Данные новой должности</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные новой должности</returns>
    Task<JobTitleDetails> AddAsync(JobTitleData data, CancellationToken cancellationToken);
    /// <summary>
    /// Изменяет существующую должность
    /// </summary>
    /// <param name="id">Идентификатор должности</param>
    /// <param name="data">Новые данные должности</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные новой должности</returns>
    Task<JobTitleDetails> UpdateAsync(int id, JobTitleData data, CancellationToken cancellationToken);
    /// <summary>
    /// Удаление существующей должности
    /// </summary>
    /// <param name="id">Идентификатор должности</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка должностей, с сортировкой по ViewName, фильтрацией и пагинацией 
    /// </summary>
    /// <param name="filter">фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>массив моделей </returns>
    Task<JobTitleDetails[]> GetPaggingAsync(BaseFilter filter, CancellationToken cancellationToken);
}
