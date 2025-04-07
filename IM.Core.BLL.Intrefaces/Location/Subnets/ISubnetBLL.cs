using InfraManager.BLL.CrudWeb;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace InfraManager.BLL.Location.Subnets;

/// <summary>
/// Бизнес логика с подсетями
/// </summary>
public interface ISubnetBLL
{
    /// <summary>
    /// Получение подсетей привязанных к зданию
    /// </summary>
    /// <param name="buildID">иддентификатор здания подсети которого хотят получить</param>
    /// <param name="search">строка для поиска по имени</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SubnetDetails[]> GetSubnetsByBuildingIDAsync(int buildID, string search, CancellationToken cancellationToken);

    /// <summary>
    /// Получение подсети по id
    /// </summary>
    /// <param name="id">идентификатор подсети</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SubnetDetails> GetByIDAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление
    /// Имеется проверка на существование подсети с таким же именем в здании
    /// </summary>
    /// <param name="model">модель добавляемой подсети</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> AddAsync(SubnetDetails model, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление
    /// Имеется проверка на существование подсети с таким же именем в здании
    /// </summary>
    /// <param name="model">модель добавляемой подсети</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(SubnetDetails model, CancellationToken cancellationToken);

    /// <summary>
    /// Множественное удаление
    /// </summary>
    /// <param name="deleteModels">модели для удаления</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(IEnumerable<DeleteModel<int>> deleteModels, CancellationToken cancellationToken);
}

